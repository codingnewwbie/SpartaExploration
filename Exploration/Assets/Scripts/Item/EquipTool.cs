using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipType
{
    Speed,
    Jump,
}

public class EquipTool : MonoBehaviour
{
    [Header("Stat")]
    public float statPoint;
    public EquipType equipType;
    
}