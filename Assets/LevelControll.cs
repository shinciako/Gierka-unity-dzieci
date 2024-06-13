using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelControll : MonoBehaviour
{
    public GameObject ezBall;
    public GameObject midBall;
    public GameObject hardBall;

    public Color ezSelectedColor = Color.green;
    public Color midSelectedColor = Color.yellow;
    public Color hardSelectedColor = new Color(1.0f, 0.6f, 0.66f);
    private Color defaultColor = Color.white;
     private string difficultyKey;

    void Start()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
        InitButtons();
        LoadSelectedDifficulty();
    }

    void InitButtons()
    {
        SetButtonColor(ezBall, defaultColor);
        SetButtonColor(midBall, defaultColor);
        SetButtonColor(hardBall, defaultColor);

        AddClickListener(ezBall, SelectEasy);
        AddClickListener(midBall, SelectMedium);
        AddClickListener(hardBall, SelectHard);
    }

    void AddClickListener(GameObject obj, UnityEngine.Events.UnityAction action)
    {
        Button button = obj.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(action);
        }
    }

    void LoadSelectedDifficulty()
    {
        if (PlayerPrefs.HasKey(difficultyKey))
        {
            int savedDifficulty = PlayerPrefs.GetInt(difficultyKey);
            switch (savedDifficulty)
            {
                case 0:
                    SelectEasy();
                    break;
                case 1:
                    SelectMedium();
                    break;
                case 2:
                    SelectHard();
                    break;
                default:
                    break;
            }
        }
        else
        {
            SelectEasy();
        }
    }

    public void SelectEasy()
    {
        SetButtonColor(ezBall, ezSelectedColor);
        SetButtonColor(midBall, defaultColor);
        SetButtonColor(hardBall, defaultColor);
        SaveSelectedDifficulty(0);
    }

    public void SelectMedium()
    {
        SetButtonColor(ezBall, defaultColor);
        SetButtonColor(midBall, midSelectedColor);
        SetButtonColor(hardBall, defaultColor);
        SaveSelectedDifficulty(1);
    }

    public void SelectHard()
    {
        SetButtonColor(ezBall, defaultColor);
        SetButtonColor(midBall, defaultColor);
        SetButtonColor(hardBall, hardSelectedColor);
        SaveSelectedDifficulty(2);
    }

    void SaveSelectedDifficulty(int difficulty)
    {
        PlayerPrefs.SetInt(difficultyKey, difficulty);
        PlayerPrefs.Save();
    }

    void SetButtonColor(GameObject button, Color color)
    {
        if (button != null)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = color;
            }
        }
    }
}