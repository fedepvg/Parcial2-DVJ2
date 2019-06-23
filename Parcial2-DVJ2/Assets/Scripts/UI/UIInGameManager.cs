using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameManager : MonoBehaviour
{
    public PlayerController Player;
    public Text Score;
    public Text Fuel;
    public Text Altitude;
    public Text HorizontalSpeed;
    public Text VerticalSpeed;
    int ActualScore;
    int ActualFuel;
    int ActualAltitude;
    int ActualHorizontalSpeed;
    int ActualVerticalSpeed;

    private void Update()
    {
        //if(ScoreManager.Instance.Score != ActualScore)
        //{
        //    ActualScore = ScoreManager.Instance.Score;
        //    Score.text = "Score: " + ActualScore;
        //}
        if(Player.Fuel != ActualFuel)
        {
            ActualFuel=Player.Fuel;
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
}
