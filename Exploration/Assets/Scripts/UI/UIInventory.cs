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
        
        //아이템 중복가능 여부 확인(canStack)해서 
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UIUpdate();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }
        
        //비어 있는 슬롯 가져온다.
        ItemSlot emptySlot = GetEmptySlot();

        //있다면
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

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
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

    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }

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
