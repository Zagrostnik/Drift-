
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(VehicleMotor))]
public class VehicleEngineSound : MonoBehaviour
{
    [Header("Engine Sound")]
    [SerializeField] private AudioClip engineClip;

    [Header("Sound Settings")]
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 2f;
    [SerializeField] private float minVolume = 0.2f;
    [SerializeField] private float maxVolume = 0.8f;
    [SerializeField] private float smoothSpeed = 5f;

    private AudioSource audioSource;
    private VehicleMotor motor;
    private VehicleInput vehicleInput;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        motor = GetComponent<VehicleMotor>();
        vehicleInput = GetComponent<VehicleInput>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.clip = engineClip;
    }

    private void Start()
    {
        if (engineClip != null)
        {
            audioSource.Play();
        }
    }

    private void Update()
    {
        if (engineClip == null)
            return;

        float speed = Mathf.Abs(motor.ForwardSpeed);

        float speedFactor = Mathf.Clamp01(speed / 25f);

        float throttle = vehicleInput != null
            ? Mathf.Abs(vehicleInput.Throttle)
            : 0f;

        float targetPitch = Mathf.Lerp(
            minPitch,
            maxPitch,
            Mathf.Clamp01(speedFactor * 0.7f + throttle * 0.3f)
        );

        float targetVolume = Mathf.Lerp(
            minVolume,
            maxVolume,
            Mathf.Clamp01(speedFactor + throttle * 0.3f)
        );

        audioSource.pitch = Mathf.Lerp(
            audioSource.pitch,
            targetPitch,
            Time.deltaTime * smoothSpeed
        );

        audioSource.volume = Mathf.Lerp(
            audioSource.volume,
            targetVolume,
            Time.deltaTime * smoothSpeed
        );
    }
}
