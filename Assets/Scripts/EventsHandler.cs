using System;

public static class EventsHandler
{
    // Pickups
    public static event Action<ItemType, int> OnItemPickedUp;
    public static event Action<int?> OnGunPickedUp;

    // Kills
    public static event Action OnEnemyKilled;
    public static event Action OnAllEnemiesKilled;
    public static event Action OnPlayerDied;

    // Inventory
    public static event Action OnInventoryChanged;
    public static event Action<int> OnGunSelected;
    public static event Action<ItemType> OnExtraItemUsed;

    // Invoke Methods
    public static void InvokeOnItemPickedUp(ItemType itemType, int count) => OnItemPickedUp?.Invoke(itemType, count);
    public static void InvokeOnGunPickedUp(int gunPickedUp) => OnGunPickedUp?.Invoke(gunPickedUp);

    public static void InvokeOnEnemyKilled() => OnEnemyKilled?.Invoke();
    public static void InvokeOnAllEnemiesKilled() => OnAllEnemiesKilled?.Invoke();
    public static void InvokeOnPlayerDied() => OnPlayerDied?.Invoke();

    public static void InvokeOnInventoryChanged() => OnInventoryChanged?.Invoke();
    public static void InvokeOnGunSelected(int weaponId) => OnGunSelected?.Invoke(weaponId);
    public static void InvokeOnExtraItemUsed(ItemType itemType) => OnExtraItemUsed?.Invoke(itemType);
}