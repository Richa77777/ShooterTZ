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
        _inventory = new Inventory();
        _gunsDatabase = Resources.Load<GunsDatabase>("Databases/GunsDatabase");
    }

    private void OnEnable()
    {
        EventsHandler.OnGunPickedUp += OnGunPickedUp;
        EventsHandler.OnItemPickedUp += OnItemPickedUp;
        EventsHandler.OnExtraItemUsed += OnExtraItemUsed;
    }

    private void OnDisable()
    {
        EventsHandler.OnGunPickedUp -= OnGunPickedUp;
        EventsHandler.OnItemPickedUp -= OnItemPickedUp;
        EventsHandler.OnExtraItemUsed -= OnExtraItemUsed;
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

    private void OnGunPickedUp(int? gunID)
    {
        AddGun(gunID);
        _inventoryView.SetGunSlotUI(_inventory.GetGunSlotIndexById(gunID.Value).Value, _gunsDatabase.GetGunEntryById(gunID.Value).Icon);
    }

    private void OnItemPickedUp(ItemType itemType, int value)
    {
        AddExtraItem(itemType, value);
        _inventoryView.SetExtraItemSlotUI(itemType, _inventory.GetExtraItemCount(itemType).Value.ToString());
    }

    private void OnExtraItemUsed(ItemType itemType)
    {
        _inventoryView.SetExtraItemSlotUI(itemType, _inventory.GetExtraItemCount(itemType).Value.ToString());
    }

    #endregion
}