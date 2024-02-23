using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    [SerializeField] private GameObject normalBulletPrefab;

    private bool canFire = true;

    public bool firing = false;
    public float rotation = 0f;

    private void Start()
    {
        if (GameStateManager.instance.gunRateOfFire <= 0f)
        {
            GameStateManager.instance.gunRateOfFire = 1f;
        }
    }

    private void Update()
    {
        if (firing)
        {
            FireBullet(rotation);
        }
    }

    public void SetFiring(bool isFiring)
	{
        firing = isFiring;
	}

    public void FireBullet(float rotation)
    {
        if (canFire)
        {
            canFire = false;

            Instantiate(normalBulletPrefab, gameObject.transform.position, Quaternion.Euler(90f, 0f, -rotation + Random.Range(-GameStateManager.instance.gunSpread, GameStateManager.instance.gunSpread)));

            StartCoroutine(EnableFire());
        }
    }

    private IEnumerator EnableFire()
    {
        yield return new WaitForSecondsRealtime(1f / GameStateManager.instance.gunRateOfFire);

        canFire = true;
    }
}
