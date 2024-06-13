using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText;
    public Button[] answerButtons;
    public GameObject wrongAnswerPopup;
    public PopupController winPopup;
    public GameObject pop;
    public GameObject buttonMenu;
    public ProgressBar progressBar;

    private int score = 0;
    private int correctAnswerIndex;
    private int numOfQuestions=2;
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
        progressBar.max = numOfQuestions;
        InitializeAnswerButtons();
        wrongAnswerPopup.SetActive(false);
        GenerateQuestion();
        UpdateScore(0);
        startTime = Time.realtimeSinceStartup;
    }

    void Update(){
        elapsedTime = Time.realtimeSinceStartup - startTime;
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
        tries++;
        if (buttonIndex == correctAnswerIndex)
        {
            UpdateScore(1);
            UpdateProgressBar();
        }
        else
        {
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
        if(score==numOfQuestions){
            buttonMenu.SetActive(false);
            int accurate = CalculatePercentage();
            winPopup.SetNumbers(accurate,elapsedTime);
            pop.SetActive(true);
        }else{
            scoreText.text = "Pytanie: " + (1+score).ToString() +" z "+numOfQuestions;
            GenerateQuestion();
        }
    }

    int CalculatePercentage(){
    float percentage = ((float)numOfQuestions / (float)tries) * 100.0f;
    int roundedPercentage = Mathf.CeilToInt(percentage);
    return roundedPercentage;
}

    void UpdateProgressBar()
    {
        progressBar.current = score;
    }
}
