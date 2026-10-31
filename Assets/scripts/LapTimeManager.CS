using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Для TextMeshPro using UnityEngine.SceneManagement;
public class LapTimeManager : MonoBehaviour { [Header("Настройки времени")] [Tooltip("Время в секундах, дающееся на прохождение одного круга/участка")] public float timePerLap = 30f; private float currentTimeLeft; private bool isRacing = false;
[Header("Интерфейс (UI)")]
public TMP_Text timerText;          // Текст для отображения таймера
public GameObject gameOverPanel;    // Панель поражения (опционально)

[Header("Ссылки")]
public MonoBehaviour carController; // Скрипт управления машиной (чтобы отключить при проигрыше)

void Start()
{
    StartNewAttempt();
}

void Update()
{
    if (!isRacing) return;

    // Уменьшаем таймер каждый кадр
    currentTimeLeft -= Time.deltaTime;

    // Обновляем текст на экране
    UpdateTimerUI();

    // Проверяем, истекло ли время
    if (currentTimeLeft <= 0f)
    {
        currentTimeLeft = 0f;
        GameOver();
    }
}

// Метод для запуска/перезапуска гонки
public void StartNewAttempt()
{
    currentTimeLeft = timePerLap;
    isRacing = true;

    if (gameOverPanel != null) 
        gameOverPanel.SetActive(false);

    if (carController != null) 
        carController.enabled = true;
}

// ЭТОТ МЕТОД НУЖНО ВЫЗВАТЬ, КОГДА МАШИНА ПЕРЕСЕКАЕТ ЛИНИЮ ФИНИША (ЧЕКПОИНТ)
public void PassFinishLine()
{
    if (!isRacing) return;

    Debug.Log("Круг пройден вовремя! Время сброшено.");

    // Сбрасываем таймер заново для следующего круга
    currentTimeLeft = timePerLap; 
    
    // Здесь можно добавить подсчет очков или номер текущего круга, если нужно
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

void GameOver()
{
    isRacing = false;
    Debug.Log("Время вышло! Вы проиграли.");

    // Показываем панель поражения
    if (gameOverPanel != null) 
        gameOverPanel.SetActive(true);

    // Отключаем управление машиной
    if (carController != null) 
        carController.enabled = false;
}

// Метод для кнопки "Попробовать снова" в UI
public void RestartGame()
{
    // Перезагружает текущую сцену (самый простой способ рестарта)
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
}
