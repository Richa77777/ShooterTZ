using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class InventorySaveData
{
    public int InventorySize;
    public List<int> Guns;
    public List<ExtraItemSaveData> ExtraItems;
}

[Serializable]
public class ExtraItemSaveData
{
    public ItemType ItemType;
    public int Count;
}

public static class InventorySaveLoad
{
    private static string GetSavePath(string fileName = "inventory.json")
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    public static void SaveInventory(Inventory inventory, string fileName = "inventory.json")
    {
        InventorySaveData saveData = new InventorySaveData
        {
            InventorySize = inventory.InventorySize,
            Guns = new List<int>(inventory.Guns),
            ExtraItems = new List<ExtraItemSaveData>()
        };

        foreach (var item in inventory.GetAllExtraItems())
        {
            saveData.ExtraItems.Add(new ExtraItemSaveData
            {
                ItemType = item.ItemType,
                Count = item.Count
            });
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(GetSavePath(fileName), json);
    }

    public static void LoadInventory(Inventory inventory, string fileName = "inventory.json")
    {
        string path = GetSavePath(fileName);

        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);

        inventory.Clear();

        foreach (var gunId in saveData.Guns)
        {
            inventory.AddGun(gunId);
        }

        foreach (var itemData in saveData.ExtraItems)
        {
            inventory.AddExtraItem(itemData.ItemType, itemData.Count);
        }
    }
}