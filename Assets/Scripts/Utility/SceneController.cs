using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    [HideInInspector] public string previousLevel = string.Empty;
    public string currentLevel = string.Empty;

    [SerializeField] private GameStateManager gameState = null;
    private bool transitionActive = false;

    private void Start()
    {
        LoadLevel(currentLevel);
        SetTransitionActive(true);
    }

    public void LoadLevel(string lvl)
    {
        previousLevel = currentLevel;
        currentLevel = lvl;
        StartCoroutine(LevelProgress(lvl));
    }

    private IEnumerator LevelProgress(string lvl)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(lvl, LoadSceneMode.Additive);
        ao.completed += OnLoadComplete;

        if (transitionActive)
        {
            ao.completed += TransitionHandler;
        }

        if (ao == null)
        {
            Debug.LogError("Unable to load " + lvl);
            yield break;
        }

        while (!ao.isDone)
        {
            Debug.Log("Loading in progress: " + Mathf.Clamp(ao.progress / 0.9f, 0, 1) * 100 + "%");
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnLoadComplete(AsyncOperation ao)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLevel));
        gameState.UpdateState(GameStateManager.GameState.RUNNING);

        Debug.Log("Load complete.");
    }

    public void SetTransitionActive(bool enabled)
    {
        transitionActive = enabled;
    }

    private void TransitionHandler(AsyncOperation ao)
    {
        UnloadLevel();
    }

    public void UnloadLevel(string lvl)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(lvl);

        if (ao == null)
        {
            Debug.LogError("Unable to unload " + lvl);
            return;
        }

        ao.completed += OnUnloadComplete;
    }

    public void UnloadLevel()
    {
        UnloadLevel(previousLevel);
    }

    public void UnloadObjects(string lvl)
    {
        foreach (GameObject obj in SceneManager.GetSceneByName(lvl).GetRootGameObjects())
        {
            Destroy(obj);
        }
    }

    private void OnUnloadComplete(AsyncOperation ao)
    {
        Debug.Log("Unload complete.");
    }
}
