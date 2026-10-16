
using UnityEngine;

public class WallCollisionHandler : MonoBehaviour, ICollisionHandler
{
    [SerializeField] private float collisionCooldown = 1f;

    private ScoreManager scoreManager;
    private float nextAllowedCollisionTime;

    private void Awake()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (Time.time < nextAllowedCollisionTime)
            return;

        HandleCollision();

        nextAllowedCollisionTime =
            Time.time + collisionCooldown;
    }

    public void HandleCollision()
    {
        if (scoreManager == null)
            return;

        scoreManager.ApplyCollisionPenalty();
    }
}
