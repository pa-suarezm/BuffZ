using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    public CameraShake cameraShakeController;
    public GameObject mainMenuScreen;
    public GameObject gameOverScreen;
    public GameObject gameplayHUD;
    public Text scoreTxt;
    public Text currentScore;
    public Text hiScoreTxt;
    public int score;
    public int hiScore;
    private bool newHiScore;
    public AudioClip[] zombieHitSound;
    private AudioSource audioSource;
    [HideInInspector] public bool paused;
    [HideInInspector] public bool gameOver;

    //Zombie params
    public int zombieMaxCnt = 50;
    public int zombieHp = 8;
    public float zombieSpeed = 5;
    public float zombieDistanceToPlayer = 50f;
    public int zombieDamage = 1;

    //Player params
    public int playerHp = 5;
    public float playerSpeed = 10f;

    //Gun params
    public int gunDamage = 1;
    public float gunRateOfFire = 2f;
    public float gunSpread = 1f;

    //Buff Nerf params
    public BuffNerfTarget currentTarget;
    public PlayerBuffsNerfs currentPlayerBuffsNerfs;
    public EnemiesBuffsNerfs currentEnemiesBuffsNerfs;
    public BuffNerf currentType;

    private void Awake()
    {
        if (instance == null)
        {
            Application.targetFrameRate = 30;
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        audioSource = GetComponent<AudioSource>();

        mainMenuScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        gameplayHUD.SetActive(false);

        paused = false;
        gameOver = true;
        score = 0;
        hiScore = PlayerPrefs.GetInt("HiScore");

        GoogleMobileAdsScript.Instance.LoadAd();
    }

    private void Update()
    {
        currentScore.text = score.ToString();
        hiScoreTxt.text = "HiScore: " + hiScore.ToString();
    }

    public void SetGameOver(bool isGameOver)
	{
        gameOver = isGameOver;
        if (gameOver)
        {
            gameOverScreen.SetActive(true);
            GoogleMobileAdsScript.Instance.LoadAd();
            gameplayHUD.SetActive(false);
            scoreTxt.text = "Your score: " + score.ToString() + "\nHiScore: " + hiScore.ToString();
            if (newHiScore)
			{
                PlayerPrefs.SetInt("HiScore", hiScore);
			}
        }
        else
        {
            GoogleMobileAdsScript.Instance.DestroyAd();
            mainMenuScreen.SetActive(false);
            gameplayHUD.SetActive(true);
        }
	}

    public void AddScore(int scoreToAdd)
	{
        score += scoreToAdd;
        if (hiScore <= score)
		{
            hiScore = score;
            newHiScore = true;
		}
	}

    public void ShakeCameraFor(float duration)
    {
        cameraShakeController.shakeDuration = duration;
    }

    public void GenerateBuffNerf()
    {
        float randomF = Random.Range(1f, 100f);

        //Decide whether to Buff or Nerf
        if (randomF <= 80)
        {
            currentType = BuffNerf.BUFF;
        }
        else //randomF <= 100
        {
            currentType = BuffNerf.NERF;
        }

        //Switch target
        if (currentTarget.Equals(BuffNerfTarget.ENEMIES))
        {
            currentTarget = BuffNerfTarget.PLAYER;

            SelectPlayerBuffNerf(Random.Range(1f, 100f));
        }
        else
        {
            currentTarget = BuffNerfTarget.ENEMIES;

            SelectEnemiesBuffNerf(Random.Range(1f, 100f));
        }
    }

    private void SelectPlayerBuffNerf(float pRandom)
    {
        if (pRandom <= 8f)
        {
            currentPlayerBuffsNerfs = PlayerBuffsNerfs.PLAYER_HP;
            playerHp += currentType.Equals(BuffNerf.BUFF) ? 1 : -1;
            if (playerHp == 0)
            {
                playerHp = 1;
                SelectPlayerBuffNerf(Random.Range(1f, 100f));
            }
        }
        else if (pRandom <= 25f)
        {
            currentPlayerBuffsNerfs = PlayerBuffsNerfs.PLAYER_SPEED;
            playerSpeed += currentType.Equals(BuffNerf.BUFF) ? 6 : -4;

            if (playerSpeed > 20)
            {
                playerSpeed = 20;
                SelectPlayerBuffNerf(Random.Range(1f, 100f));
            }

            if (playerSpeed <= 0 || playerSpeed < zombieSpeed)
            {
                playerSpeed = Mathf.Max(zombieSpeed, 2f);
                SelectPlayerBuffNerf(Random.Range(1f, 100f));
            }
        }
        else if (pRandom <= 60f)
        {
            currentPlayerBuffsNerfs = PlayerBuffsNerfs.PLAYER_DAMAGE;
            gunDamage += currentType.Equals(BuffNerf.BUFF) ? 4 : -2;
            if (gunDamage <= 0)
            {
                gunDamage = 1;
                SelectPlayerBuffNerf(Random.Range(1f, 100f));
            }
        }
        else if (pRandom <= 80f)
        {
            currentPlayerBuffsNerfs = PlayerBuffsNerfs.PLAYER_RATE_OF_FIRE;
            gunRateOfFire += currentType.Equals(BuffNerf.BUFF) ? 15 : -5;
            if (gunRateOfFire < 10)
            {
                gunRateOfFire = 10;
                SelectPlayerBuffNerf(Random.Range(1f, 100f));
            }
        }
        else //pRandom <= 100
        {
            currentPlayerBuffsNerfs = PlayerBuffsNerfs.PLAYER_SPREAD;
            gunSpread += currentType.Equals(BuffNerf.BUFF) ? -5 : 5;
            if (gunSpread < 0)
            {
                gunSpread = 0;
                SelectPlayerBuffNerf(Random.Range(1f, 100f));
            }
        }
    }

    private void SelectEnemiesBuffNerf(float pRandom)
    {
        if (pRandom <= 20f)
        {
            currentEnemiesBuffsNerfs = EnemiesBuffsNerfs.ZOMBIE_MAX_COUNT;
            zombieMaxCnt += currentType.Equals(BuffNerf.BUFF) ? 50 : -50;
            if (zombieMaxCnt < 50)
            {
                zombieMaxCnt = 50;
                SelectEnemiesBuffNerf(Random.Range(1f, 100f));
            }
            else if (zombieMaxCnt > 300)
            {
                SelectEnemiesBuffNerf(Random.Range(1f, 100f));
            }
        }
        else if (pRandom <= 40f)
        {
            currentEnemiesBuffsNerfs = EnemiesBuffsNerfs.ZOMBIE_HP;
            zombieHp += currentType.Equals(BuffNerf.BUFF) ? 2 : -2;
            if (zombieHp == 0)
            {
                zombieHp = 2;
                SelectEnemiesBuffNerf(Random.Range(1f, 100f));
            }
        }
        else if (pRandom <= 60f)
        {
            currentEnemiesBuffsNerfs = EnemiesBuffsNerfs.ZOMBIE_SPEED;
            zombieSpeed += currentType.Equals(BuffNerf.BUFF) ? 4 : -1;

            if (zombieSpeed > 19)
            {
                zombieSpeed = 19;
                SelectEnemiesBuffNerf(Random.Range(1f, 100f));
            }

            if (zombieSpeed <= 0 || zombieSpeed > playerSpeed)
            {
                zombieSpeed = Mathf.Max(playerSpeed, 1);
                SelectEnemiesBuffNerf(Random.Range(1f, 100f));
            }
        }
        else if (pRandom <= 80f)
        {
            currentEnemiesBuffsNerfs = EnemiesBuffsNerfs.ZOMBIE_DISTANCE_TO_PLAYER;
            zombieDistanceToPlayer += currentType.Equals(BuffNerf.BUFF) ? -5 : 5;
            if (zombieDistanceToPlayer == 0 || zombieDistanceToPlayer == 50)
            {
                zombieDistanceToPlayer = 30;
                SelectEnemiesBuffNerf(Random.Range(1f, 100f));
            }
        }
        else //pRandom <= 100
        {
            currentEnemiesBuffsNerfs = EnemiesBuffsNerfs.ZOMBIE_DAMAGE;
            zombieDamage += currentType.Equals(BuffNerf.BUFF) ? 1 : -1;
            if (zombieDamage < 1)
            {
                zombieDamage = 1;
                SelectEnemiesBuffNerf(Random.Range(1f, 100f));
            }
        }
    }

    public void Retry()
    {
        GoogleMobileAdsScript.Instance.DestroyAd();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        GoogleMobileAdsScript.Instance.DestroyAd();
        Application.Quit();
    }

    public void PlayZombieHit()
    {
        float random = Random.Range(0f, zombieHitSound.Length - 1);
        AudioClip hitSound = zombieHitSound[Mathf.RoundToInt(random)];
        audioSource.pitch = Random.Range(0.5f, 1.5f);
        audioSource.PlayOneShot(hitSound);
    }

    public enum BuffNerfTarget
    {
        PLAYER,
        ENEMIES
    }

    public enum PlayerBuffsNerfs
    {
        PLAYER_HP,
        PLAYER_SPEED,
        PLAYER_DAMAGE,
        PLAYER_RATE_OF_FIRE,
        PLAYER_SPREAD
    }

    public enum EnemiesBuffsNerfs
    {
        ZOMBIE_MAX_COUNT,
        ZOMBIE_HP,
        ZOMBIE_SPEED,
        ZOMBIE_DISTANCE_TO_PLAYER,
        ZOMBIE_DAMAGE
    }

    public enum BuffNerf
    {
        BUFF,
        NERF
    }
}
