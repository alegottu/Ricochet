using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public Camera mainCam = null;
    [SerializeField] private GameObject main = null;
    [SerializeField] private GameObject menu = null;
    [SerializeField] private GameObject pause = null;
    [SerializeField] private GameObject gameOver = null;

    [SerializeField] private GameObject scoreText = null;
    private TextMeshProUGUI _scoreText = null;
    private GameObject newScore = null;
    [SerializeField] private float exit = 0;

    [SerializeField] private TextMeshProUGUI scoreboard = null;
    [SerializeField] private TextMeshProUGUI timer = null;
    [HideInInspector] public float time = 0;
    private bool startTime = false;

    public Animator anim = null;
    public Event.FadeOutEvent OnMenuFadeOut;
    [SerializeField] private TextMeshProUGUI countdownText = null;
    [SerializeField] private GameObject countdown = null;

    [SerializeField] private TextMeshProUGUI info = null;
    [SerializeField] private Button[] infoButtons = null;
    [SerializeField] private CanvasGroup infoBG = null;
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

        if (newScore)
        {
            _scoreText.alpha -= exit;
        }

        if (GameManager.Instance.getMode() == GameManager.GameMode.ARCADE)
        {
            scoreboard.text = GameManager.Instance.getScore().ToString();
        }

        foreach (Button button in infoButtons)
        {
            button.interactable = infoBG.alpha > 0 ? false : true;
        }

        time = startTime ? time + Time.deltaTime : time;
        timer.text = startTime ? time.ToString() : string.Empty;

        leaveInfo = (Input.touchCount > 1 || Input.GetButtonDown("Fire1")) ? true : leaveInfo;
    }

    public void playMode(IHasInfo button)
    {
        button.getMode();
        anim.SetTrigger("menuFadeOut");
    }

    private void OnMenuFadeComplete()
    {
        time = 0;
        startTime = true;

        OnMenuFadeOut.Invoke(true);
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
        if (current == GameManager.GameState.RUNNING)
        {
            if (previous != GameManager.GameState.PREGAME)
            {
                menu.SetActive(GameManager.Instance.currentLevel.Equals("Menu"));
                gameOver.SetActive(false);
            }

            if (previous == GameManager.GameState.RUNNING)
            {
                GameManager.Instance.UnloadLevel("Menu");

                main.SetActive(true);
                scoreboard.gameObject.SetActive(GameManager.Instance.getMode() == GameManager.GameMode.ARCADE);
                timer.gameObject.SetActive(GameManager.Instance.getMode() != GameManager.GameMode.ARCADE);
            }

            if (previous == GameManager.GameState.PAUSED)
            {
                GameManager.Instance.TogglePlayObjects();
            }
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
        newScore.transform.position = mainCam.WorldToScreenPoint(pos);
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

            time = 0;
            startTime = false;
        }

        if (count == 0)
        {
            countdown.SetActive(false);

            startTime = true;
        }
    }

    public void GameOver()
    {
        startTime = false;
        GameManager.Instance.UpdateState(GameManager.GameState.POSTGAME);
        gameOver.SetActive(true);
    }

    public void displayInfo(string text)
    {
        info.maxVisibleCharacters = 0;

        info.text = text;

        leaveInfo = false;
        anim.SetBool("info", true);
    }

    private void startInfo()
    {
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

        info.text = string.Empty;
        anim.SetBool("info", false);
    }

    public void ShakeCamera()
    {
        mainCam.GetComponent<MainCamera>().anim.SetTrigger("shake");
    }
}
