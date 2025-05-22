using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public LayerMask pLayerMask;

    private void Update()
    {
        Debug.DrawRay(transform.position + Vector3.up * 0.5f, Vector3.left * 3.75f, Color.red);
        
        if (IsApproached())
        {
            CharacterManager.Instance.Player.playerCondition.TakeDamage(10f);
        }
    }


    private bool IsApproached()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, Vector3.left);
        if (Physics.Raycast(ray, 3.75f, pLayerMask))
        {
            return true;
        }
        return false;
    }
}


