using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Consumable
}

public enum ConsumableType
{
    Health,
    Stamina,
    Speed,
    Jump,
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

