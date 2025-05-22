using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;
    public GameObject inventoryWindow;
    public Transform slotPanel;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStat;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;
    
    private PlayerController controller;
    private PlayerCondition condition;

    public Transform dropPosition;

    private ItemData selectedItem;
    private int selectedItemIndex = 0;

    private int currentEquipIndex;

    void Start()
    {
        controller = CharacterManager.Instance.Player.playerController;
        condition = CharacterManager.Instance.Player.playerCondition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle;
        
        CharacterManager.Instance.Player.AddItem += AddItem;
        
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }
        
        ClearSelectedItemWindow();
    }


    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStat.text = string.Empty;
        selectedItemStatValue.text = string.Empty;
        
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;
        
        //아이템 중복가능 여부 확인(canStack)해서 중첩 가능한 아이템이면
        if (data.canStack)
        {
            //해당 아이템이 인벤토리에 존재하고, 개수가 최대개수보다 작을 때
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                //개수 추가 & UI 업데이트 & itemData null로
                slot.quantity++;
                UIUpdate();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }
        
        //비어 있는 슬롯 가져온다.
        ItemSlot emptySlot = GetEmptySlot();

        // 빈 슬롯이 있다면 중첩 가능하든 아니든 일단 개수 1로 설정.
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UIUpdate();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }
        
        //없다면
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    void UIUpdate()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();                
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    //아이템 인벤토리 존재 여부 및 최대 개수 미만인지 조회
    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            //동일 아이템이고, 아이템의 개수가 설정한 max개수보다 작을 때.
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }            
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }            
        }
        return null;
    }

    // 아이템 인벤토리 -> 맵에 생성.
    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }

    // 
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;
        
        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;

        selectedItemStat.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.Consumables.Length; i++)
        {
            selectedItemStat.text += selectedItem.Consumables[i].ConsumableType + "\n";
            selectedItemStatValue.text += selectedItem.Consumables[i].value + "\n";
        }
        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        // equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        // unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.Consumables.Length; i++)
            {
                switch (selectedItem.Consumables[i].ConsumableType)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.Consumables[i].value);
                        break;
                    case ConsumableType.Speed:
                        condition.IncreaseSpeed(selectedItem.Consumables[i].value, 10);
                        break;
                    case ConsumableType.Jump:
                        condition.IncreaseJump(selectedItem.Consumables[i].value, 10);
                        break;
                    case ConsumableType.SJump:
                        condition.DoubleJump(10);
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }
    
    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    // 인벤토리(slots)에서 아이템 제거. 무조건 -1개씩 드랍.
    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }
        UIUpdate();
    }

    public void OnEquipButton()
    {
        if (slots[currentEquipIndex].equipped)
        {
            //해제
            UnEquip(currentEquipIndex);
        }
        
        slots[selectedItemIndex].equipped = true;
        currentEquipIndex = selectedItemIndex;
        
        // CharacterManager.Instance.Player.equip.EquipNew(selectedItem);
        UIUpdate();
        
        SelectItem(selectedItemIndex);
        
    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        // CharacterManager.Instance.Player.equip.UnEquip();
        UIUpdate();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUnEquipmentButton()
    {
        UnEquip(currentEquipIndex);
    }
    
}
