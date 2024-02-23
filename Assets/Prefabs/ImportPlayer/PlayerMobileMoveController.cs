using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMobileMoveController : MonoBehaviour {

	// PUBLIC
	public SimpleTouchController leftController;
	public SimpleTouchController rightController;
	public Transform headTrans;
	public float speedMovements = 5f;
	public float speedContinuousLook = 100f;
	public float speedProgressiveLook = 3000f;
	public GunController Gun;

	// PRIVATE
	private Rigidbody _rigidbody;
	[SerializeField] bool continuousRightController = true;

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		rightController.TouchEvent += RightController_TouchEvent;
	}

	public bool ContinuousRightController
	{
		set{continuousRightController = value;}
	}

	void RightController_TouchEvent (Vector2 value)
	{
		if(!continuousRightController)
		{
			//UpdateAim(value);
			UpdateRotation(value);
		}
	}

	void Update()
	{
		// move
		_rigidbody.MovePosition(transform.position + (transform.forward * leftController.GetTouchPosition.y * Time.deltaTime * speedMovements) +
			(transform.right * leftController.GetTouchPosition.x * Time.deltaTime * speedMovements) );

		if(continuousRightController)
		{
			//UpdateAim(rightController.GetTouchPosition);
			UpdateRotation(rightController.GetTouchPosition);
		}
	}

	public void UpdateRotation(Vector2 value)
    {
		float x = value.x;
		float y = value.y;
		
		if (x != 0f && y != 0f)
		{
			//The y-axis rotation can be found by using arctan(x/y) where x and y are the values returned by the joystick
			float rotation = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
			headTrans.rotation = Quaternion.Euler(0, rotation, 0);

			//Fire gun
			Gun.FireBullet(rotation);
		}
    }

	/*void UpdateAim(Vector2 value)
	{
		if(headTrans != null)
		{
			Quaternion rot = Quaternion.Euler(0f,
				transform.localEulerAngles.y - value.x * Time.deltaTime * -speedProgressiveLook,
				0f);

			_rigidbody.MoveRotation(rot);

			rot = Quaternion.Euler(headTrans.localEulerAngles.x - value.y * Time.deltaTime * speedProgressiveLook,
				0f,
				0f);
			headTrans.localRotation = rot;

		}
		else
		{

			Quaternion rot = Quaternion.Euler(transform.localEulerAngles.x - value.y * Time.deltaTime * speedProgressiveLook,
				transform.localEulerAngles.y + value.x * Time.deltaTime * speedProgressiveLook,
				0f);

			_rigidbody.MoveRotation(rot);
		}
	}*/

	void OnDestroy()
	{
		rightController.TouchEvent -= RightController_TouchEvent;
	}

}
