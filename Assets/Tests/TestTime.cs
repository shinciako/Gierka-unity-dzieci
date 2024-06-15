using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestTime
{
    private GameObject popupControllerObject;
    private PopupController popupController;

    [SetUp]
    public void Setup()
    {
        popupControllerObject = new GameObject();
        popupController = popupControllerObject.AddComponent<PopupController>();
    }

    [Test]
    public void TestConverseTime_SecondsOnly()
    {
        float timeEx = 45f;

        string result = CallConverseTime(timeEx);

        Assert.AreEqual("Czas: 45 sekund", result);
    }

    [Test]
    public void TestConverseTime_MinutesAndSeconds()
    {
        float timeEx = 123.45f;

        string result = CallConverseTime(timeEx);

        Assert.AreEqual("Czas: 2 minut 03 sekund", result);
    }

    private string CallConverseTime(float timeEx)
    {
        return (string)typeof(PopupController)
            .GetMethod("ConverseTime", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(popupController, new object[] { timeEx });
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(popupControllerObject);
    }
}
