using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCar : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2.0f;
    public Transform thisT;
    
    private bool isMoving = false;
    private float journeyLength;
    private float startTime;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        // 如果没有手动设置 thisT，则使用自身的 Transform
        if (thisT == null)
        {
            thisT = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 检测空格键按下
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            // 调用移动方法，从 pointA 移动到 pointB
            StartMoveToPoint(pointA, pointB);
        }
        
        // 如果正在移动，更新位置
        if (isMoving)
        {
            ContinueMoving();
        }
    }

    // 开始移动：从 startPoint 移动到 endPoint
    void StartMoveToPoint(Transform startPoint, Transform endPoint)
    {
        if (startPoint != null && endPoint != null && thisT != null)
        {
            isMoving = true;
            startPosition = startPoint.position;
            targetPosition = endPoint.position;
            journeyLength = Vector3.Distance(startPosition, targetPosition);
            startTime = Time.time;
            
            // 设置初始位置
            thisT.position = startPosition;
        }
    }
    
    // 持续移动更新
    void ContinueMoving()
    {
        float distCovered = (Time.time - startTime) * moveSpeed;
        float fractionOfJourney = distCovered / journeyLength;
        
        // 使用 Lerp 平滑移动
        thisT.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
        
        // 检查是否到达目标
        if (fractionOfJourney >= 1.0f)
        {
            isMoving = false;
            thisT.position = targetPosition; // 确保精确到达目标位置
        }
    }
}
