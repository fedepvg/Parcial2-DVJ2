using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserData
{
    public string Name;
    public string Surname;
    public string Score;
}

public class UserManager : MonoBehaviourSingleton<UserManager>
{
    public string id;
    string url = "http://localhost/lunarLander/actions.php";
    public UserData userData;

    void Start()
    {
        UIMenu.OnRegister = Register;
        UIMenu.OnLogIn = LogIn;
        UIMenu.OnSaveGameData = SaveGameData;
        UIMenu.OnLoadGameData = LoadGameData;
        id = null;
        userData = new UserData();
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
        if (id != null)
        {
            WWWForm form = new WWWForm();
            form.AddField("action", "save");
            form.AddField("userid", id);
            form.AddField("name", name);
            form.AddField("surname", surname);

            StartCoroutine(GetRequestSave(url, form));
        }
    }

    void LoadGameData()
    {
        //if (id != null)
        //{
            WWWForm form = new WWWForm();
            form.AddField("action", "load");
            form.AddField("userid", id);

            StartCoroutine(GetRequestLoad(url, form));
        //}
    }

    public void SaveScore()
    {
        if (id != null)
        {
            WWWForm form = new WWWForm();
            form.AddField("action", "saveScore");
            form.AddField("userid", id);
            form.AddField("score", GameManager.Instance.Score);

            StartCoroutine(GetRequestSaveScore(url, form));
        }
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

    IEnumerator GetRequestSave(string url, WWWForm form)
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
            Debug.Log(pages[page] + ":\nReceived: " + data[0]);
        }
    }

    IEnumerator GetRequestLoad(string url, WWWForm form)
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
            Debug.Log(pages[page] + ":\nReceived: " + data[3]);
            userData.Name = data[0];
            userData.Surname = data[1];
            userData.Score = data[2];
            Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
        }
    }

    IEnumerator GetRequestSaveScore(string url, WWWForm form)
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
            Debug.Log(pages[page] + ":\nReceived: " + data[0]);
        }
    }
}
