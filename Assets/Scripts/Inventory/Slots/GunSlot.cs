using UnityEngine;
using UnityEngine.UI;

namespace UIElements
{
    public class GunSlot : MonoBehaviour
    {
        [SerializeField] private Image _mainImage;

        public void SetGunUI(Sprite gunSprite)
        {
            if (_mainImage.sprite == gunSprite) return;

            _mainImage.sprite = gunSprite;
        }
    }
}