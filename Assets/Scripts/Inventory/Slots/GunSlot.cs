using System;
using UnityEngine;
using UnityEngine.UI;

public class GunSlot : MonoBehaviour
{
    [SerializeField] private Image _mainImage;
    private GunsDatabase _gunsDatabase;

    public int CurrentGunId { get; private set; } = -1;
    
    public void Initialize(GunsDatabase gunsDatabase)
    {
        _gunsDatabase = gunsDatabase;
    }

    public void SetGun(int gunId)
    {
        CurrentGunId = gunId;
        UpdateGunSprite(_gunsDatabase.GetGunEntryById(gunId).Icon);
    }

    public void RemoveGun()
    {
        CurrentGunId = -1;
        UpdateGunSprite(null);
    }
    
    private void UpdateGunSprite(Sprite gunSprite)
    {
        _mainImage.sprite = gunSprite;
    }
}