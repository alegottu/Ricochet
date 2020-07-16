using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
using UnityEngine.Rendering;

public class GooglePlayController : Singleton<GooglePlayController>
{
    private void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
    }

    public static void UploadScore(int score, GameManager.GameMode mode)
    {
        string ID = mode == GameManager.GameMode.SURVIVAL ? GPGSIds.leaderboard_survival : mode == GameManager.GameMode.ARCADE ? GPGSIds.leaderboard_arcade : mode == GameManager.GameMode.HARDCORE ? GPGSIds.leaderboard_hardcore : GPGSIds.leaderboard_build;
        Social.ReportScore(score, ID, (bool success) =>
        {
            Debug.Log(success ? "Reported score successfully" : "Failed to report score");
        });
    }

    public static void UploadScore(float time, GameManager.GameMode mode)
    {
        string ID = mode == GameManager.GameMode.SURVIVAL ? GPGSIds.leaderboard_survival : mode == GameManager.GameMode.ARCADE ? GPGSIds.leaderboard_arcade : mode == GameManager.GameMode.HARDCORE ? GPGSIds.leaderboard_hardcore : GPGSIds.leaderboard_build;
        Social.ReportScore((long)time, ID, (bool success) =>
        {
            Debug.Log(success ? "Reported score successfully" : "Failed to report score");
        });
    }

    public static void Authenticate()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                Debug.Log(success ? "User authenticated" : "User failed to authenticate");
            });
        }
    }
}
