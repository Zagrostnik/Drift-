
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(VehicleMotor))]
[RequireComponent(typeof(NavMeshAgent))]
public class PoliceAI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("AI Settings")]
    [SerializeField] private float steeringAngle = 45f;
    [SerializeField] private float catchDistance = 3.5f;
    [SerializeField] private float updatePathRate = 0.3f;

    [Header("NavMesh")]
    [SerializeField] private float stoppingDistance = 2f;

    private VehicleMotor motor;
    private NavMeshAgent agent;

    private bool caughtPlayer;
    private float nextPathUpdate;

    private void Awake()
    {
        motor = GetComponent<VehicleMotor>();
        agent = GetComponent<NavMeshAgent>();

        // Агент ищет путь, но сам машину не перемещает
        agent.updatePosition = false;
        agent.updateRotation = false;

        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = false;
    }

    private void Start()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    private void FixedUpdate()
    {
        if (player == null || caughtPlayer)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        // Проверка по расстоянию
        if (distance <= catchDistance)
        {
            CatchPlayer();
            return;
        }

        // Обновляем маршрут с определённым интервалом
        if (Time.time >= nextPathUpdate)
        {
            agent.SetDestination(player.position);
            nextPathUpdate = Time.time + updatePathRate;
        }

        // Если маршрут не найден
        if (!agent.hasPath || agent.pathPending)
        {
            motor.Move(0f, 0f, false);
            return;
        }

        // Получаем следующую точку маршрута
        Vector3 targetPosition = agent.steeringTarget;

        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        // Рассчитываем угол поворота
        Vector3 localTarget = transform.InverseTransformPoint(
            targetPosition
        );

        float angle = Mathf.Atan2(
            localTarget.x,
            localTarget.z
        ) * Mathf.Rad2Deg;

        float steering = Mathf.Clamp(
            angle / steeringAngle,
            -1f,
            1f
        );

        // Если машина сильно отклонена от маршрута,
        // уменьшаем скорость поворота
        float throttle = Mathf.Abs(angle) > 90f ? 0.3f : 1f;

        motor.Move(throttle, steering, false);

        // Синхронизируем позицию агента с машиной
        agent.nextPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (player == null || caughtPlayer)
            return;

        if (collision.transform.root == player.root)
        {
            CatchPlayer();
        }
    }

    private void CatchPlayer()
    {
        if (caughtPlayer)
            return;

        caughtPlayer = true;

        motor.Move(0f, 0f, false);

        agent.ResetPath();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.FinishRace();
        }

        Debug.Log("ПОЛИЦИЯ ПОЙМАЛА ИГРОКА!");
    }

    public void SetTarget(Transform target)
    {
        player = target;

        if (agent != null && player != null && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
        }
    }
}

