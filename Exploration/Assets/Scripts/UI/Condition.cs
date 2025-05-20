using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float currentValue;
    public float startValue;
    public float maxValue;
    public float passiveValue;
    public Image uiBar;

    void Start()
    {
        currentValue = startValue;
    }

    void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    float GetPercentage()
    {
        return currentValue / maxValue;
    }

    public void Add(float value)
    {
        currentValue = Math.Min(currentValue + value, maxValue);
    }

    public void Subtract(float value)
    {
        currentValue = Math.Max(currentValue - value, 0);
    }
}
