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

    public delegate void OnRegisterAction(string user, string pass);
    public static OnRegisterAction OnRegister;

    public delegate void OnLogInAction(string user, string pass);
    public static OnLogInAction OnLogIn;

    public delegate void OnLoadGameDataAction();
    public static OnLoadGameDataAction OnLoadGameData;

    public delegate void OnSaveGameDataAction(string name, string surname);
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

    public Text Result;

    private void Start()
    {
        CurrentPanel = MenuPanel;
    }

    private void Update()
    {

        UserManager um = UserManager.Instance;

        if (um.id != null)
        {
            NameText.text = um.userData.Name;
            SurnameText.text = um.userData.Surname;
            Score.text = um.userData.Score;
        }
        else
        {
            NameText.text = "";
            SurnameText.text = "";
            Score.text = "0";
        }
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

    public void RegisterUser()
    {
        if (UserRegister.text != "" && PassRegister.text != "")
        {
            if (OnRegister != null)
                OnRegister(UserRegister.text, PassRegister.text);
        }
        else
        {
            Result.gameObject.SetActive(true);
        }
        PassRegister.text = "";
    }

    public void LogInUser()
    {
        if (OnLogIn != null)
            OnLogIn(UserLogIn.text, PassLogIn.text);
        PassLogIn.text = "";
    }

    public void LoadGameData()
    {
        if (OnLoadGameData != null)
            OnLoadGameData();

        NameInput.text = "";
        SurnameInput.text = "";
    }

    public void SaveGameData()
    {
        if (OnSaveGameData != null)
            OnSaveGameData(NameInput.text, SurnameInput.text);
    }
}
