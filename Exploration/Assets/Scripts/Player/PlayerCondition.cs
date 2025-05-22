using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage);
} 
public class PlayerCondition : MonoBehaviour, IDamageable
{
    private float originalSpeed;
    private float originalJump;
    public UICondition uiCondition;
    
    private Coroutine speedCoroutine;
    private Coroutine jumpCoroutine;
    private Coroutine doubleJumpCoroutine;
    
    public event Action onTakeDamage;

    private float ImmuneTime = 1.5f;
    private bool isImmuneDamage = false;
    private bool isItemImmuneDamage = false;
    
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

    // public void SpeedUp(float amount)
    // {
    //     originalSpeed = CharacterManager.Instance.Player.playerController.moveSpeed;
    //     CharacterManager.Instance.Player.playerController.moveSpeed += amount;
    //     Invoke("ResetSpeed", 10f);
    // }
    //
    // public void JumpUp(float amount)
    // {
    //     originalJump = CharacterManager.Instance.Player.playerController.jumpPower;
    //     CharacterManager.Instance.Player.playerController.jumpPower += amount;
    //     Invoke("ResetJump", 10f);
    // }
    //
    // private void ResetSpeed()
    // {
    //     CharacterManager.Instance.Player.playerController.moveSpeed = originalSpeed;
    // }
    //
    // private void ResetJump()
    // {
    //     CharacterManager.Instance.Player.playerController.jumpPower = originalJump;
    // }
    
    // 코루틴 사용한 아이템 효과 구현
    public void IncreaseSpeed(float amount, float duration)
    {
        // speedItem 사용중이면 이전 효과 멈추고
        if (speedCoroutine != null)
        {
            CharacterManager.Instance.Player.playerController.moveSpeed = originalSpeed;
            StopCoroutine(speedCoroutine);
        }
        
        // 코루틴을 실행시키고 return 값을 변수에 넣어주기.
        speedCoroutine = StartCoroutine(SpeedUpCoroutine(amount, duration));
    }

    private IEnumerator SpeedUpCoroutine(float amount, float duration)
    {
        originalSpeed = CharacterManager.Instance.Player.playerController.moveSpeed;
        CharacterManager.Instance.Player.playerController.moveSpeed += amount;

        yield return new WaitForSeconds(duration);

        CharacterManager.Instance.Player.playerController.moveSpeed = originalSpeed;
        speedCoroutine = null;
    }
    
    public void IncreaseJump(float amount, float duration)
    {
        if (jumpCoroutine != null)
        {
            CharacterManager.Instance.Player.playerController.jumpPower = originalJump;
            StopCoroutine(jumpCoroutine);
        }
        
        jumpCoroutine = StartCoroutine(JumpUpCoroutine(amount, duration));
    }

    private IEnumerator JumpUpCoroutine(float amount, float duration)
    {
        originalJump = CharacterManager.Instance.Player.playerController.jumpPower;
        CharacterManager.Instance.Player.playerController.jumpPower += amount;

        yield return new WaitForSeconds(duration);

        CharacterManager.Instance.Player.playerController.jumpPower = originalJump;
        jumpCoroutine = null;
    }

    
    public void DoubleJump(float duration)
    {
        if (doubleJumpCoroutine != null)
        {
            CharacterManager.Instance.Player.playerController.isDoubleJump = false;
            StopCoroutine(doubleJumpCoroutine);
        }
        
        doubleJumpCoroutine = StartCoroutine(DoubleJumpCoroutine(duration));
    }

    private IEnumerator DoubleJumpCoroutine(float duration)
    {
        CharacterManager.Instance.Player.playerController.isDoubleJump = true;
        
        yield return new WaitForSeconds(duration);

        CharacterManager.Instance.Player.playerController.isDoubleJump = false;
        doubleJumpCoroutine = null;
    }
    
        
    public void TakeDamage(float damage)
    {
        if (isImmuneDamage) return;

        ImmuneTime = isItemImmuneDamage ? 10f : 1.5f;
        health.Subtract(damage);
        onTakeDamage?.Invoke();
        StartCoroutine(ImmuneDamage(ImmuneTime));
        
    }

    private IEnumerator ImmuneDamage(float duration)
    {
        isImmuneDamage = true;
        yield return new WaitForSeconds(duration);
        isImmuneDamage = false;
        if(isItemImmuneDamage) isItemImmuneDamage = false;
    }

    public void InvincibleTime(float duration)
    {
        isItemImmuneDamage = true;
        StartCoroutine(ImmuneDamage(duration));
    }
}
