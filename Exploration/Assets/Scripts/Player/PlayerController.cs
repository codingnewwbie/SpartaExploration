using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
