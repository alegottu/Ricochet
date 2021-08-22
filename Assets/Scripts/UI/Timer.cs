using TMPro;
using System.Diagnostics;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text = null;

    private Stopwatch timer = new Stopwatch();

    private void Awake()
    {
        timer.Start();
    }

    private void Update()
    {
        text.text = string.Format("{0:mm\\:ss}", timer.Elapsed);
    }
}
