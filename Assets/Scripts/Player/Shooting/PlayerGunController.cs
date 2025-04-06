using UnityEngine;

[RequireComponent(typeof(AmmoDisplay))]
public class PlayerGunController : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private Gun _startGunPrefab;
    [SerializeField] private LayerMask _hitLayerMask;

    [Header("Guns Database")]
    [SerializeField] private GunsDatabase _gunsDatabase;

    [Header("Guns Pool")]
    [SerializeField] private GunsPool _gunsPool;

    [Space(10f), Header("Inventory")]
    [SerializeField] private Inventory _inventory;

    [Header("Hand Components")]
    [SerializeField] private HandRecoil _handRecoil;
    [SerializeField] private Animator _handAnimator;

    private Gun _currentGun;
    private AmmoDisplay _ammoDisplay;
    
    private void Awake()
    {
        _ammoDisplay = GetComponent<AmmoDisplay>();
        
        _gunsPool.Initialize(_gunsDatabase, _handRecoil, _handAnimator, _hitLayerMask);
        _inventory.Initialize(_gunsDatabase);
    }

    private void OnEnable()
    {
        _inventory.OnGunSelected += SetGun;
        _inventory.OnExtraItemUsed += AddAmmo;
        AddStartGun();
    }

    private void OnDisable()
    {
        _inventory.OnGunSelected -= SetGun;
        _inventory.OnExtraItemUsed -= AddAmmo;
    }
    

    private void AddStartGun()
    {
        if (_startGunPrefab != null)
        {
            _inventory.AddGun(_gunsDatabase.GetIdByPrefab(_startGunPrefab));
            _startGunPrefab = null;
        }
    }
    
    private void SetGun(int gunID)
    {
        if (_currentGun != null)
        {
            // Unsubscribes From Old Gun
            _currentGun.OnReloadEnd -= UpdateAmmoCounter;
            _currentGun.OnShot -= UpdateAmmoCounter;

            _currentGun.gameObject.SetActive(false);
        }

        // New Gun
        _currentGun = _gunsPool.GetGunFromPool(gunID);
        if (_currentGun == null) return;
        
        _currentGun.gameObject.SetActive(true);
        
        _currentGun.OnReloadEnd += UpdateAmmoCounter;
        _currentGun.OnShot += UpdateAmmoCounter;

        UpdateAmmoCounter();
    }

    private void AddAmmo(ItemType itemType)
    {
        if (itemType == ItemType.AmmoPack)
        {
            _currentGun?.AddAmmo(_currentGun.AmmoPackRefillAmount);
            UpdateAmmoCounter();
        }
    }

    private void UpdateAmmoCounter()
    {
        _ammoDisplay.UpdateAmmoCounter(_currentGun.CurrentAmmo, _currentGun.CurrentMaxAmmo);
    }
}