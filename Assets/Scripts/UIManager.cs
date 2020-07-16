using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject dummyCamera = null;

    [SerializeField] private GameObject main = null;
    [SerializeField] private GameObject menu = null;
    [SerializeField] private GameObject pause = null;
    [HideInInspector] public GameObject gameOver = null;

    [SerializeField] private GameObject scoreText = null;
    private TextMeshProUGUI _scoreText = null;
    private GameObject newScore = null;
    [SerializeField] private float exit = 0;

    [SerializeField] private TextMeshProUGUI scoreboard = null;
    [SerializeField] private TextMeshProUGUI timer = null;
    public float time = 0;
    private bool startTime = false;

    public Animator anim = null;
    public Event.FadeOutEvent OnMenuFadeOut;
    [SerializeField] private TextMeshProUGUI countdownText = null;
    [SerializeField] private GameObject countdown = null;

    [SerializeField] private TextMeshProUGUI info = null;
    [SerializeField] private GameObject infoButtons = null;
    [SerializeField] private GameObject infoBG = null;
    [SerializeField] private float textSpeed = 1;
    private bool leaveInfo;

    private void Start()
    {
        GameManager.Instance.StartGame();
        GameManager.Instance.OnGameStateChange.AddListener(GameStateEventHandler);

        OnMenuFadeOut.AddListener(MenuFadeOutEventHandler);
    }

    private void Update()
    {
        if (GameManager.Instance.currentState == GameManager.GameState.PREGAME)
        {
            return;
        }

        switch (GameManager.Instance.currentLevel)
        {
            case "Main":
                main.SetActive(true);
                gameOver.SetActive(false);

                scoreboard.gameObject.SetActive(GameManager.Instance.getMode() == GameManager.GameMode.ARCADE);
                timer.gameObject.SetActive(GameManager.Instance.getMode() != GameManager.GameMode.ARCADE);
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

        infoButtons.SetActive(!infoBG.activeSelf);

        time = startTime ? time + Time.deltaTime : time;
        timer.text = time.ToString();

        leaveInfo = (Input.touchCount > 0 || Input.GetButtonDown("Fire1")) ? true : leaveInfo;
    }

    public void playMode(GameObject button)
    {
        switch (button.name)
        {
            case "Survival":
                GameManager.Instance.setMode(GameManager.GameMode.SURVIVAL);
                break;
            case "Arcade":
                GameManager.Instance.setMode(GameManager.GameMode.ARCADE);
                break;
            case "Hardcore":
                GameManager.Instance.setMode(GameManager.GameMode.HARDCORE);
                break;
            case "Build":
                GameManager.Instance.setMode(GameManager.GameMode.BUILD);
                break;
            default:
                GameManager.Instance.setMode(GameManager.GameMode.SURVIVAL);
                break;
        }

        menuFadeOut();
    }

    private void menuFadeOut()
    {
        anim.SetTrigger("menuFadeOut");
    }

    private void OnMenuFadeComplete()
    {
        time = 0;
        startTime = true;

        OnMenuFadeOut.Invoke(true);

        anim.ResetTrigger("menuFadeOut");
        menu.SetActive(false);
    }

    private void MenuFadeOutEventHandler(bool fadedOut)
    {
        if (fadedOut && menu.activeSelf)
        {
            GameManager.Instance.LoadLevel("Main");
        }
    }

    private void GameStateEventHandler(GameManager.GameState previous, GameManager.GameState current)
    {
        if (previous == GameManager.GameState.RUNNING && current == GameManager.GameState.RUNNING)
        {
            GameManager.Instance.UnloadLevel("Menu");
        }

        pause.SetActive(current == GameManager.GameState.PAUSED);
    }

    public void updateScore(Vector3 pos, int value)
    {
        if (newScore)
        {
            Destroy(newScore);
        }

        newScore = Instantiate(scoreText, main.transform);
        newScore.transform.position = Player.Instance.mainCam.WorldToScreenPoint(pos);
        _scoreText = newScore.GetComponentInChildren<TextMeshProUGUI>();
        string oper = value < 0 ? "" : "+";
        _scoreText.text = oper + value;
    }

    private void OnCountComplete(int count)
    {
        countdownText.text = count.ToString();

        if (count == 3)
        {
            countdown.SetActive(true);
        }

        if (count == 0)
        {
            countdown.SetActive(false);
        }
    }

    public void toggleDummy()
    {
        dummyCamera.SetActive(!dummyCamera.activeSelf);
    }

    public void GameOver()
    {
        startTime = false;
        GameManager.Instance.UpdateState(GameManager.GameState.POSTGAME);
        gameOver.SetActive(true);
    }

    public void displayInfo(GameObject button)
    {
        switch (button.name)
        {
            case "Survival":
                info.text = "Survival: Able to take 3 hits to the hull. No modifications.";
                break;
            case "Arcade":                
                info.text = "Arcade: Performance is calculated by how quickly you terminate targets and by how little ammmunition is used. Performance can be penalized by the overuse of the reflection system.";
                break;
            case "Hardcore":               
                info.text = "Hardcore: Penalties are also caused by the waste of ammunition and radiation from extensive use of the reflection system.";
                break;
            case "Build":               
                info.text = "Build: Provides that the reflection system is now able to maintain up to three structures at a time.";
                break;
        }

        leaveInfo = false;
        anim.SetBool("info", true);
        StartCoroutine(revealInfo());
    }

    private IEnumerator revealInfo()
    {
        int visibleChars = info.textInfo.characterCount;
        int ctr = 0;

        while(!leaveInfo)
        {
            info.maxVisibleCharacters = ctr;
            ctr++;

            yield return new WaitForSeconds(textSpeed);
        }

        anim.SetBool("info", false);
    }
}
