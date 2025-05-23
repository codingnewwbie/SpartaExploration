using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public EquipTool currentEquip;

    public Transform equipParent;
    
    private PlayerCondition playerCondition;
    private PlayerController playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        playerCondition = GetComponent<PlayerCondition>();
        playerController = GetComponent<PlayerController>();
    }

    public void EquipNew(ItemData data)
    {
        //기존 장비 해제
        UnEquip();
        currentEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<EquipTool>();

        switch (currentEquip.equipType) 
        {
            case EquipType.Jump:
                playerCondition.EquipJumpItem(currentEquip.statPoint);
                break;
            case EquipType.Speed:
                playerCondition.EquipSpeedItem(currentEquip.statPoint);
                break;
        }
    }

    public void UnEquip()
    {
        if (currentEquip != null)
        {
            Destroy(currentEquip.gameObject);
            currentEquip = null;
        }
    }
}

