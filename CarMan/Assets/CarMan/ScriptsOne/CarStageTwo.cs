using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;


public class CarStageTwo : MonoBehaviour
{
    public List<PointPair> pointPairs = new List<PointPair>();
    public float moveSpeed = 2.0f;
    private float originalMoveSpeed; // 保存原始速度（不初始化，在Start中设置）
    private Transform thisT;
    private bool isMoving = false;
    private float journeyLength;
    private float startTime;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private int currentPairIndex = 0;
    private Coroutine speedChangeCoroutine; // 速度变化协程引用
    public Transform SuspendPoint;
    public Transform SuspendPointB;
    public Transform SuspendPointC;
    public Transform EndPoint;
    // Start is called before the first frame update
    void Start()
    {
        // 如果没有手动设置 thisT，则使用自身的 Transform
        if (thisT == null)
        {
            thisT = transform;
        }

        // 保存原始移动速度
        originalMoveSpeed = moveSpeed;

        // // 监听栏杆完全打开事件（stage One）
        // MyEvent.RodAxisFullyOpenedEvent.AddListener(OnRodAxisFullyOpened);
        // MyEvent.MoveContinueEvent.AddListener(OnMoveContinue);

        // // 监听栏杆start打开事件（stage Two）
        MyEvent.SystemStartEventTwo.AddListener(OnSystemStartEventTriggered);

        // 监听释放汉堡事件
        MyEvent.ReleaseHunbergerEventStageTwo.AddListener(OnReleaseHunbergerEventTriggered);

        // 监听栏杆完全打开事件（stage Two）
        MyEvent.RodAxisFullyOpenedEvent.AddListener(OnRodAxisFullyOpened);


    }

    private void OnReleaseHunbergerEventTriggered()
    {
        // 等待五秒  继续执行StartContinueMoveToPoint方法
        StartCoroutine(WaitAndContinueMove());
    }

    // 等待5秒后继续移动的协程
    private IEnumerator WaitAndContinueMove()
    {
        Debug.Log("汉堡释放事件触发，等待5秒后继续移动...");
        yield return new WaitForSeconds(2f);
        Debug.Log("5秒等待结束，继续移动");
        StartContinueMoveToPoint();
    }

    private void OnSystemStartEventTriggered()
    {
        StartContinueMoveToPoint();
    }

    // Update is called once per frame
    void Update()
    {
        // 检测空格键按下
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving && pointPairs.Count > 0)
        {
            // 调用移动方法，使用当前索引的点对
            StartMoveToPoint();

            // 移动到下一个点对（循环）
            currentPairIndex = (currentPairIndex + 1) % pointPairs.Count;
        }

        // 如果正在移动，更新位置
        if (isMoving)
        {
            ContinueMoving();
        }
    }

    // 开始移动：使用当前索引的点对
    [Button("StartContinueMoveToPoint")]
    void StartContinueMoveToPoint()
    {
        // 调用移动方法，使用当前索引的点对
        StartMoveToPoint();

        // 移动到下一个点对（循环）
        currentPairIndex = (currentPairIndex + 1) % pointPairs.Count;
    }



    // 开始移动：使用当前索引的点对
    [Button("StartMoveToPoint")]
    void StartMoveToPoint()
    {
        if (currentPairIndex >= 0 && currentPairIndex < pointPairs.Count)
        {
            PointPair pair = pointPairs[currentPairIndex];
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


    // 开始移动：使用当前索引的点对
    [Button("textNextMove")]
    void textNextMove()
    {
        if (currentPairIndex >= 0 && currentPairIndex < pointPairs.Count)
        {
            PointPair pair = pointPairs[currentPairIndex];
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

            // 判断是否可以继续移动，如果可以则移动到下一个点对
            if (pointPairs.Count > 0)
            {
                // 使用正确的索引来检查当前到达的点
                int completedPairIndex = (currentPairIndex - 1 + pointPairs.Count) % pointPairs.Count;
                CanContinueMoving(pointPairs[completedPairIndex].pointB);
            }
        }

    }

    // 栏杆完全打开事件处理方法
    private void OnRodAxisFullyOpened()
    {
        Debug.Log("栏杆完全打开事件触发，汽车开始移动");
        if (!isMoving && pointPairs.Count > 0)
        {
            // 调用移动方法，使用当前索引的点对
            StartMoveToPoint();

            // 移动到下一个点极（循环）
            currentPairIndex = (currentPairIndex + 1) % pointPairs.Count;
        }
    }

    // 继续移动事件处理方法
    private void OnMoveContinue()
    {
        Debug.Log("栏杆完全打开事件触发，汽车开始移动");
        if (!isMoving && pointPairs.Count > 0)
        {
            // 调用移动方法，使用当前索引的点对
            StartMoveToPoint();

            // 移动到下一个点对（循环）
            currentPairIndex = (currentPairIndex + 1) % pointPairs.Count;
        }
    }

    // 在对象销毁时移除事件监听，避免内存泄漏
    private void OnDestroy()
    {
        MyEvent.RodAxisFullyOpenedEvent.RemoveListener(OnRodAxisFullyOpened);
        MyEvent.MoveContinueEvent.RemoveListener(OnMoveContinue);
        MyEvent.RodAxisFullyOpenedEvent.RemoveListener(OnRodAxisFullyOpened);
    }

    // 自动移动到下一个点对
    IEnumerator AutoMoveToNextPair()
    {
        // 短暂延迟，让用户看到移动完成
        yield return new WaitForSeconds(0f);

        // 移动到下一个点对
        StartMoveToPoint();
        currentPairIndex = (currentPairIndex + 1) % pointPairs.Count;
    }

    // 判断是否可以继续移动到下一个点对
    void CanContinueMoving(Transform currentPoint)
    {
        if (currentPoint == SuspendPoint)
        {
            MyEvent.MoveToSuspendPointEventStageTwo.Invoke();
            Debug.Log("移动到终点事件触发  到汉堡店了");
            return;
        }
        if (currentPoint == SuspendPointB)
        {
            MyEvent.MoveToSuspendPointEventStageTwoB.Invoke();
            Debug.Log("移动到点B事件触发  到收费站了");
            return;
        }
        if (currentPoint == SuspendPointC)
        {
            MyEvent.MoveToSuspendPointEventStageTwoC.Invoke();

            Debug.Log("移动到点C事件触发  到收费站了");

            // 启动速度渐变协程：从当前速度降到50%，在1秒内完成
            if (speedChangeCoroutine != null)
            {
                StopCoroutine(speedChangeCoroutine);
            }
            speedChangeCoroutine = StartCoroutine(GraduallyReduceSpeed(1.0f, 0.5f));
            
            // 立即继续移动，同时进行速度渐变
            StartCoroutine(AutoMoveToNextPair());

            return;
        }
        else if (currentPoint == EndPoint)
        {
            // MyEvent.MoveToEndPointEvent.Invoke();
            // MyEvent.CameraBlackModeEvent.Invoke();
            Debug.Log("移动到end事件触发");
            MyEvent.MoveToSuspendPointEventStageTwoEnd.Invoke();
            return;
        }
        else
        {
            Debug.Log("继续移动事件触发");
            StartCoroutine(AutoMoveToNextPair());
        }
    }

    /// <summary>
    /// 逐渐降低速度的协程
    /// </summary>
    /// <param name="duration">渐变持续时间（秒）</param>
    /// <param name="targetPercentage">目标速度百分比（0-1）</param>
    private IEnumerator GraduallyReduceSpeed(float duration, float targetPercentage)
    {
        float startSpeed = moveSpeed;
        float targetSpeed = originalMoveSpeed * targetPercentage;
        float elapsedTime = 0f;
        
        Debug.Log($"开始速度渐变: 从 {startSpeed} 降到 {targetSpeed}，持续 {duration} 秒");
        Debug.Log($"原始速度: {originalMoveSpeed}, 目标百分比: {targetPercentage}");
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            moveSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);
            yield return null;
        }
        
        // 确保最终速度精确到达目标值
        moveSpeed = targetSpeed;
        Debug.Log($"速度渐变完成: 当前速度 {moveSpeed}");
        
        // 验证最终速度是否正确
        if (Mathf.Approximately(moveSpeed, targetSpeed))
        {
            Debug.Log("速度渐变验证: 成功达到目标速度");
        }
        else
        {
            Debug.LogWarning($"速度渐变验证: 期望 {targetSpeed}，实际 {moveSpeed}");
        }
    }
}
