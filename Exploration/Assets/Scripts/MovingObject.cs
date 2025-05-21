using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private float movingAmount = 5f;
    private float prevMovingTime = 3f;
    private Vector3 prevPosition;
    private Coroutine coroutine;

    Vector3 velocity = Vector3.zero;
    float smoothTime = 1f;
    private Vector3 target;
    private Vector3 targetPosition;
    
    private void Start()
    {
        // 시작 위치와 끝 위치, 그리고 각 위치에 도달 시 바뀌는 목표 위치 설정, 5초마다 목표 위치 바꾸기 설정. 
        prevPosition = transform.position;
        target = new Vector3(prevPosition.x + movingAmount, prevPosition.y, prevPosition.z);
        targetPosition = target;
        StartCoroutine(MovingPanel());
    }
    
    private void Update()
    {
        // update 아니면 1프레임 이동하고 멈춤
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }

    // 플레이어가 발판에 올라오면 같이 이동하기 위함.
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {   
            other.transform.SetParent(this.transform);
        }
    }
    
    // 발판에서 나가면 발판 움직임과 동기화 해제
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {   
            other.transform.SetParent(null);
        }
    }
    
    // 발판 이동 로직
    private IEnumerator MovingPanel()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            // 현재 위치와 목표 위치를 비교, (목표 위치 도달 시 목표 위치를 변경 후 목표위치까지 이동).
            if (Vector3.Distance(transform.position, target) < 0.05f)
            {
                target = (target == prevPosition) ? targetPosition : prevPosition;
            }
        }
    }
}
