using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

// 상호작용가능한 Interface에 없다면 상호작용 못함 = 넘어가면 됨. 있다면 안에 기능들 사용 가능. 
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;
    
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.AddItem?.Invoke();
        Destroy(gameObject); //획득에서 인벤토리로 이동 ==> 맵 상에서 삭제.
    }
}