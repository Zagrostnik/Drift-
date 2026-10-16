
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 7f;
    [SerializeField] private float height = 3f;
    [SerializeField] private float positionSmoothness = 5f;
    [SerializeField] private float rotationSmoothness = 7f;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition =
            target.position
            - target.forward * distance
            + Vector3.up * height;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            positionSmoothness * Time.deltaTime);

        Quaternion desiredRotation = Quaternion.LookRotation(
            target.position + Vector3.up - transform.position);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationSmoothness * Time.deltaTime);
    }
}
