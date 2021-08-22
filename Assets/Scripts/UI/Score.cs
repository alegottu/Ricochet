using System.Collections;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private TextMeshProUGUI newPointsText = null;
    [SerializeField] private TextMeshProUGUI comboText = null;
    [SerializeField] private Animator anim = null;
    [SerializeField] private float comboFreezeTime = 0; // The amount of time to wait before a combo diappears

    private int points = 0;
    private int comboMultipiler = 0;

    private void OnEnable()
    {
        Enemy.OnEnemyKilled += AddPoints;
    }

    private void AddPoints(int points) 
    {
        comboMultipiler++;
        points *= comboMultipiler;
        this.points += points; 

        scoreText.text = this.points.ToString();
        newPointsText.text = "+" + points.ToString();
        comboText.text = comboMultipiler.ToString();
 
        anim.SetTrigger("Add"); // Should make combo and new score text flash
        StopAllCoroutines();
        StartCoroutine(TrackCombo());
    }

    private IEnumerator TrackCombo()
    {
        yield return new WaitForSeconds(comboFreezeTime);
        comboMultipiler = 0;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyKilled -= AddPoints;
    }
}
