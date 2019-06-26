using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreensManager : MonoBehaviour
{
    public GameObject[] Screens;
    int ScreenIndex;
    
    void Start()
    {
        ScreenIndex = 0;
        FadeScreen();
    }

    void FadeScreen()
    {
        Screens[ScreenIndex].SetActive(true);
        StartCoroutine(Fade(Screens[ScreenIndex]));
    }

    void LoadMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    IEnumerator Fade(GameObject g)
    {
        yield return new WaitForSeconds(1);
        float t = 1;
        Material mat = g.GetComponent<Image>().material;
        Color color = mat.color;

        while (t > 0)
        {
            t -= Time.deltaTime;
            color.a = t;
            mat.color = color;
            if (t <= 0)
            {
                g.SetActive(false);
                if (++ScreenIndex < Screens.Length)
                {
                    FadeScreen();
                }
                else
                    LoadMenu();
                color.a = 1;
                mat.color = color;
            }
            yield return null;
        }
    }
}
