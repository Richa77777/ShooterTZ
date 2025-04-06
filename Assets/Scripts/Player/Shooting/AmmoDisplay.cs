using TMPro;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ammoCounter;
    
    public void UpdateAmmoCounter(int currentAmmo, int maxAmmo)
    {
        _ammoCounter.text = $"{currentAmmo} / {maxAmmo}";
    }
}
