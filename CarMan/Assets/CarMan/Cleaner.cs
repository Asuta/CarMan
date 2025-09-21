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
        Move
    }

    // 当前状态
    private TaskState currentState = TaskState.Idle;
    
    // 移动相关参数
    public float moveSpeed = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        // 初始化逻辑
    }

    // Update is called once per frame
    void Update()
    {
        // 保存前一帧的状态
        TaskState previousState = currentState;
        
        //test
        // 按空格键切换Idle状态和Move状态
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentState == TaskState.Idle)
            {
                currentState = TaskState.Move;
            }
            else if (currentState == TaskState.Move)
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
        
        // 检测状态变化，处理状态进入/退出
        if (previousState != currentState)
        {
            // 处理离开上一个状态
            HandleStateExit(previousState);
            // 处理进入新状态
            HandleStateEnter(currentState);
        }
        
        // 根据当前状态执行相应的逻辑
        switch (currentState)
        {
            case TaskState.Idle:
                Idle();
                break;
            case TaskState.Move:
                ExecuteMove();
                break;
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
        Debug.Log("Move");
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
            case TaskState.Move:
                OnEnterMove();
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
            case TaskState.Move:
                OnExitMove();
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
