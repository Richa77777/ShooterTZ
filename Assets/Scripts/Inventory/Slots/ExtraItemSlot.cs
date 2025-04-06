using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExtraItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;
    
    [field: SerializeField] public ItemType ItemType { get; private set; }
    public int ItemCount { get; private set; }

    private void Awake()
    {
        UpdateCountText();
    }

    public void AddItem(int count)
    {
        ItemCount += count;
        UpdateCountText();
    }
    
    public void RemoveItem(int count)
    {
        ItemCount -= count;
        UpdateCountText();
    }

    private void UpdateCountText()
    {
        _countText.text = ItemCount.ToString();
    }
}

public enum ItemType
{
    Medkit,
    AmmoPack
}
