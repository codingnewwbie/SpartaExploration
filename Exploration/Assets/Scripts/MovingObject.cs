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
        prevPosition = transform.position;
        target = new Vector3(prevPosition.x + movingAmount, prevPosition.y, prevPosition.z);
        targetPosition = target;
        StartCoroutine(MovingPanel());
    }
    
    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {   
            other.transform.SetParent(this.transform);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {   
            other.transform.SetParent(null);
        }
    }

    private void StartMovingPanel()
    {
        coroutine = StartCoroutine(MovingPanel());
    }
    
    private IEnumerator MovingPanel()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(5f);
            // 현재 위치와 목표 위치를 비교, 위치 도달 시 목표 위치를 바꿔 계속해서 이동하도록.
            if (Vector3.Distance(transform.position, target) < 0.05f)
            {
                target = (target == prevPosition) ? targetPosition : prevPosition;
            }
        }
    }
}
