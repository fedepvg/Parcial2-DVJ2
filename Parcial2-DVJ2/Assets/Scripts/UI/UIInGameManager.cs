using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameManager : MonoBehaviour
{
    public delegate void OnGoToMenu();
    public static OnGoToMenu GoToMenu;

    public delegate void ResetStatsAction();
    public static ResetStatsAction OnResetStats;

    public delegate void GoToNextLevelAction();
    public static GoToNextLevelAction OnGoToNextLevel;

    public delegate void OnQuitGameAction();
    public static OnQuitGameAction OnQuitGame;

    public PlayerController Player;
    public Text Score;
    public Text Fuel;
    public Text Altitude;
    public Text HorizontalSpeed;
    public Text VerticalSpeed;
    public Button PauseButton;
    public GameObject PausePanel;
    public GameObject LandingPanel;
    public GameObject ContinueButton;
    public GameObject RestartButton;
    public Text Result;
    public Text ResultScore;

    int ActualScore;
    int ActualFuel;
    int ActualAltitude;
    int ActualHorizontalSpeed;
    int ActualVerticalSpeed;

    private void Start()
    {
        PlayerController.OnFinishedLevel += ShowLandingScreen;
    }

    private void Update()
    {
        if(GameManager.Instance.Score != ActualScore)
        {
            ActualScore = GameManager.Instance.Score;
            Score.text = "Score: " + ActualScore;
            ResultScore.text = Score.text;
        }
        if (Player.Fuel != ActualFuel)
        {
            ActualFuel = Player.Fuel;
            Fuel.text = "Fuel: " + ActualFuel;
        }
        if (Player.Altitude != ActualAltitude)
        {
            ActualAltitude = (int)Player.Altitude;
            Altitude.text = "Altitude: " + ActualAltitude;
        }
        if (Player.VerticalSpeed != ActualVerticalSpeed)
        {
            ActualVerticalSpeed = (int)Player.VerticalSpeed;
            VerticalSpeed.text = "Vertical Speed: " + ActualVerticalSpeed;
        }
        if (Player.HorizontalSpeed != ActualHorizontalSpeed)
        {
            ActualHorizontalSpeed = (int)Player.HorizontalSpeed;
            HorizontalSpeed.text = "Horizontal Speed: " + ActualHorizontalSpeed;
        }
    }

    public void ActivatePause()
    {
        Time.timeScale = 0;
        PauseButton.gameObject.SetActive(false);
        PausePanel.SetActive(true);
    }

    public void DeactivatePause()
    {
        PausePanel.SetActive(false);
        PauseButton.gameObject.SetActive(true);
        Time.timeScale = 1;
    }

    void ShowLandingScreen(PlayerController player)
    {
        LandingPanel.SetActive(true);
        if (!player.Dead)
        {
            RestartButton.SetActive(false);
            ContinueButton.SetActive(true);
            Result.text = "You Won";
        }
        else
        {
            ContinueButton.SetActive(false);
            RestartButton.SetActive(true);
            Result.text = "Game Over";
        }
        ResultScore.text = Score.text;
    }

    public void GoToNextLevel()
    {
        if (OnGoToNextLevel != null)
            OnGoToNextLevel();
    }

    public void ResetStats()
    {
        if (OnResetStats != null)
            OnResetStats();
    }

    public void ReturnToMenu()
    {
        if (GoToMenu != null)
            GoToMenu();
    }

    public void QuitGame()
    {
        if (OnQuitGame != null)
            OnQuitGame();
    }

    private void OnDestroy()
    {
        PlayerController.OnFinishedLevel -= ShowLandingScreen;
    }
}
