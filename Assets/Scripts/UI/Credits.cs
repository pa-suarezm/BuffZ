using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public float StartingPosition = -2400f;

	private Coroutine creditsCoroutine;
	private WaitForSecondsRealtime wait;
	private Vector3 helperPosition;

	private void Awake()
	{
		helperPosition = new Vector3(0f, StartingPosition, 0f);
		wait = new WaitForSecondsRealtime(0.01f);
	}

	private void OnEnable()
	{
		ResetPosition();

		creditsCoroutine = StartCoroutine(HandleCredits());
	}

	private void OnDisable()
	{
		StopCoroutine(creditsCoroutine);

		ResetPosition();
	}

	private void ResetPosition()
	{
		gameObject.transform.localPosition = new Vector3(0f, StartingPosition, 0f);
		helperPosition = gameObject.transform.localPosition;
	}

	private IEnumerator HandleCredits()
	{
		while(true)
		{
			if (gameObject.transform.localPosition.y >= -StartingPosition)
				ResetPosition();

			helperPosition.y += 2;
			gameObject.transform.localPosition = helperPosition;

			yield return wait;
		}
	}
}
