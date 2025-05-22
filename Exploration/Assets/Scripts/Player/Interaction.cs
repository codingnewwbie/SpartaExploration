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
    public LayerMask ladderLayerMask; //어떤 레이어가 달려있는 게임오브젝트를 추출하느냐.

    public GameObject currentInteractGameObject; //현재 조사 대상 아이템
    private IInteractable currentInteractable;

    private TextMeshProUGUI prompText; // UI 분리시켜서 드래그앤드랍 안하고 어떻게 할지 리팩토링 고민 + 꼭 고민
    private Camera camera;
    private bool prevView;    
    
    private void Start()
    {
        camera = Camera.main;
        prevView = CharacterManager.Instance.Player.playerController.is1stView;
        
        prompText = GameObject.Find("UI/Canvas/PrompText").GetComponent<TextMeshProUGUI>(); // 리팩토링1. 찾아오기.
        // 이거 말고 UIManager에서 불러와도 될 거 같긴 한데.
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate) // = 아이템 검색 주기가 되었다면
        {
            lastCheckTime = Time.time; // 또 사용할 검색기준값 갱신하고 
    
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // ray를 camera 정 중앙에서 쏘고
            RaycastHit hit; // hit은 ray와 충돌한 물체
            
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != currentInteractGameObject) // ray가 새로운 물체 봤을 때만
                {
                    currentInteractGameObject = hit.collider.gameObject; //현재오브젝트에 넣고
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
            
            bool hitLadder = Physics.Raycast(ray, out hit, 1f, ladderLayerMask);
            
            if (CharacterManager.Instance.Player.playerController.isLadder != hitLadder)
            {
                CharacterManager.Instance.Player.playerController.isLadder = hitLadder;
            }
        }

        if (prevView != CharacterManager.Instance.Player.playerController.is1stView)
        {
            camera.transform.localPosition = prevView ? new Vector3(0, 0, 0) : new Vector3(0, 5f, -5f);
            maxCheckDistance = prevView ? 3 : 10;
            prevView = CharacterManager.Instance.Player.playerController.is1stView;
        }
    }

    private void SetPromptText()
    {
        prompText.gameObject.SetActive(true);
        prompText.text = currentInteractable.GetInteractPrompt();
    }

    //상호작용 가능하면 해당 물체를 인벤토리로 이동(현재 아이템 습득만 가능함)
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
