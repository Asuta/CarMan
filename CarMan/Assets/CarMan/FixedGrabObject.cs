using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class FixedGrabObject : MonoBehaviour
{
    public Transform FixedPoint;
    private bool isTracking = true;
    
    // Start is called before the first frame update
    void Start()
    {
        // 不再需要记录初始位置和旋转
    }

    // Update is called once per frame
    void Update()
    {
        // 如果处于跟踪状态且FixedPoint不为空，则跟随FixedPoint的位置和旋转
        if (isTracking && FixedPoint != null)
        {
            transform.position = FixedPoint.position;
            transform.rotation = FixedPoint.rotation;
        }
    }

    [Button("OnGrabObject")]
    public void OnGrabObject()
    {
        var thisRb = GetComponent<Rigidbody>();
        thisRb.isKinematic = false;
        // 抓取时关闭跟踪
        isTracking = false;
    }

    [Button("OnReleaseObject")]
    public void OnReleaseObject()
    {
        var thisRb = GetComponent<Rigidbody>();
        thisRb.isKinematic = true;
        // 释放时开启跟踪
        isTracking = true;
    }
}
