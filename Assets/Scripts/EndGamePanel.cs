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
        EventsHandler.OnAllEnemiesKilled += Win;
        EventsHandler.OnPlayerDied += Lose;
    }

    private void OnDisable()
    {
        EventsHandler.OnAllEnemiesKilled -= Win;
        EventsHandler.OnPlayerDied -= Lose;
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