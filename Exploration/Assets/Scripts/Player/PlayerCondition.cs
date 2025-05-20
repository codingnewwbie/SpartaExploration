using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    private float originalSpeed;
    public UICondition uiCondition;
    
    Condition health 
    {
        get { return uiCondition.health; }
    }
    
    Condition stamina 
    {
        get { return uiCondition.stamina; }
    }

    private bool isDied = false;  
    
    void Update()
    {
        health.Add(health.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (health.currentValue == 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Die()
    {
        if (!isDied)
        {
            isDied = true;
            Debug.Log("체력 0! 사망!");
        }
    }
    
    public bool UseStamina(float amount)
    {
        if (stamina.currentValue - amount < 0f) return false;
        
        stamina.Subtract(amount);
        return true;
    }

    public void SpeedUp(float amount)
    {
        originalSpeed = CharacterManager.Instance.Player.playerController.moveSpeed;
        CharacterManager.Instance.Player.playerController.moveSpeed += amount;
        Invoke("ResetSpeed", 10f);
    }
    
    

    private void ResetSpeed()
    {
        CharacterManager.Instance.Player.playerController.moveSpeed = originalSpeed;
    }
}
