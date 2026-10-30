
using UnityEngine;

[RequireComponent(typeof(VehicleMotor))]
public class PoliceAI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("AI Settings")]
    [SerializeField] private float steeringAngle = 45f;
    [SerializeField] private float catchDistance = 3.5f;

    private VehicleMotor motor;
    private bool caughtPlayer;

    private void Awake()
    {
        motor = GetComponent<VehicleMotor>();
    }

    private void FixedUpdate()
    {
        if (player == null || caughtPlayer)
            return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        // Запасной способ обнаружения по расстоянию
        if (distance <= catchDistance)
        {
            CatchPlayer();
            return;
        }

        Vector3 localTarget =
            transform.InverseTransformPoint(player.position);

        float angle = Mathf.Atan2(
            localTarget.x,
            localTarget.z
        ) * Mathf.Rad2Deg;

        float steering = Mathf.Clamp(
            angle / steeringAngle,
            -1f,
            1f
        );

        motor.Move(1f, steering, false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (player == null || caughtPlayer)
            return;

        // Проверяем, что полиция столкнулась именно с игроком
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

        if (GameManager.Instance != null)
        {
            GameManager.Instance.FinishRace();
        }

        Debug.Log("ПОЛИЦИЯ ПОЙМАЛА ИГРОКА!");
    }

    public void SetTarget(Transform target)
    {
        player = target;
    }
}
