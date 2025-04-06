using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryPersistence : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GunsDatabase _gunsDatabase;

    private const string GameExitFlag = "GameExitFlag";

    private void Awake()
    {
        if (!WasGameExitedCorrectly())
        {
            ClearSavedData(); // Only Between Scenes
            SetGameExitFlag(false);
        }
    }

    private void OnApplicationQuit()
    {
        ClearSavedData();
        SetGameExitFlag(true);
    }

    private void OnEnable()
    {
        _inventory.OnGunAdded += SaveInventory;
        _inventory.OnGunRemoved += SaveInventory;
        _inventory.OnExtraItemAdded += SaveInventory;
        _inventory.OnGunRemoved += SaveInventory;
    }

    private void OnDisable()
    {
        _inventory.OnGunAdded -= SaveInventory;
        _inventory.OnGunRemoved -= SaveInventory;
        _inventory.OnExtraItemAdded -= SaveInventory;
        _inventory.OnGunRemoved -= SaveInventory;
    }

    private void SaveInventory()
    {
        for (int i = 0; i < _inventory.GunSlots.Length; i++)
        {
            int gunId = _inventory.GunSlots[i].CurrentGunId;
            PlayerPrefs.SetInt($"GunSlot_{i}", gunId);
        }

        for (int i = 0; i < _inventory.ExtraItemSlots.Length; i++)
        {
            int itemCount = _inventory.ExtraItemSlots[i].ItemCount;
            PlayerPrefs.SetInt($"ItemSlot_{i}", itemCount); 
        }

        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        for (int i = 0; i < _inventory.GunSlots.Length; i++)
        {
            int gunId = PlayerPrefs.GetInt($"GunSlot_{i}", -1);
            if (gunId != -1)
            {
                _inventory.GunSlots[i].SetGun(gunId);
            }
        }

        for (int i = 0; i < _inventory.ExtraItemSlots.Length; i++)
        {
            int itemCount = PlayerPrefs.GetInt($"ItemSlot_{i}", 0);
            if (itemCount > 0)
            {
                _inventory.ExtraItemSlots[i].AddItem(itemCount);
            }
        }
    }

    private void ClearSavedData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    private bool WasGameExitedCorrectly()
    {
        return PlayerPrefs.GetInt(GameExitFlag, 1) == 1;
    }

    private void SetGameExitFlag(bool isCorrectExit)
    {
        PlayerPrefs.SetInt(GameExitFlag, isCorrectExit ? 1 : 0);
        PlayerPrefs.Save();
    }
}