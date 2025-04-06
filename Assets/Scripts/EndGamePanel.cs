using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _endText;
    [SerializeField] private GameObject _endPanel;
    
    private void OnEnable()
    {
        if (EventsHandler.Instance == null) return;
        EventsHandler.Instance.OnAllEnemiesKilled += Win;
        EventsHandler.Instance.OnPlayerDied += Lose;
    }

    private void OnDisable()
    {
        if (EventsHandler.Instance == null) return;

        EventsHandler.Instance.OnAllEnemiesKilled -= Win;
        EventsHandler.Instance.OnPlayerDied -= Lose;
    }

    private void Win()
    {
        EndGame(GameEndStatus.Win);
    }
    
    private void Lose()
    {
        EndGame(GameEndStatus.Lose);
    }
    
    private void EndGame(GameEndStatus status)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        _endPanel.SetActive(true);
        
        if (status == GameEndStatus.Win)
        {
            _endText.text = "You Win!";
        }
        else if (status == GameEndStatus.Lose)
        {
            _endText.text = "You Lose!";
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
        
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
}

public enum GameEndStatus
{
    Lose,
    Win
}