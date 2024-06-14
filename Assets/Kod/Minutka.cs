using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;

public class Tryby : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public Button[] answerButtons;
    public GameObject wrongAnswerPopup;
    public PopupController winPopup;
    public GameObject pop;
    public GameObject buttonMenu;
    public ProgressBar progressBar;

    private int score = 0;
    private int correctAnswerIndex;
    private int tries = 0;

    private float startTime;
    private float elapsedTime;

    void Start()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
        progressBar.max = 60;
        progressBar.current = 60;
        InitializeAnswerButtons();
        UpdateScore(0);
        startTime = Time.realtimeSinceStartup;
    }


    void Update()
    {
        elapsedTime = Time.realtimeSinceStartup - startTime;
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        if (elapsedTime >=60)
        {
            foreach (Button button in answerButtons)
            {
                button.interactable = false;
            }
            progressBar.current = 0;
            int accurate = CalculatePercentage();
            winPopup.SetNumbersM(accurate, score);
            buttonMenu.SetActive(false);
            scoreText.text = "";
            timeText.text = "";
            pop.SetActive(true);
        } else{
            timeText.text = 60-seconds+"s";
            progressBar.current = 60 - seconds;
        }
        
    }


    void InitializeAnswerButtons()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.AddListener(() => AnswerButtonClicked(index));
        }
    }

    void GenerateQuestion()
    {
        wrongAnswerPopup.SetActive(false);
        int correctAnswerQ = GenerateQuestionEasy();
        SetAnswers(correctAnswerQ);
    }

    int GenerateQuestionEasy()
    {
        int a, b, correctAnswer = 0;
        string opSign = "";
        while (true)
        {
            a = Random.Range(1, 51);
            b = Random.Range(1, 51);
            int operation = Random.Range(0, 4);
            switch (operation)
            {
                case 0:
                    correctAnswer = a + b;
                    opSign = "+";
                    break;
                case 1:
                    if (a >= b)
                    {
                        correctAnswer = a - b;
                        opSign = "-";
                    }
                    else
                    {
                        continue;
                    }
                    break;
                case 2:
                    correctAnswer = a * b;
                    opSign = "*";
                    break;
                case 3:
                    if (a % b == 0)
                    {
                        correctAnswer = a / b;
                        opSign = "/";
                    }
                    else
                    {
                        continue;
                    }
                    break;
            }

            if (correctAnswer >= 0 && correctAnswer <= 100)
            {
                break;
            }
        }

        questionText.text = $"Ile to: {a} {opSign} {b}?";
        return correctAnswer;
    }

    void SetAnswers(int correctAnswer)
    {
        correctAnswerIndex = Random.Range(0, answerButtons.Length);
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            if (i == correctAnswerIndex)
            {
                buttonText.text = correctAnswer.ToString();
            }
            else
            {
                int wrongAnswer;
                do
                {
                    wrongAnswer = correctAnswer + Random.Range(-10, 11);
                } while (wrongAnswer == correctAnswer || wrongAnswer < 0 || wrongAnswer > 100);

                buttonText.text = wrongAnswer.ToString();
            }
        }
    }

    public void AnswerButtonClicked(int buttonIndex)
    {
        tries++;
        if (buttonIndex == correctAnswerIndex)
        {
            UpdateScore(1);
        }
    }

    void UpdateScore(int change)
    {
        score += change;
        scoreText.text = "Wynik: "+score;
        GenerateQuestion();
    }

    int CalculatePercentage()
    {
        float percentage = ((float)score / (float)tries) * 100.0f; // Fixed typo: numOfQuestions to score
        int roundedPercentage = Mathf.CeilToInt(percentage);
        return roundedPercentage;
    }

    void UpdateProgressBar()
    {
        progressBar.current = score;
    }
}
