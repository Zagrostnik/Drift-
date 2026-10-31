using UnityEngine; using TMPro; using UnityEngine.SceneManagement; using System.Collections; // Нужно для корутин (плавной анимации)
public class LapTimeManager : MonoBehaviour { [Header("Настройки времени")] public float timePerLap = 30f; private float currentTimeLeft; private bool isRacing = false;
[Header("Очки и Текст")]
public int score = 0;                  
public TMP_Text scoreText;             
public TMP_Text popupText;             // Ссылка на всплывающий текст (+5)

[Header("Интерфейс (UI)")]
public TMP_Text timerText;          
public GameObject gameOverPanel;    

[Header("Ссылки")]
public MonoBehaviour carController; 

void Start()
{
    if (popupText != null) popupText.gameObject.SetActive(false);
    StartNewAttempt();
}

void Update()
{
    if (!isRacing) return;

    currentTimeLeft -= Time.deltaTime;
    UpdateTimerUI();

    if (currentTimeLeft <= 0f)
    {
        currentTimeLeft = 0f;
        GameOver();
    }
}

public void StartNewAttempt()
{
    currentTimeLeft = timePerLap;
    score = 0; 
    isRacing = true;

    if (gameOverPanel != null) 
        gameOverPanel.SetActive(false);

    if (carController != null) 
        carController.enabled = true;
        
    UpdateScoreUI();
}

public void PassFinishLine()
{
    if (!isRacing) return;

    currentTimeLeft = timePerLap; 
    AddScore(100, "КРУГ ПРОЙДЕН! +100"); 
}

// Метод для начисления очков с выводом всплывающего текста
public void AddScore(int amount, string customMessage = "")
{
    if (!isRacing) return;
    score += amount;
    UpdateScoreUI();

    // Показываем всплывающий текст
    if (popupText != null)
    {
        StopAllCoroutines(); // Останавливаем прошлую анимацию, если она еще идет
        StartCoroutine(ShowPopupAnimation(customMessage != "" ? customMessage : "+" + amount));
    }
}

IEnumerator ShowPopupAnimation(string message)
{
    popupText.text = message;
    popupText.gameObject.SetActive(true);
    
    Color textColor = popupText.color;
    textColor.a = 1f; // Делаем текст полностью видимым
    popupText.color = textColor;

    float duration = 0.8f; // Время, сколько текст виден/анимируется
    float elapsed = 0f;

    Vector3 initialScale = Vector3.one * 0.5f;
    Vector3 targetScale = Vector3.one * 1.2f;

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = elapsed / duration;

        // Плавное увеличение и увядание
        popupText.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
        textColor.a = Mathf.Lerp(1f, 0f, t);
        popupText.color = textColor;

        yield return null;
    }

    popupText.gameObject.SetActive(false); // Скрываем текст после анимации
}

void UpdateTimerUI()
{
    if (timerText != null)
    {
        int seconds = Mathf.FloorToInt(currentTimeLeft);
        int milliseconds = Mathf.FloorToInt((currentTimeLeft * 100) % 100);
        timerText.text = string.Format("Время: {0:00}:{1:00}", seconds, milliseconds);
    }
}

void UpdateScoreUI()
{
    if (scoreText != null)
    {
        scoreText.text = "Очки: " + score;
    }
}

void GameOver()
{
    isRacing = false;
    if (popupText != null) popupText.gameObject.SetActive(false);
    if (gameOverPanel != null) gameOverPanel.SetActive(true);
    if (carController != null) carController.enabled = false;
}

public void RestartGame()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
}
