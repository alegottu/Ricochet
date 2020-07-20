using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject[] Prefabs = null;
    List<GameObject> instantiatedPrefabs;
    [SerializeField] private GameObject restartContainer = null;

    List<AsyncOperation> loadOperations;
    public Event.GameStateEvent OnGameStateChange;
    public string currentLevel;

    private int score = 0;
    private int scoreMultiplier = 1; 

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

        loadOperations = new List<AsyncOperation>();

        instantiatedPrefabs = new List<GameObject>();
        InstantiatePrefabs();
    }

    private void InstantiatePrefabs()
    {
        GameObject prefInstance;
        for (int i = 0; i < Prefabs.Length; i++)
        {
            prefInstance = Instantiate(Prefabs[i]);
            instantiatedPrefabs.Add(prefInstance);
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

    public void LoadLevel()
    {
        StartCoroutine(LevelProgress(currentLevel));
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
            UpdateState(GameState.RUNNING);
        }

        if (UIManager.Instance.gameOver.activeSelf) { UIManager.Instance.gameOver.SetActive(false); }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLevel));
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
            case GameState.PREGAME:
                Time.timeScale = 1;
                break;
            case GameState.RUNNING:
                Time.timeScale = 1;
                break;
            case GameState.PAUSED:
                Time.timeScale = 0;
                break;
            default:
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
        if (_currentState == GameState.PAUSED) { TogglePause(); }

        List<GameObject> rootObjects = new List<GameObject>();
        SceneManager.GetActiveScene().GetRootGameObjects(rootObjects);
        foreach(GameObject obj in rootObjects)
        {
            Destroy(obj);
        }

        score = 0;
        UIManager.Instance.time = 0;
        UIManager.Instance.toggleDummy();
        UIManager.Instance.anim.SetTrigger("count");
        StartCoroutine(restartCountdown());
    }

    private IEnumerator restartCountdown()
    {
        yield return new WaitForSeconds(3);

        Instantiate(restartContainer);
        UIManager.Instance.toggleDummy();
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

        for (int i = 0; i < instantiatedPrefabs.Count; i++)
        {
            Destroy(instantiatedPrefabs[i]);
        }
        instantiatedPrefabs.Clear();
    }
}
