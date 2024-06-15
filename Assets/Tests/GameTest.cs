using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using TMPro;

public class GameTests
{
    private GameController gameController;

    [SetUp]
    public void Setup()
    {
        GameObject gameControllerObject = new GameObject();
        gameController = gameControllerObject.AddComponent<GameController>();
        if (gameControllerObject.GetComponent<TextMeshProUGUI>() == null)
        {
            gameController.questionText = gameControllerObject.AddComponent<TextMeshProUGUI>();
            gameController.scoreText = gameControllerObject.AddComponent<TextMeshProUGUI>();
        }
        gameController.answerButtons = new UnityEngine.UI.Button[4];
        for (int i = 0; i < 4; i++)
        {
            GameObject buttonObject = new GameObject();
            gameController.answerButtons[i] = buttonObject.AddComponent<UnityEngine.UI.Button>();
        }
        gameController.winPopup = gameControllerObject.AddComponent<PopupController>();
        gameController.pop = new GameObject();
        gameController.buttonMenu = new GameObject();
        gameController.progressBar = gameControllerObject.AddComponent<ProgressBar>();
    }

    [Test]
    public void TestGenerateQuestionEasy()
    {
        int correctAnswer = gameController.GenerateQuestionEasy();
        Assert.IsTrue(gameController.questionText.text.Contains("Ile to:"));
        Assert.GreaterOrEqual(correctAnswer, 0);
        Assert.LessOrEqual(correctAnswer, 50);
    }

    [Test]
    public void TestGenerateQuestionMid()
    {
        int correctAnswer = gameController.GenerateQuestionMid();
        Assert.IsTrue(gameController.questionText.text.Contains("Ile to:"));
        Assert.GreaterOrEqual(correctAnswer, 0);
        Assert.LessOrEqual(correctAnswer, 100);
    }

    [Test]
    public void TestGenerateQuestionHard()
    {
        int correctAnswer = gameController.GenerateQuestionHard();
        Assert.IsTrue(gameController.questionText.text.Contains("Ile to:"));
        Assert.GreaterOrEqual(correctAnswer, 0);
        Assert.LessOrEqual(correctAnswer, 200);
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(gameController.gameObject);
    }
}
