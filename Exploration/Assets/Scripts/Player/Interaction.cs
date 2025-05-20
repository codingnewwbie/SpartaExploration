using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // 아이템 조사 시간 간격
    private float lastCheckTime;
    public float maxCheckDistance; //최대 아이템조사 거리
    public LayerMask layerMask; //어떤 레이어가 달려있는 게임오브젝트를 추출하느냐.

    public GameObject currentInteractGameObject; //현재 조사 대상 아이템
    private IInteractable currentInteractable;

    public TextMeshProUGUI prompText; // UI 분리시켜서 드래그앤드랍 안하고 어떻게 할지 리팩토링 고민 + 꼭 고민
    private Camera camera;
    
    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;  
    
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // ray 발사점
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != currentInteractGameObject)
                {
                    currentInteractGameObject = hit.collider.gameObject;
                    currentInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                currentInteractGameObject = null;
                currentInteractable = null;
                prompText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        prompText.gameObject.SetActive(true);
        prompText.text = currentInteractable.GetInteractPrompt();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && currentInteractable != null)
        {
            currentInteractable.OnInteract();
            currentInteractGameObject = null;
            currentInteractable = null;
            prompText.gameObject.SetActive(false);
        }
    }
}
