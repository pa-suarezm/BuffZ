using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : MonoBehaviour
{
    public GameObject[] zombiePrefabs;
    public AudioClip[] zombieSounds;
    public Transform playerPosition;
    public float distanceToBorder = 60;

    public static int currentZombies = 6;

    private WaitForSecondsRealtime waitInSpawn = new WaitForSecondsRealtime(0.5f);

    public void Start()
    {
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        GameObject prefab;
        GameObject newZombie;
        Vector3 zombiePos = new Vector3(0f, 0f, 0f);

        while(true)
        {
            if (GameStateManager.instance.paused || GameStateManager.instance.gameOver)
            {
                yield return waitInSpawn;
                continue;
			}

            if (currentZombies <= GameStateManager.instance.zombieMaxCnt)
            {
                prefab = zombiePrefabs[Mathf.RoundToInt(Random.Range(0, zombiePrefabs.Length - 1))];

                zombiePos.x = Random.Range(-distanceToBorder, distanceToBorder);
                zombiePos.z = Random.Range(-distanceToBorder, distanceToBorder);

                while(Vector3.Distance(zombiePos, playerPosition.position) < GameStateManager.instance.zombieDistanceToPlayer)
                {
                    zombiePos.x = Random.Range(-distanceToBorder, distanceToBorder);
                    zombiePos.z = Random.Range(-distanceToBorder, distanceToBorder);
                }

                newZombie = Instantiate(prefab, zombiePos, Quaternion.Euler(0f, 0f, 0f), gameObject.transform);
                newZombie.GetComponent<ZombieAI>().zombieSound = zombieSounds[Mathf.RoundToInt(Random.Range(0f, zombieSounds.Length - 1))];
                currentZombies++;
            }

            yield return waitInSpawn;
        }
    }

	private void OnDestroy()
	{
        StopAllCoroutines();
	}
}
