using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointPair
{
    public Transform pointA;
    public Transform pointB;
}

public class TestCar : MonoBehaviour
{
    public List<PointPair> pointPairs = new List<PointPair>();
    public float moveSpeed = 2.0f;
    public Transform thisT;
    public bool isAutoMoveNext = false;
    
    private bool isMoving = false;
    private float journeyLength;
    private float startTime;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private int currentPairIndex = 0;
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
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving && pointPairs.Count > 0)
        {
            // 调用移动方法，使用当前索引的点对
            StartMoveToPoint(currentPairIndex);
            
            // 移动到下一个点对（循环）
            currentPairIndex = (currentPairIndex + 1) % pointPairs.Count;
        }
        
        // 如果正在移动，更新位置
        if (isMoving)
        {
            ContinueMoving();
        }
    }

    // 开始移动：使用指定索引的点对
    void StartMoveToPoint(int pairIndex)
    {
        if (pairIndex >= 0 && pairIndex < pointPairs.Count)
        {
            PointPair pair = pointPairs[pairIndex];
            if (pair.pointA != null && pair.pointB != null && thisT != null)
            {
                isMoving = true;
                startPosition = pair.pointA.position;
                targetPosition = pair.pointB.position;
                startRotation = pair.pointA.rotation;
                targetRotation = pair.pointB.rotation;
                journeyLength = Vector3.Distance(startPosition, targetPosition);
                startTime = Time.time;
                
                // 设置初始位置和旋转
                thisT.position = startPosition;
                thisT.rotation = startRotation;
            }
        }
    }
    
    // 持续移动更新
    void ContinueMoving()
    {
        float distCovered = (Time.time - startTime) * moveSpeed;
        float fractionOfJourney = distCovered / journeyLength;
        
        // 使用 Lerp 平滑移动位置
        thisT.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
        
        // 使用 Slerp 平滑旋转（与位置同步）
        thisT.rotation = Quaternion.Slerp(startRotation, targetRotation, fractionOfJourney);
        
        // 检查是否到达目标
        if (fractionOfJourney >= 1.0f)
        {
            isMoving = false;
            thisT.position = targetPosition; // 确保精确到达目标位置
            thisT.rotation = targetRotation; // 确保精确到达目标旋转
            
            // 如果启用自动移动，移动到下一个点对
            if (isAutoMoveNext && pointPairs.Count > 0)
            {
                // 短暂延迟后开始下一个移动
                StartCoroutine(AutoMoveToNextPair());
            }
        }
    }
    
    // 自动移动到下一个点对
    IEnumerator AutoMoveToNextPair()
    {
        // 短暂延迟，让用户看到移动完成
        yield return new WaitForSeconds(0f);
        
        // 移动到下一个点对
        StartMoveToPoint(currentPairIndex);
        currentPairIndex = (currentPairIndex + 1) % pointPairs.Count;
    }
}
