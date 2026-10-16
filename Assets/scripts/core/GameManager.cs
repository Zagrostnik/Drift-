
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState State { get; private set; } =
        GameState.MainMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("GameManager initialized");
    }
    //private void Start()
    //{
    //    StartRace();
    //}
    public void StartRace()
    {
        SetState(GameState.Playing);
        Time.timeScale = 1f;
    }

    public void PauseRace()
    {
        if (State != GameState.Playing)
            return;

        SetState(GameState.Paused);
        Time.timeScale = 0f;
    }

    public void ResumeRace()
    {
        if (State != GameState.Paused)
            return;

        Time.timeScale = 1f;
        SetState(GameState.Playing);
    }

    public void FinishRace()
    {
        if (State != GameState.Playing)
            return;

        Time.timeScale = 1f;
        SetState(GameState.Finished);
    }

    private void SetState(GameState newState)
    {
        State = newState;
        Debug.Log("Game state: " + State);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            Time.timeScale = 1f;
        }
    }
}
