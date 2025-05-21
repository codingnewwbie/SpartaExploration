using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public PlayerController playerController;
    [HideInInspector]
    public PlayerCondition playerCondition;
    public Transform dropPosition;
    public ItemData itemData;
    public Action AddItem;
    
    private void Awake()
    {
        CharacterManager.Instance.Player = this; // 외부에서 플레이어 접근 시 CharacterManager를 통해 접근 가능하도록
        playerController = GetComponent<PlayerController>();
        playerCondition = GetComponent<PlayerCondition>();
    }
}
