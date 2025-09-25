using System.Collections.Generic;
using UnityEngine;

namespace UIElements
{

    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private List<GunSlot> _gunSlots = new List<GunSlot>();
        [SerializeField] private List<ExtraItemSlot> _extraItemSlots = new List<ExtraItemSlot>();

        public void SetGunSlotUI(int slotIndex, Sprite gunSprite)
        {
            _gunSlots[slotIndex].SetGunUI(gunSprite);
        }

        public void SetExtraItemSlotUI(ItemType itemType, string counterText)
        {
            for (int i = 0; i < _extraItemSlots.Count; i++)
            {
                if (_extraItemSlots[i].ItemType == itemType)
                    _extraItemSlots[i].UpdateCountText(counterText);
            }
        }
    }
}