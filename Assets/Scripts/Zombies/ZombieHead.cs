using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHead : MonoBehaviour
{
    float randomX;
    float randomY;
    float randomZ;
    // Update is called once per frame
    void Update()
    {
        randomX = Random.Range(-80f, 80f);
        randomY = Random.Range(-80f, 80f);
        randomZ = Random.Range(-80f, 80f);

        transform.rotation = Quaternion.Euler(randomX, randomY, randomZ);
    }
}
