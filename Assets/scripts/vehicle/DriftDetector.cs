
using UnityEngine;

public class DriftDetector : MonoBehaviour
{
    [SerializeField] private float minimumSpeed = 5f;
    [SerializeField] private float minimumDriftAngle = 12f;

    private VehicleMotor motor;

    public bool IsDrifting { get; private set; }
    public float DriftAngle { get; private set; }

    private void Awake()
    {
        motor = GetComponent<VehicleMotor>();
    }

    private void FixedUpdate()
    {
        Vector3 velocity = motor.Velocity;
        velocity.y = 0f;

        if (velocity.magnitude < minimumSpeed)
        {
            DriftAngle = 0f;
            IsDrifting = false;
            return;
        }

        DriftAngle = Vector3.Angle(
            transform.forward, velocity.normalized);

        IsDrifting = DriftAngle >= minimumDriftAngle
            && DriftAngle <= 75f;
    }
}
