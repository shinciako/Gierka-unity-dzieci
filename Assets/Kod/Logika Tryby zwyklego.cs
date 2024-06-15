using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText;
    public Button[] answerButtons;
    public PopupController winPopup;
    public GameObject pop;
    public GameObject buttonMenu;
    public ProgressBar progressBar;
    private string difficultyKey = "SelectedDifficulty";
    private int savedDifficulty;

    private int score = 0;
    private int correctAnswerIndex;
    private int numOfQuestions;
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
        LoadSelectedDifficulty();
        progressBar.max = numOfQuestions;
        InitializeAnswerButtons();
        UpdateScore(0);
        startTime = Time.realtimeSinceStartup;
    }

    void LoadSelectedDifficulty()
    {
        if (PlayerPrefs.HasKey(difficultyKey))
        {
            savedDifficulty = PlayerPrefs.GetInt(difficultyKey);
            switch (savedDifficulty)
            {
                case 0:
                    numOfQuestions = 10;
                    break;
                case 1:
                    numOfQuestions = 15;
                    break;
                case 2:
                    numOfQuestions = 20;
                    break;
                default:
                    numOfQuestions = 10;
                    break;
            }
        }
        else
        {
            numOfQuestions = 10;
        }
    }

    void Update()
    {
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
         foreach (Button button in answerButtons)
        {
            button.interactable = true;
            button.image.enabled = true;
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.color = Color.black;
            buttonText.fontSize = 24;
            buttonText.fontStyle = FontStyles.Normal;

        }
        int correctAnswerQ = 0;
        switch (savedDifficulty)
        {
            case 0:
                correctAnswerQ = GenerateQuestionEasy();
                break;
            case 1:
                correctAnswerQ = GenerateQuestionMid();
                break;
            case 2:
                correctAnswerQ = GenerateQuestionHard();
                break;
            default:
                correctAnswerQ = GenerateQuestionEasy();
                break;
        }
        SetAnswers(correctAnswerQ);
        if (wrongAnswerCoroutine != null)
        {
        StopCoroutine(wrongAnswerCoroutine);
        }
    }

    public int GenerateQuestionEasy()
    {
        int a, b, correctAnswer = 0;
        string opSign = "";
        while (true)
        {
            a = Random.Range(1, 21);
            b = Random.Range(1, 21);
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

            if (correctAnswer >= 0 && correctAnswer <= 50)
            {
                break;
            }
        }

        questionText.text = $"Ile to: {a} {opSign} {b}?";
        return correctAnswer;
    }

    public int GenerateQuestionMid()
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



    public int GenerateQuestionHard()
    {
        int correctAnswer = 0;
        int a = 0, b = 0, c = 0, d = 0;
        List<(string, int)> validOperations = new List<(string, int)>();

        while (validOperations.Count == 0)
        {
            a = Random.Range(1, 51);
            b = Random.Range(1, 51);
            c = Random.Range(1, 51);
            d = Random.Range(1, 51);

            List<(string, int)> operations = new List<(string, int)>
            {
                ("+++", a + b + c + d),
                ("++-", a + b + c - d),
                ("+-+-", a + b - c - d),
                ("+*-", a + b * c - d),
                ("--+", a - b - c + d),
                ("---", a - b - c - d),
                ("-*-", a - b * c - d),
                ("*++", a * b + c + d),
                ("*+-", a * b + c - d),
                ("*-+", a * b - c + d),
                ("*--", a * b - c - d)
            };

            List<(string, int)> verifiedOperations = new List<(string, int)>();

            if (b > 0 && a % b == 0)
            {
                int intermediateResult = a / b;
                verifiedOperations.Add(("/++", intermediateResult + c + d));
                verifiedOperations.Add(("/+-", intermediateResult + c - d));
                verifiedOperations.Add(("/-+", intermediateResult - c + d));
                verifiedOperations.Add(("/--", intermediateResult - c - d));
                if (c > 0 && intermediateResult % c == 0)
                {
                    intermediateResult = intermediateResult / c;
                    verifiedOperations.Add(("//+", intermediateResult + d));
                    verifiedOperations.Add(("//-", intermediateResult - d));
                    if (d > 0 && intermediateResult % d == 0)
                    {
                        intermediateResult = intermediateResult / d;
                        verifiedOperations.Add(("///", intermediateResult));
                    }
                }
            }
            if (c > 0 && b % c == 0)
            {
                int intermediateResult = b / c;
                verifiedOperations.Add(("+/+", a + intermediateResult + d));
                verifiedOperations.Add(("+/-", a + intermediateResult - d));
                verifiedOperations.Add(("-/+", a - intermediateResult + d));
                verifiedOperations.Add(("-/-", a - intermediateResult - d));
                if (d > 0 && intermediateResult % d == 0)
                {
                    intermediateResult = intermediateResult / d;
                    verifiedOperations.Add(("+//", a + intermediateResult));
                    verifiedOperations.Add(("-//", a - intermediateResult));
                }
            }
            if (d > 0 && c % d == 0)
            {
                int intermediateResult = c / d;
                verifiedOperations.Add(("+/+", a + b + intermediateResult));
                verifiedOperations.Add(("+/-", a + b - intermediateResult));
                verifiedOperations.Add(("-/+", a - b + intermediateResult));
                verifiedOperations.Add(("-/-", a - b - intermediateResult));
            }

            verifiedOperations.AddRange(operations);
            validOperations = verifiedOperations.Where(op => op.Item2 >= 0 && op.Item2 <= 200).ToList();
        }

        int randomIndex = Random.Range(0, validOperations.Count);
        var operation = validOperations[randomIndex];
        string opSign = operation.Item1;
        correctAnswer = operation.Item2;

        string question = $"Ile to: {a} {opSign[0]} {b} {opSign[1]} {c}";
        if (opSign.Length > 2)
        {
            question += $" {opSign[2]} {d}?";
        }
        else
        {
            question += $" {d}?";
        }

        questionText.text = question;
        return correctAnswer;
    }

    private Coroutine wrongAnswerCoroutine;

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
        if (wrongAnswerCoroutine != null)
        {
            StopCoroutine(wrongAnswerCoroutine);
        }
    }
    else
    {
        if (wrongAnswerCoroutine != null)
        {
            StopCoroutine(wrongAnswerCoroutine);
        }
        wrongAnswerCoroutine = StartCoroutine(ShowWrongAnswerPopup(buttonIndex));
    }
}

    IEnumerator ShowWrongAnswerPopup(int index)
    {
    answerButtons[index].interactable = false;
    answerButtons[index].image.enabled = false;
    
    TextMeshProUGUI childText = answerButtons[index].GetComponentInChildren<TextMeshProUGUI>();
    
    childText.text = "Niepoprawna odpowiedź";
    childText.color = Color.red;
    childText.fontSize = 30; 
    childText.fontStyle = FontStyles.Bold;

    yield return new WaitForSeconds(1);

    childText.text = "";
    childText.color = Color.white; 
    childText.fontSize = 20; 
    childText.fontStyle = FontStyles.Normal; 
}

    void UpdateScore(int change)
    {
        score += change;
        if (score == numOfQuestions)
        {
            buttonMenu.SetActive(false);
            foreach (Button button in answerButtons)
            {
                button.interactable = false;
            }
            int accurate = CalculatePercentage();
            winPopup.SetNumbers(accurate, elapsedTime);
            pop.SetActive(true);
        }
        else
        {
            scoreText.text = "Pytanie: " + (1 + score) + " z " + numOfQuestions;
            GenerateQuestion();
        }
    }

    int CalculatePercentage()
    {
        float percentage = ((float)numOfQuestions / (float)tries) * 100.0f;
        int roundedPercentage = Mathf.CeilToInt(percentage);
        return roundedPercentage;
    }

    void UpdateProgressBar()
    {
        progressBar.current = score;
    }
}
