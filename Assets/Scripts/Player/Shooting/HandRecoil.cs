using System.Collections;
using UnityEngine;

public class HandRecoil : MonoBehaviour
{
    private Vector3 _originalRotation;

    private void Start()
    {
        _originalRotation = transform.localRotation.eulerAngles;
    }

    public void ApplyRecoil(float recoilAngle, float recoilTime)
    {
        StartCoroutine(RecoilEffect(recoilAngle, recoilTime));
    }

    private IEnumerator RecoilEffect(float recoilAngle, float recoilTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < recoilTime)
        {
            float recoilRotation = Mathf.Lerp(0, -recoilAngle, elapsedTime / recoilTime);
            transform.localRotation = Quaternion.Euler(_originalRotation.x + recoilRotation, _originalRotation.y, _originalRotation.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < recoilTime)
        {
            float recoilRotation = Mathf.Lerp(-recoilAngle, 0, elapsedTime / recoilTime);
            transform.localRotation = Quaternion.Euler(_originalRotation.x + recoilRotation, _originalRotation.y, _originalRotation.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}