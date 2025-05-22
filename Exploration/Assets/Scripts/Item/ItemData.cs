using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Consumable,
    Equip,
}

public enum ConsumableType
{
    Health,
    Speed,
    Jump,
    SJump,
    SHealth,
}
[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType ConsumableType;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")] //
    public bool canStack;
    public int maxStackAmount;
    
    [Header("Consumable")]
    public ItemDataConsumable[] Consumables;
    
    [Header("Equip")]
    public GameObject equipPrefab;
    
}

