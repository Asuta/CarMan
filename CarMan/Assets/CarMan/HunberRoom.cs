using System;
using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;
using VInspector;

public class HunberRoom : MonoBehaviour
{
    public Transform targetT;
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2.0f;
    public float waitTime = 5.0f;
    public Grabbable grabbable;
    public GrabObject grabObject;
    

    // Start is called before the first frame update
    void Start()
    {
        // 在开始时将目标物体放在 point A
        if (targetT != null && pointA != null)
        {
            targetT.position = pointA.position;
        }

        MyEvent.MoveToSuspendPointEventStageTwo.AddListener(After5SecondMoveToPointB);
    }

    private void After5SecondMoveToPointB()
    {
        StartCoroutine(After5SecondMoveToPointBCoroutine());
    }

    // 5秒后移动到B点的协程
    private IEnumerator After5SecondMoveToPointBCoroutine()
    {
        Debug.Log("开始等待5秒...");
        yield return new WaitForSeconds(waitTime);
        Debug.Log("5秒等待结束，开始移动到B点");
        MoveToPointB();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 匀速移动到 B 点的方法
    [Button("MoveToPointB")]
    public void MoveToPointB()
    {
        StartCoroutine(MoveToPointBCoroutine());
    }

    // 协程实现匀速移动
    private IEnumerator MoveToPointBCoroutine()
    {
        if (targetT == null || pointA == null || pointB == null)
        {
            Debug.LogWarning("目标物体或移动点未设置!");
            yield break;
        }

        // 确保从 point A 开始
        targetT.position = pointA.position;
        
        float journeyLength = Vector3.Distance(pointA.position, pointB.position);
        float startTime = Time.time;
        
        while (Vector3.Distance(targetT.position, pointB.position) > 0.01f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            
            targetT.position = Vector3.Lerp(pointA.position, pointB.position, fractionOfJourney);
            
            yield return null;
        }
        
        // 确保精确到达 B 点
        targetT.position = pointB.position;
        Debug.Log("移动完成，已到达 B 点");
        
        // 移动完成后启用 Grabbable 和 GrabObject 脚本
        EnableGrabbingComponents();
    }

    // 启用抓取相关的组件
    private void EnableGrabbingComponents()
    {
        if (grabbable != null)
        {
            grabbable.enabled = true;
            Debug.Log("已启用 Grabbable 组件");
        }
        else
        {
            Debug.LogWarning("Grabbable 组件未设置!");
        }
        
        if (grabObject != null)
        {
            grabObject.enabled = true;
            Debug.Log("已启用 GrabObject 组件");
        }
        else
        {
            Debug.LogWarning("GrabObject 组件未设置!");
        }
    }
}
