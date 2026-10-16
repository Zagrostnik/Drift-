
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject raceCanvas;
    [SerializeField] private GameObject finishCanvas;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private LapManager lapManager;
    [SerializeField] private VehicleMotor vehicleMotor;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private bool finishScreenShown;

    private void Start()
    {
        Time.timeScale = 1f;

        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(true);

        if (raceCanvas != null)
            raceCanvas.SetActive(false);

        if (finishCanvas != null)
            finishCanvas.SetActive(false);
    }
    private void Awake()
    {
        if (scoreManager == null)
            scoreManager = FindFirstObjectByType<ScoreManager>();

        if (lapManager == null)
            lapManager = FindFirstObjectByType<LapManager>();

        if (vehicleMotor == null)
            vehicleMotor = FindFirstObjectByType<VehicleMotor>();

        if (vehicleMotor != null)
        {
            spawnPosition = vehicleMotor.transform.position;
            spawnRotation = vehicleMotor.transform.rotation;
        }
    }

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.State == GameState.Finished
            && !finishScreenShown)
        {
            ShowFinishScreen();
        }
    }


    public void StartGame()
    {
        Time.timeScale = 1f;

        if (vehicleMotor == null)
        {
            Debug.LogError("VehicleMotor не найден!");
            return;
        }

        // Возвращаем машину на старт
        vehicleMotor.ResetVehicle(
            spawnPosition,
            spawnRotation
        );

        // Обнуляем очки
        if (scoreManager != null)
            scoreManager.ResetScore();

        // Сбрасываем пройденные круги
        if (lapManager != null)
            lapManager.ResetLaps();

        finishScreenShown = false;

        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(false);

        if (finishCanvas != null)
            finishCanvas.SetActive(false);

        if (raceCanvas != null)
            raceCanvas.SetActive(true);

        GameManager.Instance.StartRace();

        Debug.Log("Новый заезд начат!");
    }


    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Выход из игры работает в собранной версии.");
    }

    private void ShowFinishScreen()
    {
        Debug.Log("Показываем экран завершения гонки");

        finishScreenShown = true;

        if (raceCanvas != null)
            raceCanvas.SetActive(false);
        else
            Debug.LogError("Race Canvas не назначен!");

        if (finishCanvas != null)
            finishCanvas.SetActive(true);
        else
            Debug.LogError("Finish Canvas не назначен!");
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(true);

        if (raceCanvas != null)
            raceCanvas.SetActive(false);

        if (finishCanvas != null)
            finishCanvas.SetActive(false);

        finishScreenShown = false;

        Debug.Log("Для нового заезда потребуется сбросить прогресс.");
    }
}
