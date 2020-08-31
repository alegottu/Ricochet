using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject[] Prefabs = null;
    List<GameObject> systemPrefabs = new List<GameObject>();
    public SpriteRenderer bounds = null;

    List<AsyncOperation> loadOperations = new List<AsyncOperation>();
    public Event.GameStateEvent OnGameStateChange;
    public string currentLevel;

    private int score = 0;
    private int scoreMultiplier = 1;

    [SerializeField] private float _bgSpeed = 1;
    [HideInInspector] public float bgSpeed = 1;

    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED,
        POSTGAME
    }
    private GameState _currentState;
    public GameState currentState
    {
        get { return _currentState; }
        private set { currentState = value; }
    }

    public enum GameMode
    {
        SURVIVAL,
        ARCADE,
        HARDCORE,
        BUILD
    }
    private GameMode currentMode;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        InstantiatePrefabs();
    }

    private void InstantiatePrefabs()
    {
        GameObject prefInstance;
        for (int i = 0; i < Prefabs.Length; i++)
        {
            prefInstance = Instantiate(Prefabs[i]);
            systemPrefabs.Add(prefInstance);
        }
    }

    public void addScore(int value)
    {
        score += (value * scoreMultiplier);
    }

    public int getScore()
    {
        return score;
    }

    public void LoadLevel(string lvl)
    {
        currentLevel = lvl;
        StartCoroutine(LevelProgress(lvl));
    }

    public IEnumerator LevelProgress(string lvl)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(lvl, LoadSceneMode.Additive);
        loadOperations.Add(ao);
        ao.completed += OnLoadComplete;

        if (ao == null)
        {
            Debug.LogError("Unable to load " + lvl);
            yield break;
        }

        while (!ao.isDone)
        {
            Debug.Log(Mathf.Clamp(ao.progress / 0.9f, 0, 1));
            yield return null;
        }
    }

    private void OnLoadComplete(AsyncOperation ao)
    {
        if (loadOperations.Contains(ao))
        {
            loadOperations.Remove(ao);
        }

        if (loadOperations.Count < 1)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLevel));
            UpdateState(GameState.RUNNING);
        }

        Debug.Log("Load complete.");
    }

    public void UnloadLevel(string lvl)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(lvl);
        ao.completed += OnUnloadComplete;

        if (ao == null)
        {
            Debug.LogError("Unable to load " + lvl);
            return;
        }
    }

    public void UnloadLevel()
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(currentLevel);
        ao.completed += OnUnloadComplete;

        if (ao == null)
        {
            Debug.LogError("Unable to load " + currentLevel);
            return;
        }
    }

    private void OnUnloadComplete(AsyncOperation ao)
    {
        Debug.Log("Unload complete.");
    }

    public void UpdateState(GameState state)
    {
        GameState previousState = _currentState;
        _currentState = state;

        switch(currentState)
        {
            case GameState.PAUSED:
                bgSpeed = 0;
                Time.timeScale = 0;
                break;
            default:
                bgSpeed = _bgSpeed;
                Time.timeScale = 1;
                break;
        }

        OnGameStateChange.Invoke(previousState, _currentState);
    }

    public void setMode(GameMode mode)
    {
        currentMode = mode;
    }

    public GameMode getMode()
    {
        return currentMode;
    }

    public void StartGame()
    {
        LoadLevel(currentLevel);
        UpdateState(GameState.PREGAME);
    }

    public void TogglePause()
    {
        UpdateState(_currentState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }

    public void Restart()
    {
        UnloadLevel("Main");

        score = 0;
        UIManager.Instance.time = 0;
        UIManager.Instance.anim.SetTrigger("count");
        StartCoroutine(restartCountdown());
    }

    private IEnumerator restartCountdown()
    {
        LoadLevel("Main");
        yield return new WaitForSeconds(3);

        TogglePlayObjects();
    }

    public void TogglePlayObjects()
    {
        GameObject master = SceneManager.GetActiveScene().GetRootGameObjects()[0];
        master.SetActive(!master.activeSelf);
    }

    public void Quit()
    {
        LoadLevel("Menu");
        UpdateState(GameState.PREGAME);
        UnloadLevel("Main");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        for (int i = 0; i < systemPrefabs.Count; i++)
        {
            Destroy(systemPrefabs[i]);
        }
        systemPrefabs.Clear();
    }
}
