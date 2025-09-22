using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class RodAxis : MonoBehaviour
{
    public Vector3 closeAngle = new Vector3(0, 0, 0);
    public Vector3 openAngle = new Vector3(0, 0, 90);
    public float rotationSpeed = 90f; // 旋转速度（度/秒）
    
    private bool isRotating = false;
    private Quaternion targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            // 平滑旋转到目标角度
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            // 检查是否到达目标角度
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                isRotating = false;
                transform.rotation = targetRotation; // 确保精确到达目标角度
            }
        }
    }

    /// <summary>
    /// 平滑旋转到打开角度
    /// </summary>
    [Button("Open")]
    public void Open()
    {
        targetRotation = Quaternion.Euler(openAngle);
        isRotating = true;
    }

    /// <summary>
    /// 平滑旋转到关闭角度
    /// </summary>
    [Button("Close")]
    public void Close()
    {
        targetRotation = Quaternion.Euler(closeAngle);
        isRotating = true;
    }
}
