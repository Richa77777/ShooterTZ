using UIElements;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryView _inventoryView;

    private GunsDatabase _gunsDatabase;
    private Inventory _inventory;

    private int _currentSelectedSlot = 0;

    private void Awake()
    {
        _gunsDatabase = Resources.Load<GunsDatabase>("Databases/GunsDatabase");

        _inventory = new Inventory();
        LoadInventory();
    }

    private void OnEnable()
    {
        EventsHandler.OnGunPickedUp += OnGunPickedUp;
        EventsHandler.OnItemPickedUp += OnItemPickedUp;
        EventsHandler.OnExtraItemUsed += OnExtraItemUsed;
        EventsHandler.OnInventoryChanged += SaveInventory;
    }

    private void OnDisable()
    {
        EventsHandler.OnGunPickedUp -= OnGunPickedUp;
        EventsHandler.OnItemPickedUp -= OnItemPickedUp;
        EventsHandler.OnExtraItemUsed -= OnExtraItemUsed;
        EventsHandler.OnInventoryChanged -= SaveInventory;
    }

    private void Update()
    {
        for (int i = 1; i <= _inventory.InventorySize; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                SelectGun(i - 1);
            }
        }

        HandleItemUse(KeyCode.Q, ItemType.Medkit);
        HandleItemUse(KeyCode.F, ItemType.AmmoPack);
    }

    private void HandleItemUse(KeyCode key, ItemType itemType)
    {
        if (Input.GetKeyDown(key))
        {
            if (TryRemoveExtraItem(itemType))
                EventsHandler.InvokeOnExtraItemUsed(itemType);
        }
    }

    #region Save/Load Methods

    private void SaveInventory()
    {
        InventorySaveLoad.SaveInventory(_inventory);
    }

    private void LoadInventory()
    {
        InventorySaveLoad.LoadInventory(_inventory);

        DisplayAllGuns();
        DisplayAllExtraItems();
    }

    #endregion

    #region Gun Methods

    private void AddGun(int? gunId)
    {
        _inventory.AddGun(gunId);
    }

    private void RemoveGun(int gunId)
    {
        _inventory.RemoveGun(gunId);
    }

    private void SelectGun(int slotIndex)
    {
        if (slotIndex >= _inventory.InventorySize) return;

        _currentSelectedSlot = slotIndex;

        int? gunIndex = _inventory.GetGunBySlotIndex(slotIndex);
        if (gunIndex != null)
            EventsHandler.InvokeOnGunSelected(gunIndex.Value);
    }

    #endregion

    #region Extra Item Methods

    private void AddExtraItem(ItemType itemType, int count = 1)
    {
        _inventory.AddExtraItem(itemType, count);
    }

    public bool TryRemoveExtraItem(ItemType itemType, int count = 1)
    {
        return _inventory.TryRemoveExtraItem(itemType, count);

    }

    #endregion

    #region UI Methods

    #region Events
    private void OnGunPickedUp(int? gunID)
    {
        AddGun(gunID);
        UpdateGunSlotUI(gunID.Value);
    }

    private void OnItemPickedUp(ItemType itemType, int value)
    {
        AddExtraItem(itemType, value);
        UpdateExtraItemUI(itemType);
    }

    private void OnExtraItemUsed(ItemType itemType)
    {
        UpdateExtraItemUI(itemType);
    }

    #endregion

    private void UpdateGunSlotUI(int gunID)
    {
        int? slotIndex = _inventory.GetGunSlotIndexById(gunID);
        if (!slotIndex.HasValue) return;

        var gunEntry = _gunsDatabase.GetGunEntryById(gunID);
        _inventoryView.SetGunSlotUI(slotIndex.Value, gunEntry.Icon);
    }

    private void UpdateExtraItemUI(ItemType itemType)
    {
        int? count = _inventory.GetExtraItemCount(itemType);
        _inventoryView.SetExtraItemSlotUI(itemType, count?.ToString() ?? "0");
    }

    private void DisplayAllGuns()
    {
        foreach (int gunID in _inventory.Guns)
            UpdateGunSlotUI(gunID);
    }

    private void DisplayAllExtraItems()
    {
        foreach (var extraItem in _inventory.ExtraItems)
            UpdateExtraItemUI(extraItem.ItemType);
    }

    #endregion
}