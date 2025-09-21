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
        ShakeOne
    }

    // 当前状态
    private TaskState currentState = TaskState.Idle;
    private TaskState previousState;
    
    // 移动相关参数
    public float moveSpeed = 5f;
    public Transform pointA;


    public string nowState = "";
    
    // Start is called before the first frame update
    void Start()
    {
        // 初始化 previousState
        previousState = currentState;
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

        if (Input.GetKeyDown(KeyCode.A))
        {
            //invoke event
            Debug.Log("Invoke SystemStartEvent");
            MyEvent.SystemStartEvent.Invoke();
        }
        
        // 根据当前状态执行相应的逻辑
        switch (currentState)
        {
            case TaskState.Idle:
                Idle();
                break;
            case TaskState.MoveToA:
                ExecuteMove();
                break;
            case TaskState.ShakeOne:
                ExecuteShakeOne();
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
    }

    private void Move()
    {
        Debug.Log("Move");
    }

    #region 状态方法

    private void Idle()
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
        }
    }

    // Idle状态进入/退出方法
    private void OnEnterIdle()
    {
        Debug.Log("进入Idle状态");
        // 注册事件监听器
        MyEvent.LoadEvent.AddListener(OnLoadEventTriggered);
        MyEvent.SystemStartEvent.AddListener(OnSystemStartEventTriggered);
        Debug.Log("已注册事件监听器");
    }

    private void OnExitIdle()
    {
        Debug.Log("离开Idle状态");
        // 取消注册事件监听器
        MyEvent.LoadEvent.RemoveListener(OnLoadEventTriggered);
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
    }

    private void OnExitShakeOne()
    {
        Debug.Log("离开ShakeOne状态");
        // 在这里添加离开ShakeOne状态时的逻辑
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
        // currentState = TaskState.MoveToB;
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
