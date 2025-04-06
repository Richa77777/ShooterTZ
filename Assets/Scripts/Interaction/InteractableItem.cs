using UnityEngine;

public class InteractableItem : InteractableObject
{
    [Header("Item Settings")]
    [SerializeField] private ItemType _itemType;

    public override void Interact(GameObject obj)
    {
        EventsHandler.Instance.OnItemPickedUp?.Invoke(_itemType);
        Debug.Log($"Pick up item of type {_itemType}");
        base.Interact(obj);
    }
}