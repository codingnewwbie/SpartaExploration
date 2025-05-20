using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{   //움직일 때 필요한 값
    [Header("Move")] public float moveSpeed;
    private Vector2 currentMovementInput;
    
    //점프에 필요한 값
    [Header("Jump")] public float jumpPower;
    public LayerMask groundLayerMask;
    
    //카메라 조절 시 필요한 값
    [Header("Look")] public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    public float cameraCurrentXRotation;
    public float lookSensitivity;
    public Vector2 mouseDelta;
    
    private Rigidbody rigidbody;

    public bool canLook = true;
    public Action inventory;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>(); //rigidbody 초기화
    }
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 시작 시 마우스 위치 중앙 고정 및 보이지 않게 해서 1인칭 시점 구현.
    }
    
    void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();            
        }
    }
    
    private void FixedUpdate()
    {
        Move();
    }
    
    #region Move
    // 실제 Player가 움직이게 함
    void Move()
    {
        // currentMovementInput의 x,y값은 2D 기준 방향. dir.y는 3d 기준 방향.  
        Vector3 dir = transform.forward * currentMovementInput.y + transform.right * currentMovementInput.x;
        dir *= moveSpeed;
        dir.y = rigidbody.velocity.y; // 점프를 했을 때만 위/아래로 움직이도록 하기 위해 기존 y값 유지.

        rigidbody.velocity = dir;

    }

    // 입력값을 받아 실제 움직임에서 사용.
    public void OnMove(InputAction.CallbackContext context) //현재 상태 받아오기
    {
        if (context.phase == InputActionPhase.Performed) //Started = 키가 눌린 순간 1회, Performed = 키 눌리면 지속 
        {
            currentMovementInput = context.ReadValue<Vector2>(); // 입력값 계속해서 넘겨줌.
        }
        else if (context.phase == InputActionPhase.Canceled) // 누르다가 손 똈을 때
        {
            currentMovementInput = Vector2.zero;
        }
    }
    #endregion

    #region Look

    // 실제 카메라 이동 조절
    void CameraLook()
    {
        // 마우스 움직여보면 좌우 움직임 넣을때(마우스 좌우이동) y축 기준으로 회전, 상하 움직임이면 x축 기준 회전함.
        cameraCurrentXRotation -= mouseDelta.y * lookSensitivity;
        cameraCurrentXRotation = Mathf.Clamp(cameraCurrentXRotation, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(cameraCurrentXRotation, 0, 0);
        /* 마우스 실제 움직임과 카메라 움직임을 맞추기 위해 - 붙임.
           여기서 음수로 전환하던가, 아니면 처음에 XRotation 할 때 -=하던가 둘 중 하나.
           상하 움직임 시 캐릭터는 그대로, 카메라만 움직이고  
        좌우 움직임 시 카메라 멈추고 캐릭터 움직이도록 카메라와 Player의 Angles 조절 */
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }
    
    // 마우스 움직임 값 받아옴
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>(); //마우스가 움직인 방향 + 힘을 읽어냄. 마우스 고정되어 있음
    }

    #endregion

    #region Jump
    //점프 키 입력 값을 받아옴.
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounrded() && CharacterManager.Instance.Player.playerCondition.UseStamina(10))
        {
            rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    bool IsGrounrded()
    {   // 플레이어 기준 책상 다리 4개를 만든다고 생각.
        Ray[] rays = new Ray[4]
        {
            // transform.up 이유 = player와 ground가 맞닿아 있는 곳에서 아래로 Ray를 쏘면 ground가 무시될 수 있어서 땅 위로 살짝 올려줌
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        // 이거 안됬던 이유 : 피벗이 바닥 쪽에 있는 게 아니라, 플레이어 중심에 있어서 0.1f를 내려도 땅에 닿이지 않아서.
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.5f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    #endregion

    #region MyRegion
    // 인벤토리 열고 닫음에 따라 커서값 조절
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    #endregion
}
