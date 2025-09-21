using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cleaner : MonoBehaviour
{
    // 定义状态枚举
    public enum TaskState
    {
        Idle,
        MoveToA,
        ShakeOne,
        ShakeTwo,
        GoBackToB
    }

    // 当前状态
    private TaskState currentState = TaskState.Idle;
    private TaskState previousState;

    // 移动相关参数
    public float moveSpeed = 5f;
    public Transform pointA;
    public Transform pointB;

    // 摇晃相关参数
    public float shakeAmplitude = 0.5f; // 摇晃的幅度
    public float shakeSpeed = 2f;      // 摇晃的速度

    // 摇晃状态变量
    private Vector3 shakeOrigin;       // 摇晃的原始位置
    private float shakeTime = 0f;      // 摇晃时间累加器


    public string nowState = "";

    // Start is called before the first frame update
    void Start()
    {
        // 初始化 previousState

        previousState = currentState;
        HandleStateEnter(currentState);

    }

    // Update is called once per frame
    void Update()
    {
        nowState = currentState.ToString();
        //test
        // 按空格键切换Idle状态和Move状态
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentState == TaskState.Idle)
            {
                currentState = TaskState.MoveToA;
            }
            else if (currentState == TaskState.MoveToA)
            {
                currentState = TaskState.Idle;
            }
        }



        // 根据当前状态执行相应的逻辑
        switch (currentState)
        {
            case TaskState.Idle:
                ExecuteIdle();
                break;
            case TaskState.MoveToA:
                ExecuteMove();
                break;
            case TaskState.ShakeOne:
                ExecuteShakeOne();
                break;
            case TaskState.ShakeTwo:
                ExecuteShakeTwo();
                break;
        }

        // 检测状态变化，处理状态进入/退出
        if (previousState != currentState)
        {
            // 处理离开上一个状态
            HandleStateExit(previousState);
            // 处理进入新状态
            HandleStateEnter(currentState);

            // 更新 previousState
            previousState = currentState;
        }


        //test==================
        if (Input.GetKeyDown(KeyCode.A))
        {
            //invoke event
            Debug.Log("Invoke SystemStartEvent");
            MyEvent.SystemStartEvent.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            //invoke event
            Debug.Log("Invoke WindshieldEvent");
            MyEvent.WindshieldEvent.Invoke();
        }
    }


    #region 状态方法

    private void ExecuteIdle()
    {
        // 在Idle状态中等待事件触发
        // 事件触发时会调用对应的处理方法
    }

    private void ExecuteMove()
    {
        // 计算朝向A点的方向
        Vector3 direction = (pointA.position - transform.position).normalized;

        // 朝向A点移动
        transform.position += direction * moveSpeed * Time.deltaTime;

        // 检查是否到达A点（使用一个小的阈值来判断是否到达）
        float distance = Vector3.Distance(transform.position, pointA.position);
        if (distance < 0.1f) // 0.1f作为到达的阈值
        {
            // 到达A点，切换到ShakeOne状态
            currentState = TaskState.ShakeOne;
        }
    }

    private void ExecuteShakeOne()
    {
        // ShakeOne 状态的执行逻辑
        // 实现前后摇晃效果

        // 如果是第一次进入摇晃状态，记录原始位置
        if (shakeTime == 0f)
        {
            shakeOrigin = transform.position;
        }

        // 累加时间
        shakeTime += Time.deltaTime * shakeSpeed;

        // 使用正弦函数计算前后偏移
        float offsetZ = Mathf.Sin(shakeTime) * shakeAmplitude;

        // 应用偏移到物体的位置
        Vector3 newPosition = shakeOrigin;
        newPosition.z += offsetZ;
        transform.position = newPosition;
    }

    private void ExecuteShakeTwo()
    {
        // ShakeTwo 状态的执行逻辑
        // 实现前后摇晃效果，速度是ShakeOne的两倍

        // 如果是第一次进入摇晃状态，记录原始位置
        if (shakeTime == 0f)
        {
            shakeOrigin = transform.position;
        }

        // 累加时间，速度是ShakeOne的两倍
        shakeTime += Time.deltaTime * shakeSpeed * 2f;

        // 使用正弦函数计算前后偏移
        float offsetZ = Mathf.Sin(shakeTime) * shakeAmplitude;

        // 应用偏移到物体的位置
        Vector3 newPosition = shakeOrigin;
        newPosition.z += offsetZ;
        transform.position = newPosition;
    }

    #endregion

    #region 状态进入/退出方法

    // 处理进入状态
    private void HandleStateEnter(TaskState state)
    {
        switch (state)
        {
            case TaskState.Idle:
                OnEnterIdle();
                break;
            case TaskState.MoveToA:
                OnEnterMove();
                break;
            case TaskState.ShakeOne:
                OnEnterShakeOne();
                break;
            case TaskState.ShakeTwo:
                OnEnterShakeTwo();
                break;
        }
    }

    // 处理离开状态
    private void HandleStateExit(TaskState state)
    {
        switch (state)
        {
            case TaskState.Idle:
                OnExitIdle();
                break;
            case TaskState.MoveToA:
                OnExitMove();
                break;
            case TaskState.ShakeOne:
                OnExitShakeOne();
                break;
            case TaskState.ShakeTwo:
                OnExitShakeTwo();
                break;
        }
    }

    // Idle状态进入/退出方法
    private void OnEnterIdle()
    {
        Debug.Log("进入Idle状态");
        // 注册事件监听器
        MyEvent.SystemStartEvent.AddListener(OnSystemStartEventTriggered);
        Debug.Log("已注册事件监听器");
    }

    private void OnExitIdle()
    {
        Debug.Log("离开Idle状态");
        // 取消注册事件监听器
        MyEvent.SystemStartEvent.RemoveListener(OnSystemStartEventTriggered);
        Debug.Log("已取消注册事件监听器");
    }

    // Move状态进入/退出方法
    private void OnEnterMove()
    {
        Debug.Log("进入Move状态");
        // 在这里添加进入Move状态时的逻辑
    }

    private void OnExitMove()
    {
        Debug.Log("离开Move状态");
        // 在这里添加离开Move状态时的逻辑
    }

    // ShakeOne状态进入/退出方法
    private void OnEnterShakeOne()
    {
        Debug.Log("进入ShakeOne状态");
        // 在这里添加进入ShakeOne状态时的逻辑
        // 重置摇晃时间，确保每次进入状态时从开始摇晃
        shakeTime = 0f;
        MyEvent.WindshieldEvent.AddListener(OnWindshieldEventTriggered);
    }

    private void OnExitShakeOne()
    {
        Debug.Log("离开ShakeOne状态");
        // 在这里添加离开ShakeOne状态时的逻辑
        MyEvent.WindshieldEvent.RemoveListener(OnWindshieldEventTriggered);
    }

    private void OnWindshieldEventTriggered()
    {
        Debug.Log("WindshieldEvent被触发，当前状态是ShakeOne");
        // 在这里添加WindshieldEvent触发时的逻辑
        currentState = TaskState.ShakeTwo;
    }

    private void OnEnterShakeTwo()
    {
        Debug.Log("进入ShakeTwo状态");
        // 在这里添加进入ShakeTwo状态时的逻辑
        // 重置摇晃时间，确保每次进入状态时从开始摇晃
        shakeTime = 0f;
    }

    #endregion

    #region 事件处理方法

    // LoadEvent事件触发时的处理方法
    private void OnLoadEventTriggered()
    {
        Debug.Log("LoadEvent被触发，当前状态是Idle");
        // 在这里添加LoadEvent触发时的逻辑
        // 例如：切换到其他状态
        // currentState = TaskState.MoveToB;
    }

    // SystemStartEvent事件触发时的处理方法
    private void OnSystemStartEventTriggered()
    {
        Debug.Log("SystemStartEvent被触发，当前状态是Idle");
        // 在这里添加SystemStartEvent触发时的逻辑
        // 例如：切换到其他状态
        currentState = TaskState.MoveToA;
    }

    private void OnExitShakeTwo()
    {
        Debug.Log("离开ShakeTwo状态");
        // 在这里添加离开ShakeTwo状态时的逻辑
    }

    #endregion

    // 在对象销毁时取消注册事件监听器，避免内存泄漏
    void OnDestroy()
    {
        // 如果当前是Idle状态，先取消注册事件监听器
        if (currentState == TaskState.Idle)
        {
            MyEvent.LoadEvent.RemoveListener(OnLoadEventTriggered);
            MyEvent.SystemStartEvent.RemoveListener(OnSystemStartEventTriggered);
        }
    }
}
