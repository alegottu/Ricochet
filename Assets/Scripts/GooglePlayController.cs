using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayController : Singleton<GooglePlayController>
{
    [HideInInspector] public bool authenticating = false;
    [HideInInspector] public bool authenticated = false;

#if UNITY_ANDROID
    private void Start()
    {
        Authenticate();
    }

    public void UploadScore(int score, GameManager.GameMode mode)
    {
        if (authenticated)
        {
            string ID = mode == GameManager.GameMode.SURVIVAL ? GPGSIds.leaderboard_survival : mode == GameManager.GameMode.ARCADE ? GPGSIds.leaderboard_arcade : mode == GameManager.GameMode.HARDCORE ? GPGSIds.leaderboard_hardcore : GPGSIds.leaderboard_build;
            Social.ReportScore(score, ID, (bool success) =>
            {
                Debug.Log(success ? "Reported score successfully" : "Failed to report score");
            });
        }
    }

    public void UploadScore(float time, GameManager.GameMode mode)
    {
        if (authenticated)
        {
            string ID = mode == GameManager.GameMode.SURVIVAL ? GPGSIds.leaderboard_survival : mode == GameManager.GameMode.ARCADE ? GPGSIds.leaderboard_arcade : mode == GameManager.GameMode.HARDCORE ? GPGSIds.leaderboard_hardcore : GPGSIds.leaderboard_build;
            Social.ReportScore((long)time, ID, (bool success) =>
            {
                Debug.Log(success ? "Reported score successfully" : "Failed to report score");
            });
        }
    }

    public void Authenticate()
    {
        if (authenticated || authenticating)
        {
            Debug.LogWarning("Ignoring repeated call to Authenticate().");
            return;
        }

        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .Build();
        PlayGamesPlatform.InitializeInstance(config);

        // Activate the Play Games platform. This will make it the default
        // implementation of Social.Active
        PlayGamesPlatform.Activate();

        ((PlayGamesPlatform)Social.Active).SetDefaultLeaderboardForUI(GPGSIds.leaderboard_survival);

        // Sign in to Google Play Games
        authenticating = true;
        Social.localUser.Authenticate((bool success) =>
        {
            authenticating = false;
            if (success)
            {
                Debug.Log("Login successful.");
            }
        });
    }

    public void SignOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
    }

    public void ShowLeaderboards()
    {
        if (authenticated)
        {
            Social.ShowLeaderboardUI();
        }
    }
#endif
}