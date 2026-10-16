
using UnityEngine;

public class VehicleInput : MonoBehaviour
{
    public float Steering { get; private set; }
    public float Throttle { get; private set; }
    public bool IsHandbraking { get; private set; }

    private void Update()
    {
        Steering = Input.GetAxis("Horizontal");
        Throttle = Input.GetAxis("Vertical");
        IsHandbraking = Input.GetKey(KeyCode.Space);
    }
}
