using UnityEngine;

public class InteractableItem : InteractableObject
{
    [Header("Item Settings")]
    [SerializeField] private ItemType _itemType;
    [SerializeField] private int _count = 1;

    public override void Interact(GameObject obj)
    {
        EventsHandler.InvokeOnItemPickedUp(_itemType, _count);
        Debug.Log($"Pick up item of type {_itemType}");
        base.Interact(obj);
    }
}