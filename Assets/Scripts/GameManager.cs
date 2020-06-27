using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable] public class GameStateEvent : UnityEvent<GameManager.GameState, GameManager.GameState> { }

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject[] Prefabs = null;
    List<GameObject> instantiatedPrefabs;

    List<AsyncOperation> loadOperations;
    public GameStateEvent OnGameStateChange;
    public string currentLevel;

    private int score = 0;
    private int scoreMultiplier = 1;

    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
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
        HARDCORE
    }
    private GameMode currentMode;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        loadOperations = new List<AsyncOperation>();

        instantiatedPrefabs = new List<GameObject>();
        InstantiatePrefabs();
    }

    public void StartGame()
    {
        LoadLevel(currentLevel);
        UpdateState(GameState.PREGAME);
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

    private void OnLoadComplete(AsyncOperation ao)
    {
        if(loadOperations.Contains(ao))
        {
            loadOperations.Remove(ao);
        }

        if (loadOperations.Count < 1)
        {
            UpdateState(GameState.RUNNING);
        }

        Debug.Log("Load complete.");
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

    private void OnUnloadComplete(AsyncOperation ao)
    {
        Debug.Log("Unload complete.");
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

    private void UpdateState(GameState state)
    {
        GameState previousState = _currentState;
        _currentState = state;

        switch(currentState)
        {
            case GameState.PREGAME:
                break;

            case GameState.RUNNING:
                break;

            case GameState.PAUSED:
                break;

            default:
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
