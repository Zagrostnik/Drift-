
using UnityEngine;

[RequireComponent(typeof(VehicleInput))]
[RequireComponent(typeof(VehicleMotor))]
[RequireComponent(typeof(DriftDetector))]
public class VehicleController : MonoBehaviour
{
    private VehicleInput vehicleInput;
    private VehicleMotor vehicleMotor;

    private void Awake()
    {
        vehicleInput = GetComponent<VehicleInput>();
        vehicleMotor = GetComponent<VehicleMotor>();
    }

    private void FixedUpdate()
    {
        vehicleMotor.Move(
            vehicleInput.Throttle,
            vehicleInput.Steering,
            vehicleInput.IsHandbraking);
    }
}
