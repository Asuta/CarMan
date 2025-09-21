using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner : MonoBehaviour
{
    // 定义状态枚举
    public enum TaskState
    {
        MoveToB,       // 从A点移动到B点
        WaitForInput,  // 等待按钮输入
        MoveLeft,      // 向左移动
        MoveRight,     // 向右移动
        Complete,       // 任务完成
        Idle
    }

    // 当前状态
    private TaskState currentState = TaskState.Idle;
    
    // 移动相关参数
    public float moveSpeed = 5f;
    public Transform pointA;  // 起始点
    public Transform pointB;  // 目标点B
    public Transform leftTarget;  // 左侧目标点
    public Transform rightTarget; // 右侧目标点
    
    // Start is called before the first frame update
    void Start()
    {
        // 如果没有设置点A，则使用当前位置作为点A
        if (pointA == null)
        {
            pointA = transform;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        // 根据当前状态执行相应的逻辑
        switch (currentState)
        {
            case TaskState.MoveToB:
                MoveToTarget(pointB, TaskState.WaitForInput);
                break;
                
            case TaskState.WaitForInput:
                MoveToTarget(pointB, TaskState.WaitForInput);
                break;
                
            case TaskState.MoveLeft:
                MoveToTarget(leftTarget, TaskState.Complete);
                break;
                
            case TaskState.MoveRight:
                MoveToTarget(rightTarget, TaskState.Complete);
                break;
                
            case TaskState.Complete:
                // 任务完成，可以在这里添加完成后的逻辑
                Debug.Log("所有任务已完成！");
                break;
            case TaskState.Idle:
                Idle();
                break;
        }
    }

    private void Idle()
    {
        
    }

    void MoveToTarget(Transform target, TaskState nextState)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            currentState = nextState;
        }
    }
    

}
