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
    private int correctAnswerIndex;

    void Start()
    {
        // Ensure there's an Event System in the scene
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.AddListener(() => AnswerButtonClicked(index));
        }

        wrongAnswerPopup.SetActive(false); // Ensure the popup is initially hidden
        GenerateQuestion();
        UpdateScore(0);
    }

    void GenerateQuestion()
    {
        wrongAnswerPopup.SetActive(false); // Hide the popup when generating a new question

        int a, b, correctAnswer = 0;
        string opSign = "";

        // Loop until a valid question is generated
        while (true)
        {
            a = Random.Range(1, 101);
            b = Random.Range(1, 101);
            int operation = Random.Range(0, 4); // 0 for addition, 1 for subtraction, 2 for multiplication, 3 for division

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
                        continue; // Skip if the result would be negative
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
                        continue; // Skip if the division is not exact
                    }
                    break;
            }

            // Ensure the correct answer is within the range of 0-100
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
                Debug.Log("Setting correct answer at index " + i + ": " + correctAnswer);
            }
            else
            {
                int wrongAnswer;
                do
                {
                    wrongAnswer = correctAnswer + Random.Range(-10, 11);
                } while (wrongAnswer == correctAnswer || wrongAnswer < 0 || wrongAnswer > 100);

                buttonText.text = wrongAnswer.ToString();
                Debug.Log("Setting wrong answer at index " + i + ": " + wrongAnswer);
            }
        }
    }

    public void AnswerButtonClicked(int buttonIndex)
    {
        Debug.Log("Button clicked: " + buttonIndex);
        Debug.Log("Correct answer index: " + correctAnswerIndex);
        if (buttonIndex == correctAnswerIndex)
        {
            UpdateScore(1);
            Debug.Log("Correct Answer!");
            GenerateQuestion();
        }
        else
        {
            Debug.Log("Wrong Answer!");
            StartCoroutine(ShowWrongAnswerPopup());
        }
    }

    IEnumerator ShowWrongAnswerPopup()
    {
        wrongAnswerPopup.SetActive(true);
        yield return new WaitForSeconds(2); // Show the popup for 2 seconds
        wrongAnswerPopup.SetActive(false);
    }

    void UpdateScore(int change)
    {
        Debug.Log("UpdateScore called with change: " + change);
        score += change;
        Debug.Log("New score: " + score);
        scoreText.text = "Score: " + score.ToString();
    }
}
