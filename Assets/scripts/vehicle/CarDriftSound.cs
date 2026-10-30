
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(VehicleInput))]
public class CarDriftSound : MonoBehaviour
{
    [Header("Drift Sound")]
    [SerializeField] private AudioClip driftClip;

    [Header("Settings")]
    [SerializeField] private float maxVolume = 0.7f;
    [SerializeField] private float fadeSpeed = 4f;

    private AudioSource audioSource;
    private VehicleInput vehicleInput;


    private void Awake()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        audioSource = sources[sources.Length - 1];
        vehicleInput = GetComponent<VehicleInput>();

        audioSource.clip = driftClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;
        audioSource.spatialBlend = 1f;
    }


    private void Update()
    {
        if (driftClip == null)
            return;

        float targetVolume = vehicleInput.IsHandbraking
            ? maxVolume
            : 0f;

        audioSource.volume = Mathf.MoveTowards(
            audioSource.volume,
            targetVolume,
            fadeSpeed * Time.deltaTime
        );

        if (audioSource.volume > 0.01f && !audioSource.isPlaying)
            audioSource.Play();

        if (audioSource.volume <= 0.01f && audioSource.isPlaying)
            audioSource.Stop();
    }
}
