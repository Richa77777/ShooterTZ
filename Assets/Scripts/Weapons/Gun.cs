using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Parameters")]
    [SerializeField] protected string _name = "";
    [SerializeField] private int _damage = 10;
    [SerializeField] protected float _fireRate = 0.5f;
    [SerializeField] private float _shotDistance = 100f;
    private float _lastShotTime;
    private LayerMask _hitLayerMask;

    [Header("Ammo Parameters")]
    [SerializeField] private int _maxAmmo = 100;
    [SerializeField] private int _magazineCapacity = 10;

    [Header("Recoil Parameters")]
    [SerializeField] private float _recoilAngle = 5f;
    [SerializeField] private float _recoilTime = 0.1f;
    private HandRecoil _handRecoil;
    private Animator _handAnimator;

    private Camera _camera;
    private Coroutine _reloadCor;

    private bool _isInitialized = false;

    public event Action OnShot;
    public event Action OnReloadEnd;

    [field: Space(20f), SerializeField] public int AmmoPackRefillAmount { get; private set; }
    public int ID { get; private set; } = -1;
    public int CurrentMaxAmmo { get; private set; }
    public int CurrentAmmo { get; private set; }

    public void Initialize(int id, HandRecoil handRecoil, Animator handAnimator, LayerMask hitLayerMask)
    {
        if (_isInitialized) return;
        _isInitialized = true;

        ID = id;
        _handRecoil = handRecoil;
        _handAnimator = handAnimator;
        _hitLayerMask = hitLayerMask;

        LoadAmmoState();
    }

    private void Awake()
    {
        _camera = Camera.main;
        CurrentMaxAmmo = _maxAmmo;
        CurrentAmmo = _magazineCapacity;
    }

    private void OnDisable()
    {
        if (_reloadCor != null) StopCoroutine(_reloadCor);
        _reloadCor = null;
    }

    private void LoadAmmoState()
    {
        int savedAmmo = PlayerPrefs.GetInt($"Gun_{ID}_Ammo", _magazineCapacity);
        int savedMaxAmmo = PlayerPrefs.GetInt($"Gun_{ID}_MaxAmmo", _maxAmmo);

        CurrentAmmo = savedAmmo;
        CurrentMaxAmmo = savedMaxAmmo;
    }

    private void SaveAmmoState()
    {
        PlayerPrefs.SetInt($"Gun_{ID}_Ammo", CurrentAmmo);
        PlayerPrefs.SetInt($"Gun_{ID}_MaxAmmo", CurrentMaxAmmo);
        PlayerPrefs.Save();
    }

    public void AddAmmo(int count)
    {
        CurrentMaxAmmo = Mathf.Clamp(CurrentMaxAmmo + count, 0, _maxAmmo);
        SaveAmmoState();
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0)) Shot();
        if (Input.GetKeyDown(KeyCode.R)) ReloadGun();
    }

    protected virtual void Shot()
    {
        if (CurrentAmmo <= 0 || _reloadCor != null || Time.time - _lastShotTime < _fireRate) return;

        _lastShotTime = Time.time;
        CurrentAmmo--;

        CheckShotHit();
        ShotAnimation();

        OnShot?.Invoke();

        if (CurrentAmmo <= 0) ReloadGun();
        SaveAmmoState();
    }

    private void CheckShotHit()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, _shotDistance, _hitLayerMask))
        {
            hit.collider.gameObject.GetComponent<IDamagable>().TakeDamage(_damage);
            Debug.Log($"Hit: {hit.collider.name} at {hit.point}");
        }
        else
        {
            Debug.Log("Missed!");
        }
    }

    protected virtual bool CanShoot()
    {
        return CurrentAmmo > 0 && _reloadCor == null && Time.time - _lastShotTime >= _fireRate;
    }

    private void ShotAnimation()
    {
        _handRecoil.ApplyRecoil(_recoilAngle, _recoilTime);
    }

    protected virtual void ReloadGun()
    {
        if (_reloadCor == null)
            _reloadCor = StartCoroutine(ReloadCor());
    }

    private IEnumerator ReloadCor()
    {
        if (CurrentAmmo == _magazineCapacity || CurrentMaxAmmo <= 0) yield break;

        _handAnimator.SetTrigger("Reload");

        yield return new WaitForSeconds(_handAnimator.GetCurrentAnimatorClipInfo(0).Length + 0.05f);

        int ammoNeeded = _magazineCapacity - CurrentAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, CurrentMaxAmmo);

        CurrentAmmo += ammoToLoad;
        CurrentMaxAmmo -= ammoToLoad;

        OnReloadEnd?.Invoke();

        _reloadCor = null;

        SaveAmmoState();
    }
}