using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<int> _guns = new List<int>();
    private List<ExtraItem> _extraItems = new List<ExtraItem>();

    public int InventorySize { get; private set; } = 3;
    public IReadOnlyList<int> Guns => _guns;


    public Inventory(int inventorySize = 3)
    {
        InventorySize = Mathf.Max(0, inventorySize);
    }

    #region Gun Methods

    public void AddGun(int? gunId)
    {
        if (!_guns.Contains(gunId.Value) && _guns.Count < InventorySize)
        {
            _guns.Add(gunId.Value);
            EventsHandler.InvokeOnInventoryChanged();
        }
    }

    public void RemoveGun(int gunId)
    {
        if (_guns.Remove(gunId))
        {
            EventsHandler.InvokeOnInventoryChanged();
        }
    }

    public int? GetGunBySlotIndex(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _guns.Count) return null;

        return _guns[slotIndex];
    }

    public int? GetGunSlotIndexById(int gunId)
    {
        for (int i = 0; i < _guns.Count; i++)
        {
            if (_guns[i] == gunId)
            {
                return i;
            }
        }

        return null;
    }

    #endregion

    #region Extra Item Methods

    public void AddExtraItem(ItemType itemType, int count)
    {
        int? extraItemIndex = GetExtraItemIndex(itemType);

        if (extraItemIndex == null) _extraItems.Add(new ExtraItem(itemType, Mathf.Clamp(count, 0, int.MaxValue)));
        else _extraItems[extraItemIndex.Value].AddCount(count);

        if (count > 0)
            EventsHandler.InvokeOnInventoryChanged();
    }

    public bool TryRemoveExtraItem(ItemType itemType, int count)
    {
        int? extraItemIndex = GetExtraItemIndex(itemType);

        if (extraItemIndex != null)
        {
            if (_extraItems[extraItemIndex.Value].SubtractCount(Mathf.Abs(count)))
            {
                EventsHandler.InvokeOnInventoryChanged();
                return true;
            }
        }

        return false;
    }

    public int? GetExtraItemIndex(ItemType itemType)
    {
        for (int i = 0; i < _extraItems.Count; i++)
        {
            if (_extraItems[i].ItemType == itemType)
            {
                return i;
            }
        }

        return null;
    }

    public int? GetExtraItemCount(ItemType itemType)
    {
        for (int i = 0; i < _extraItems.Count; i++)
        {
            if (_extraItems[i].ItemType == itemType)
            {
                return _extraItems[i].Count;
            }
        }

        return null;
    }

    #endregion
}