using UnityEngine;

public class ExtraItem
{
    public ItemType ItemType { get; private set; }
    public int Count { get; private set; }

    public ExtraItem(ItemType type, int count)
    {
        ItemType = type;
        Count = count;
    }

    public void AddCount(int value)
    {
        Count = Mathf.Clamp(Count + value, 0, int.MaxValue);
    }

    public bool SubtractCount(int value)
    {
        if (Count <= 0) return false;
        
        Count = Mathf.Clamp(Count - value, 0, int.MaxValue);
        return true;
    }
}

public enum ItemType
{
    Medkit,
    AmmoPack
}