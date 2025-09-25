using TMPro;
using UnityEngine;

namespace UIElements
{
    public class ExtraItemSlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private string _itemTypeString;

        public ItemType ItemType => System.Enum.Parse<ItemType>(_itemTypeString);

        public void UpdateCountText(string countText)
        {
            _countText.text = countText;
        }
    }
}