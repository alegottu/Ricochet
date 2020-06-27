using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject main = null;
    [SerializeField] private GameObject menu = null;
    [SerializeField] private GameObject pause = null;

    [SerializeField] private GameObject scoreText = null;
    private TextMeshProUGUI _scoreText = null;
    private GameObject newScore = null;
    [SerializeField] private float exit = 0;

    [SerializeField] private TextMeshProUGUI scoreboard = null;

    private void Start()
    {
        GameManager.Instance.OnGameStateChange.AddListener(GameStateEventHandler);
        GameManager.Instance.StartGame();
    }

    private void Update()
    {
        switch (GameManager.Instance.currentLevel)
        {
            case "Main":
                main.SetActive(true);
                menu.SetActive(false);

                scoreboard.enabled = GameManager.Instance.getMode() == GameManager.GameMode.ARCADE ? true : false;
                break;
            case "Menu":
                menu.SetActive(true);
                main.SetActive(false);
                break;
        }

        if (newScore)
        {
            _scoreText.alpha -= exit;
        }

        if (GameManager.Instance.getMode() == GameManager.GameMode.ARCADE)
        {
            scoreboard.text = GameManager.Instance.getScore().ToString();
        }
    }

    private void GameStateEventHandler(GameManager.GameState previous, GameManager.GameState current)
    {
        if (previous == GameManager.GameState.RUNNING && current == GameManager.GameState.RUNNING)
        {
            GameManager.Instance.UnloadLevel("Menu");
        }
    }

    public void playMode(GameObject button)
    {
        switch (button.name)
        {
            case "Survival Button":
                GameManager.Instance.setMode(GameManager.GameMode.SURVIVAL);
                break;
            case "Arcade Button":
                GameManager.Instance.setMode(GameManager.GameMode.ARCADE);
                break;
            case "Hardcore Button":
                GameManager.Instance.setMode(GameManager.GameMode.HARDCORE);
                break;
            default:
                GameManager.Instance.setMode(GameManager.GameMode.SURVIVAL);
                break;
        }

        GameManager.Instance.LoadLevel("Main");
    }

    public void updateScore(Vector3 pos, int value)
    {
        if (newScore)
        {
            Destroy(newScore);
        }

        newScore = Instantiate(scoreText, main.transform);
        newScore.transform.position = Camera.main.WorldToScreenPoint(pos);
        _scoreText = newScore.GetComponentInChildren<TextMeshProUGUI>();
        _scoreText.text = "+" + value;
    }
}
