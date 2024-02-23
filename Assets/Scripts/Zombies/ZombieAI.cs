using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    private Transform playerTransform;
    private Rigidbody rgb;
    private AudioSource audioSource;

    private int hp;

    public GameObject prefabRagdoll;
    public AudioClip zombieSound;

    private void Start()
    {
        hp = GameStateManager.instance.zombieHp;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rgb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = zombieSound;
        audioSource.loop = false;
        audioSource.volume = 0.05f;
        audioSource.pitch = Random.Range(0.1f, 2f);
        StartCoroutine(HandleZombieGrunt());
    }

    void FixedUpdate()
    {
        if (!GameStateManager.instance.gameOver)
        {
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            rgb.velocity = directionToPlayer.normalized * GameStateManager.instance.zombieSpeed;
            rgb.rotation = Quaternion.Euler(0f, Mathf.Atan2(directionToPlayer.x, directionToPlayer.z) * Mathf.Rad2Deg, 0f);
        }
        else
        {
            rgb.velocity = new Vector3(0f, 0f, 0f);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 0.1f, transform.rotation.eulerAngles.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);

            hp -= GameStateManager.instance.gunDamage;
            GameStateManager.instance.AddScore(10);

            if (hp <= 0)
            {
                OnDeath();
            }
        }
    }

    public void OnDeath()
    {
        GameStateManager.instance.PlayZombieHit();
        Instantiate(prefabRagdoll, transform.position, transform.rotation, transform.parent.transform);
        GameStateManager.instance.AddScore(50);
        ZombiePool.currentZombies--;
        Destroy(gameObject);
    }

    IEnumerator HandleZombieGrunt()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(2f, 5f));
            audioSource.Play();
        }
    }
}
