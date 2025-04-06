using UnityEngine;

public class EnemiesCounter : MonoBehaviour
{
    private int _enemyCount = 0;

    private void Awake()
    {
        GetAllEnemiesFromScene();
    }

    private void OnEnable()
    {
        EventsHandler.Instance.OnEnemyKilled += SubtractEnemyCount;
    }

    private void OnDisable()
    {
        EventsHandler.Instance.OnEnemyKilled -= SubtractEnemyCount;
    }

    private void GetAllEnemiesFromScene()
    {
        _enemyCount = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
    }

    private void SubtractEnemyCount()
    {
        _enemyCount--;

        if (_enemyCount <= 0)
        {
            EventsHandler.Instance.OnAllEnemiesKilled?.Invoke();
        }
    }
}