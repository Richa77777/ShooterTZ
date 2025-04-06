using System;
using UnityEngine;

public class EventsHandler : MonoBehaviour
{
    public static EventsHandler Instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (Instance == null)
        {
            GameObject obj = new GameObject("EventsHandler");
            Instance = obj.AddComponent<EventsHandler>();
            DontDestroyOnLoad(obj);
        }
    }

    public Action<ItemType> OnItemPickedUp;
    public Action<int> OnGunPickedUp;
    public Action OnEnemyKilled;
    public Action OnAllEnemiesKilled;
    public Action OnPlayerDied;
}