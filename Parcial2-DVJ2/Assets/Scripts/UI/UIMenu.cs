using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    public delegate void OnNewGame();
    public static OnNewGame StartGame;

    public delegate void OnQuitGameAction();
    public static OnQuitGameAction OnQuitGame;

    private GameObject CurrentPanel;
    public GameObject MenuPanel;
    public GameObject SplashPanel;

    private void Start()
    {
        CurrentPanel = MenuPanel;
    }

    public void DeactivateCurrentPanel()
    {
        CurrentPanel.SetActive(false);
    }

    public void ActivatePanel(GameObject panel)
    {
        panel.SetActive(true);
        CurrentPanel = panel;
    }

    public void NewGame()
    {
        if (StartGame != null)
            StartGame();
    }

    public void QuitGame()
    {
        if (OnQuitGame != null)
            OnQuitGame();
    }
}
