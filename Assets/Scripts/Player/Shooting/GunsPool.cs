using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunsPool : MonoBehaviour
{
    private List<Gun> _gunPool = new List<Gun>();
    private GunsDatabase _gunsDatabase;

    private bool _isInitialized = false;
    
    public void Initialize(GunsDatabase gunsDatabase, HandRecoil handRecoil, 
        Animator handAnimator, LayerMask hitLayerMask)
    {
        if (_isInitialized) return;
        _isInitialized = true;
        
        _gunsDatabase = gunsDatabase;

        for (int i = 0; i < _gunsDatabase.Guns.Count; i++)
        {
            Gun gun = Instantiate(_gunsDatabase.Guns[i].GunPrefab, transform);
            gun.Initialize(_gunsDatabase.Guns[i].ID, handRecoil, handAnimator, hitLayerMask);
            gun.gameObject.SetActive(false);

            _gunPool.Add(gun);
        }
    }

    public Gun GetGunFromPool(int gunID)
    {
        if (gunID == -1) return null;
        
        Gun gun = _gunPool.FirstOrDefault(g => g.ID == gunID);
        
        if (gun != null)
        {
            return gun;
        }

        Debug.Log($"Gun {gun?.name ?? "null"} not found in pool");
        return null;
    }
}