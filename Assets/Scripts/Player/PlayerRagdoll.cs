using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    public Rigidbody rgb;

    private void Start()
    {
        rgb.AddRelativeForce(new Vector3(0f, 200f, 0f), ForceMode.Impulse);
        GameStateManager.instance.ShakeCameraFor(0.2f);
    }
}
