using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(InventoryPersistence))]
public class Inventory : MonoBehaviour
{
    [SerializeField] private GunSlot[] _gunSlots = new GunSlot[3];
    [SerializeField] private ExtraItemSlot[] _extraItemSlots = new ExtraItemSlot[2];

    private int _currentSelectedSlot = 1;
    private GunsDatabase _gunsDatabase;

    public event Action<int /* Weapon ID */> OnGunSelected;
    public event Action<ItemType> OnExtraItemUsed;

    public event Action OnGunAdded;
    public event Action OnGunRemoved;
    public event Action OnExtraItemAdded;
    public event Action OnExtraItemRemoved;

    public GunSlot[] GunSlots => _gunSlots;
    public ExtraItemSlot[] ExtraItemSlots => _extraItemSlots;
    
    public void Initialize(GunsDatabase gunsDatabase)
    {
        _gunsDatabase = gunsDatabase;

        foreach (GunSlot gunSlot in _gunSlots)
        {
            gunSlot.Initialize(_gunsDatabase);
        }
        
        GetComponent<InventoryPersistence>().LoadInventory();
        SelectGunSlot(1);
    }

    private void OnEnable()
    {
        if (EventsHandler.Instance == null) return;

        EventsHandler.Instance.OnGunPickedUp += AddGun;
        EventsHandler.Instance.OnItemPickedUp += AddExtraItem;
    }

    private void OnDisable()
    {
        if (EventsHandler.Instance == null) return;
        
        EventsHandler.Instance.OnGunPickedUp -= AddGun;
        EventsHandler.Instance.OnItemPickedUp -= AddExtraItem;
    }

    private void Update()
    {
        for (int i = 1; i <= _gunSlots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                SelectGunSlot(i);
            }
        }

        HandleItemUse(KeyCode.Q, ItemType.Medkit);
        HandleItemUse(KeyCode.F, ItemType.AmmoPack);
    }

    private void HandleItemUse(KeyCode key, ItemType itemType)
    {
        if (Input.GetKeyDown(key) && CheckExtraItemAvailable(itemType))
        {
            RemoveExtraItem(itemType);
            OnExtraItemUsed?.Invoke(itemType);
        }
    }
    
    #region Gun
    
    public void AddGun(int gunId)
    {
        if (_gunSlots.Any(gunSlot => gunSlot.CurrentGunId == gunId))
        {
            return;
        }
        
        foreach (GunSlot gunSlot in _gunSlots)
        {
            if (gunSlot.CurrentGunId == -1)
            {
                gunSlot.SetGun(gunId);
                break;
            }
        }
        
        SelectGunSlot(_currentSelectedSlot);
        OnGunAdded?.Invoke();
    }

    public void RemoveGun(int slotIndex)
    {
        _gunSlots[slotIndex].RemoveGun();
        SelectGunSlot(_currentSelectedSlot);
        OnGunRemoved?.Invoke();
    }

    private void SelectGunSlot(int slotIndex)
    {
        if (slotIndex > _gunSlots.Length || slotIndex < 1) return;
        _currentSelectedSlot = slotIndex;
        
        OnGunSelected?.Invoke(_gunSlots[slotIndex - 1].CurrentGunId);
    }
    
    #endregion

    #region Extra
    private void AddExtraItem(ItemType itemType)
    {
        _extraItemSlots.First(e => e.ItemType == itemType)?.AddItem(1);
        OnGunAdded?.Invoke();
    }
    
    public void RemoveExtraItem(ItemType itemType)
    {
        _extraItemSlots.First(e => e.ItemType == itemType)?.RemoveItem(1);
        OnExtraItemRemoved?.Invoke();
    }
    
    public bool CheckExtraItemAvailable(ItemType itemType)
    {
        return _extraItemSlots.First(e => e.ItemType == itemType).ItemCount > 0;
    }
    
    #endregion
}