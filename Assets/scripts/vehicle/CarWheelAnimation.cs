
using UnityEngine;

public class CarWheelAnimation : MonoBehaviour
{
    [Header("Wheel References")]
    [SerializeField] private Transform wheelFL;
    [SerializeField] private Transform wheelFR;
    [SerializeField] private Transform wheelRL;
    [SerializeField] private Transform wheelRR;

    [Header("Wheel Settings")]
    [SerializeField] private float wheelRadius = 0.35f;
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float steeringSmoothTime = 0.12f;

    private Quaternion originalFL;
    private Quaternion originalFR;
    private Quaternion originalRL;
    private Quaternion originalRR;

    private Vector3 previousPosition;

    private float currentSteerAngle;
    private float steerVelocity;
    private float wheelRotation;

    private void Start()
    {
        originalFL = wheelFL.localRotation;
        originalFR = wheelFR.localRotation;
        originalRL = wheelRL.localRotation;
        originalRR = wheelRR.localRotation;

        previousPosition = transform.position;
    }

    private void LateUpdate()
    {
        AnimateWheels();
    }

    private void AnimateWheels()
    {
        float deltaTime = Mathf.Max(Time.deltaTime, 0.0001f);

        // 1. Получаем реальное направление движения
        Vector3 movement = (transform.position - previousPosition) / deltaTime;

        float signedSpeed = transform.InverseTransformDirection(movement).z;

        // 2. Вращаем колёса с учётом движения вперёд и назад
        wheelRotation +=
            signedSpeed / wheelRadius * Mathf.Rad2Deg * deltaTime;

        // 3. Получаем непосредственный ввод руления
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        float targetSteerAngle = horizontalInput * maxSteerAngle;

        // 4. Плавно поворачиваем передние колёса
        currentSteerAngle = Mathf.SmoothDamp(
            currentSteerAngle,
            targetSteerAngle,
            ref steerVelocity,
            steeringSmoothTime
        );

        // 5. Передние колёса: поворот и вращение
        wheelFL.localRotation =
            originalFL *
            Quaternion.Euler(0f, currentSteerAngle, 0f) *
            Quaternion.Euler(wheelRotation, 0f, 0f);

        wheelFR.localRotation =
            originalFR *
            Quaternion.Euler(0f, currentSteerAngle, 0f) *
            Quaternion.Euler(wheelRotation, 0f, 0f);

        // 6. Задние колёса: только вращение
        wheelRL.localRotation =
            originalRL *
            Quaternion.Euler(wheelRotation, 0f, 0f);

        wheelRR.localRotation =
            originalRR *
            Quaternion.Euler(wheelRotation, 0f, 0f);

        previousPosition = transform.position;
    }
}
