using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserManager : MonoBehaviourSingleton<UserManager>
{
    string id;
    string url = "http://localhost/lunarLander/actions.php";

    void Start()
    {
        UIMenu.OnRegister = Register;
        UIMenu.OnLogIn = LogIn;
        UIMenu.OnSaveGameData = SaveGameData;
        UIMenu.OnLoadGameData = LoadGameData;
    }

    void Register(string user, string pass)
    {
        WWWForm form = new WWWForm();
        form.AddField("action", "register");
        form.AddField("user", user);
        form.AddField("pass", pass);

        StartCoroutine(GetRequestRegister(url, form));
    }

    void LogIn(string user, string pass)
    {
        WWWForm form = new WWWForm();
        form.AddField("action", "login");
        form.AddField("user", user);
        form.AddField("pass", pass);

        StartCoroutine(GetRequestLogIn(url, form));
    }

    void SaveGameData(string name, string surname)
    {

    }

    void LoadGameData()
    {

    }

    IEnumerator GetRequestRegister(string url, WWWForm form)
    {
        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        string[] pages = url.Split('/');
        int page = pages.Length - 1;

        if (webRequest.isNetworkError)
        {
            Debug.Log(pages[page] + ": Error: " + webRequest.error);
        }
        else
        {
            string []data = webRequest.downloadHandler.text.Split('=');
            Debug.Log(pages[page] + ":\nReceived: " + data[0]);
        }
    }

    IEnumerator GetRequestLogIn(string url, WWWForm form)
    {
        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        string[] pages = url.Split('/');
        int page = pages.Length - 1;

        if (webRequest.isNetworkError)
        {
            Debug.Log(pages[page] + ": Error: " + webRequest.error);
        }
        else
        {
            string[] data = webRequest.downloadHandler.text.Split('=');
            Debug.Log(pages[page] + ":\nReceived: " + data[1]);
            id = data[0];
        }
    }
}
