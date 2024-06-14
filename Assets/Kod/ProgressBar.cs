using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    public int max;
    public int current;
    public Image fill;

    void Update()
    {
        GetCurrentFill();
    }

    void GetCurrentFill(){
        fill.fillAmount = (float)current/(float)max;
    }
}
