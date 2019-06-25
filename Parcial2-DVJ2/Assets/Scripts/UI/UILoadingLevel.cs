using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingLevel : MonoBehaviour
{
    public Slider LoadingSlider;
    public Text LevelText;

    void Start()
    {
        LoaderManager.Instance.LoadScene("Level1");
        LevelText.text = "Loading Level " + GameManager.Instance.Level;
    }
    
    void Update()
    {
        LoadingSlider.value = LoaderManager.Instance.loadingProgress;
    }
}
