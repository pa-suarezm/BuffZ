using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRagdoll : MonoBehaviour
{
    public float lifetime = 3f;

    private Rigidbody rgb;

    private void Start()
    {
        rgb = GetComponent<Rigidbody>();
        rgb.AddRelativeForce(new Vector3(0f, 0f, -500f), ForceMode.Impulse);
        GameStateManager.instance.ShakeCameraFor(0.1f);
        Destroy(gameObject, lifetime);
    }
}
