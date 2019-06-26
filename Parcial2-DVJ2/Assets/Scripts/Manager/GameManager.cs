using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public int Level;
    public int Score;
    int ScorePerLevel = 1000;
    bool PlayerLost;

    private void Start()
    {
        UIInGameManager.GoToMenu = GoToMenu;
        UIMenu.StartGame = StartNewLevel;
        UIMenu.OnQuitGame = QuitGame;
        UIInGameManager.OnQuitGame = QuitGame;
        UIInGameManager.OnResetStats += InitializeStats;
        UIInGameManager.OnGoToNextLevel += StartNewLevel;
        PlayerController.OnFinishedLevel += AddScore;
        Level = 0;
        Score = 0;
    }

    public void GoToMenu()
    {
        InitializeStats();
        SceneManager.LoadScene("MenuScene");
    }

    public void StartNewLevel()
    {
        Level++;
        SceneManager.LoadScene("LoadingScene");
    }

    void AddScore(PlayerController player)
    {
        if (!player.Dead)
            Score += player.Fuel + ScorePerLevel;
        else
            Score -= player.Fuel;

        if (Score < 0)
            Score = 0;
    }

    void InitializeStats()
    {
        Level = 0;
        Score = 0;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}