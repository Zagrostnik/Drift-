
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicleMotor : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float acceleration = 9000f;
    [SerializeField] private float reverseAcceleration = 4000f;
    [SerializeField] private float maxSpeed = 25f;

    [Header("Steering")]
    [SerializeField] private float steeringPower = 85f;
    [SerializeField] private float minimumSteeringSpeed = 1.5f;

    [Header("Drift")]
    [SerializeField] private float normalGrip = 5f;
    [SerializeField] private float driftGrip = 1.2f;

    private Rigidbody rb;

    public float ForwardSpeed =>
        Vector3.Dot(rb.linearVelocity, transform.forward);

    public Vector3 Velocity => rb.linearVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(float throttle, float steering,
        bool handbraking)
    {
        float speed = rb.linearVelocity.magnitude;
        float forwardSpeed = ForwardSpeed;

        // Ускорение вперёд.
        if (throttle > 0f && forwardSpeed < maxSpeed)
        {
            rb.AddForce(
                transform.forward * throttle * acceleration,
                ForceMode.Force);
        }
        // Движение назад.
        else if (throttle < 0f && forwardSpeed > -maxSpeed * 0.4f)
        {
            rb.AddForce(
                transform.forward * throttle * reverseAcceleration,
                ForceMode.Force);
        }

        // Поворот зависит от скорости движения.
        // На месте машина не поворачивает.
        if (speed > minimumSteeringSpeed)
        {
            float speedFactor = Mathf.Clamp01(speed / 8f);

            // При движении назад направление поворота меняется.
            float direction = forwardSpeed >= 0f ? 1f : -1f;

            float turnAmount =
                steering * steeringPower * speedFactor
                * direction * Time.fixedDeltaTime;

            rb.MoveRotation(
                rb.rotation *
                Quaternion.Euler(0f, turnAmount, 0f));
        }

        ApplyLateralGrip(handbraking);
    }

    private void ApplyLateralGrip(bool handbraking)
    {
        Vector3 lateralVelocity =
            Vector3.Project(rb.linearVelocity, transform.right);

        float grip = handbraking ? driftGrip : normalGrip;

        rb.AddForce(
            -lateralVelocity * grip,
            ForceMode.Acceleration);
    }

    public void ResetVehicle(Vector3 position, Quaternion rotation)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.position = position;
        rb.rotation = rotation;

        transform.SetPositionAndRotation(position, rotation);
    }

}
