using UnityEngine;
using System;

public class Countdown : MonoBehaviour
{
    public static event Action OnCountdownComplete;

	// Meant to be called by Animation Event
	public void FinishCountdown()
	{
		OnCountdownComplete?.Invoke();
	}
}
