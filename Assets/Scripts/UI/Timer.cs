using TMPro;
using System.Diagnostics;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text = null;
    [SerializeField] private Health player = null;

    private Stopwatch timer = new Stopwatch();

	private void OnEnable()
    {
		Countdown.OnCountdownComplete += OnCountdownCompleteEventHandler;
		player.OnDeath += OnPlayerDeathEventHandler;
	}

    private void OnCountdownCompleteEventHandler()
    {
        timer.Start();
    }

	private void OnPlayerDeathEventHandler()
	{
		timer.Stop();
	}

    private void Update()
    {
        text.text = string.Format("{0:mm\\:ss}", timer.Elapsed);
    }

	private void OnDisable()
    {
		Countdown.OnCountdownComplete -= OnCountdownCompleteEventHandler;
		player.OnDeath -= OnPlayerDeathEventHandler;
	}
}
