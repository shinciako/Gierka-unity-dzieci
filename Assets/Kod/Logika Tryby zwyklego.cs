using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText;
    public Button[] answerButtons;
    public GameObject wrongAnswerPopup;

    private int score = 0;
    private int clicks = 0;
    private int correctAnswerIndex;

    void Start()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
        InitializeAnswerButtons();
        wrongAnswerPopup.SetActive(false);
        GenerateQuestion();
        UpdateScore(0);
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
        int a, b, correctAnswer = 0;
        string opSign = "";
        while (true)
        {
            a = Random.Range(1, 51);
            b = Random.Range(1, 51);
            int operation = Random.Range(0, 4); // 0 +, 1 -, 2 *, 3 /
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
                        continue; // skip if negative
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
                        continue; //skip not exact
                    }
                    break;
            }

            // score 0-100
            if (correctAnswer >= 0 && correctAnswer <= 100)
            {
                break;
            }
        }

        questionText.text = $"Ile to: {a} {opSign} {b}?";
        SetAnswers(correctAnswer);
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
        if (buttonIndex == correctAnswerIndex)
        {
            UpdateScore(1);
            GenerateQuestion();
            Debug.Log("bajlando");
        }
        else
        {
            Debug.Log("not bajlando");
            StartCoroutine(ShowWrongAnswerPopup());
        }
    }

    IEnumerator ShowWrongAnswerPopup()
    {
        wrongAnswerPopup.SetActive(true);
        yield return new WaitForSeconds(2);
        wrongAnswerPopup.SetActive(false);
    }

    void UpdateScore(int change)
    {
        score += change;
        scoreText.text = "Score: " + score.ToString();
    }
}
