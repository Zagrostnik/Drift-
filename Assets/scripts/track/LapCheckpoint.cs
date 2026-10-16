
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class LapCheckpoint : MonoBehaviour
{
    [SerializeField] private int checkpointIndex;

    public int CheckpointIndex => checkpointIndex;

    private void Awake()
    {
        BoxCollider trigger = GetComponent<BoxCollider>();
        trigger.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        LapManager lapManager =
            FindFirstObjectByType<LapManager>();

        if (lapManager != null)
        {
            lapManager.RegisterCheckpoint(checkpointIndex);
        }
    }
}
