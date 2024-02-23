using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    public float bulletSpeed = 5f;
    public float lifetime = 1.5f;

    private Rigidbody rgb;

    private void Start()
    {
        rgb = GetComponent<Rigidbody>();
        rgb.AddRelativeForce(new Vector3(0f, bulletSpeed, 0f), ForceMode.VelocityChange);
        Destroy(gameObject, lifetime);
    }
}
