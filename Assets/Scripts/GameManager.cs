using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject[] Prefabs = null;
    List<GameObject> instantiatedPrefabs;

    List<AsyncOperation> loadOperations;
    [SerializeField] private string levelToLoad;
    private bool loading = false;

    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }
    public GameState currentState
    {
        get { return currentState; }
        private set { currentState = value; }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        loadOperations = new List<AsyncOperation>();
        LoadLevel(levelToLoad);

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

    private void OnLoadComplete(AsyncOperation ao)
    {
        if(loadOperations.Contains(ao))
        {
            loadOperations.Remove(ao);
        }

        Debug.Log("Load complete.");
    }

    public void LoadLevel(string lvl)
    {
        levelToLoad = lvl;
        StartCoroutine(LevelProgress(lvl));
    }

    public void LoadLevel()
    {
        StartCoroutine(LevelProgress(levelToLoad));
    }

    public IEnumerator LevelProgress(string lvl)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(lvl, LoadSceneMode.Additive);
        ao.completed += OnLoadComplete;

        if (ao == null)
        {
            Debug.LogError("Unable to load " + lvl);
            yield break;
        }

        loadOperations.Add(ao);

        while (!ao.isDone)
        {
            Debug.Log(Mathf.Clamp(ao.progress / 0.9f, 0, 1));
            loading = true;
            yield return null;
        }

        loading = false;
    }

    public IEnumerator LevelProgress()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive);
        ao.completed += OnLoadComplete;

        if (ao == null)
        {
            Debug.LogError("Unable to load " + levelToLoad);
            yield break;
        }

        loadOperations.Add(ao);

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
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelToLoad);
        ao.completed += OnUnloadComplete;

        if (ao == null)
        {
            Debug.LogError("Unable to load " + levelToLoad);
            return;
        }
    }

    private void UpdateState(GameState state)
    {
        currentState = state;

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
