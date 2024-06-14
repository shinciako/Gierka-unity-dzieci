using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupController : MonoBehaviour
{
    public TextMeshProUGUI accurate;
    public TextMeshProUGUI time;

    public void SetNumbers(int percent, float timeEx){
        accurate.text = "Skuteczność: " +percent+"%";
        time.text = ConverseTime(timeEx);
    }

    public void SetNumbersM(int percent, int wynik){
        accurate.text = "Skuteczność: " +percent+"%";
        time.text = "Wynik: "+wynik;
    }

    public void SetScore(int wynik){
        accurate.text = "Udało Ci się: "+ wynik +" podrząd!";
    }

    private string ConverseTime(float timeEx)
    {
        int minutes = Mathf.FloorToInt(timeEx / 60);
        int seconds = Mathf.FloorToInt(timeEx % 60);

        if (minutes > 0)
        {
            if (seconds < 10)
            {
                return string.Format("Czas: {0} minut {1:00} sekund", minutes, seconds);
            }
            else
            {
                return string.Format("Czas: {0} minut {1} sekund", minutes, seconds);
            }
        }
        else
        {
            return string.Format("Czas: {0} sekund", seconds);
        }
    }

}
