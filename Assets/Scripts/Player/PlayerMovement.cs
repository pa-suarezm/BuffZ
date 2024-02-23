using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;
    public Rigidbody rgbLeftJoystickTarget;
    public Transform transformRightJoystickTarget;
    public GunController Gun;
    public GameObject prefabRagdoll;
    public Animator characterAnim;

    private bool isRagdoll = false;

    private bool canTakeDamage;

    private float leftStickRotation;
    private float rightStickRotation;
    private float stickDifference;

    private void Start()
    {
        canTakeDamage = true;

        leftStickRotation = 0f;
        rightStickRotation = 0f;
        stickDifference = 0f;
    }

    private void FixedUpdate()
    {
        characterAnim.SetBool("running", Mathf.Abs(rgbLeftJoystickTarget.velocity.magnitude) > 0.1f);

        if (GameStateManager.instance.gameOver)
        {
            rgbLeftJoystickTarget.velocity = new Vector3(0f, 0f, 0f);
        }
    }

    private void Update()
    {
        if (GameStateManager.instance.gameOver && !isRagdoll)
        {
            isRagdoll = true;
            GameObject playerRagdoll = GameObject.FindGameObjectWithTag("PlayerRagdoll");
            if (playerRagdoll != null) transform.position = new Vector3(playerRagdoll.transform.position.x, transform.position.y, playerRagdoll.transform.position.z);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!GameStateManager.instance.gameOver)
        {
            Vector2 movement = context.ReadValue<Vector2>();

            float x = movement.x;
            float y = movement.y;

            if (x != 0f && y != 0f)
            {
                leftStickRotation = Mathf.Atan2(x, y) * Mathf.Rad2Deg;

                stickDifference = leftStickRotation - rightStickRotation;

                if (stickDifference < 0f)
                {
                    if (stickDifference > -45f || stickDifference < -135f)
                    {
                        stickDifference *= -1;
                    }
                }

                characterAnim.SetFloat("rotationDifference", stickDifference);
            }

            rgbLeftJoystickTarget.velocity = new Vector3(x * GameStateManager.instance.playerSpeed, 0f, y * GameStateManager.instance.playerSpeed);
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
        if (!GameStateManager.instance.gameOver)
        {
            if (context.action.ToString().Contains("Mouse"))
            {
                //Handle Mouse input
                Vector2 mouse = context.ReadValue<Vector2>();
                Vector2 screenCenter = new Vector2(mainCamera.pixelWidth / 2, mainCamera.pixelHeight / 2);
                Vector2 mouseRelativeToCenter = new Vector2(mouse.x - screenCenter.x, mouse.y - screenCenter.y).normalized;

                UpdateRotation(mouseRelativeToCenter);
            }
            else
            {
                UpdateRotation(context.ReadValue<Vector2>());
            }

        }
    }

    public void TouchMove(Vector2 movement)
	{
        if (!GameStateManager.instance.gameOver)
        {
            float x = ((1 - movement.x) - 0.5f) * 2f;
            float y = ((1 - movement.y) - 0.5f) * 2f;

            if (x != 0f && y != 0f)
            {
                leftStickRotation = Mathf.Atan2(x, y) * Mathf.Rad2Deg;

                stickDifference = leftStickRotation - rightStickRotation;

                if (stickDifference < 0f)
                {
                    if (stickDifference > -45f || stickDifference < -135f)
                    {
                        stickDifference *= -1;
                    }
                }

                characterAnim.SetFloat("rotationDifference", stickDifference);
            }

            rgbLeftJoystickTarget.velocity = new Vector3(x * GameStateManager.instance.playerSpeed, 0f, y * GameStateManager.instance.playerSpeed);
        }
    }

    public void TouchLook(Vector2 value)
    {
        Vector2 newValue = new Vector2(((1 - value.x) - 0.5f) * 2f, ((1 - value.y) - 0.5f) * 2f);
        UpdateRotation(newValue);
	}

    public void UpdateRotation(Vector2 value)
    {
        float x = value.x;
        float y = value.y;

        if (x != 0f && y != 0f)
        {
            //The y-axis rotation can be found by using arctan(x/y) where x and y are the values returned by the joystick
            float rotation = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
            transformRightJoystickTarget.rotation = Quaternion.Euler(0, rotation, 0);

            //Update gun rotation
            Gun.rotation = rotation;
            rightStickRotation = rotation;

            stickDifference = leftStickRotation - rightStickRotation;

            if (stickDifference < 0f)
            {
                if (stickDifference > -45f || stickDifference < -135f)
                {
                    stickDifference *= -1;
                }
            }

            characterAnim.SetFloat("rotationDifference", stickDifference);

            //Fire gun
            //Gun.FireBullet(rotation);
        }
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (!GameStateManager.instance.gameOver)
        {
            if (context.action.phase.Equals(InputActionPhase.Performed))
            {
                Gun.firing = true;
            }
            else if (context.action.phase.Equals(InputActionPhase.Canceled))
            {
                Gun.firing = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!GameStateManager.instance.gameOver)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (canTakeDamage)
                {
                    StopAllCoroutines();
                    canTakeDamage = false;
                    StartCoroutine(blinkPlayer());
                    GameStateManager.instance.playerHp -= GameStateManager.instance.zombieDamage;

                    if (GameStateManager.instance.playerHp <= 0)
                    {
                        GameStateManager.instance.playerHp = 0;
                        GameStateManager.instance.SetGameOver(true);
                        OnDeath();
                    }
                }
            }
        }
    }

    IEnumerator blinkPlayer()
    {
        GameObject playerModel = GameObject.FindGameObjectWithTag("PlayerModel");
        int i = 0;
        while(i < 25)
        {
            yield return new WaitForSeconds(0.1f);
            playerModel.SetActive(!playerModel.activeInHierarchy);
            i++;
        }

        playerModel.SetActive(true);
        canTakeDamage = true;

        yield return true;
    }

    private void OnDeath()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        StopAllCoroutines();
        Instantiate(prefabRagdoll, transform.position, transform.rotation);
        GameObject playerModel = GameObject.FindGameObjectWithTag("PlayerModel");
        playerModel.SetActive(false);
        rgbLeftJoystickTarget.velocity = new Vector3(0f, 0f, 0f);
        Gun.firing = false;
    }
}
