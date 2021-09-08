using System.Collections;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private TextMeshProUGUI newPointsText = null;
    [SerializeField] private TextMeshProUGUI comboText = null;
    [SerializeField] private Animator anim = null;
    [SerializeField] private Health player = null;
    [SerializeField] private float comboFreezeTime = 0; // The amount of time to wait before a combo diappears

    private int points = 0;
    private int comboMultiplier = 0;

    private void OnEnable()
    {
        Enemy.OnEnemyKilled += AddPoints;
        player.OnDamageTaken += OnPlayerDamageEventHandler;
    }

    private void OnPlayerDamageEventHandler()
    {
        comboMultiplier = 0;
        StopAllCoroutines();
        StartCoroutine(TrackCombo());
    }

    private void AddPoints(Enemy enemy) 
    {
        comboMultiplier++;
        points *= comboMultiplier;
        points += enemy.GetPoints(); 

        scoreText.text = points.ToString();
        newPointsText.text = "+" + points.ToString();
        comboText.text = comboMultiplier.ToString();
 
        anim.SetTrigger("Add"); // Should make combo and new score text flash
        StopAllCoroutines();
        StartCoroutine(TrackCombo());
    }

    private IEnumerator TrackCombo()
    {
        yield return new WaitForSeconds(comboFreezeTime);
        comboMultiplier = 0;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyKilled -= AddPoints;
        player.OnDamageTaken -= OnPlayerDamageEventHandler;
    }
}
