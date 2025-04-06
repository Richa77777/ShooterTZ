using System.Collections;
using UnityEngine;

public class SMG : Gun
{
    private bool _isFiring;

    protected override bool CanShoot()
    {
        return base.CanShoot() && _isFiring;
    }

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isFiring = true;
            StartCoroutine(AutoFireCor());
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isFiring = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadGun();
        }
    }

    private IEnumerator AutoFireCor()
    {
        while (_isFiring && CanShoot())
        {
            base.Shot();
            yield return new WaitForSeconds(_fireRate);
        }
    }
}