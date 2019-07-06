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

    public delegate void OnRegisterAction();
    public static OnRegisterAction OnRegister;

    public delegate void OnLogInAction();
    public static OnLogInAction OnLogIn;

    public delegate void OnLoadGameDataAction();
    public static OnLoadGameDataAction OnLoadGameData;

    public delegate void OnSaveGameDataAction();
    public static OnSaveGameDataAction OnSaveGameData;

    private GameObject CurrentPanel;
    public GameObject MenuPanel;

    public InputField UserRegister;
    public InputField PassRegister;
    public InputField UserLogIn;
    public InputField PassLogIn;
    public InputField NameInput;
    public InputField SurnameInput;
    public Text NameText;
    public Text SurnameText;
    public Text Score;

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
