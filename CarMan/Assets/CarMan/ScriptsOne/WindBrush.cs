using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class WindBrush : MonoBehaviour
{
    public Transform windOne;
    public Transform windTwo;
    public Vector3 startRoation = new Vector3(0, 0, 0);
    public Vector3 endRoation = new Vector3(0, 0, 66);
    private bool isWiping = false;
    // Start is called before the first frame update
    void Start()
    {
         MyEvent.WindshieldEvent.AddListener(OnWindshieldEvent);
    }

    [Button("OnWindshieldEvent")]
    private void OnWindshieldEvent()
    {
        if (isWiping) return; // 如果正在转动，不响应事件
        StartCoroutine(WindshieldWipeAnimation());
    }

    private IEnumerator WindshieldWipeAnimation()
    {
        isWiping = true; // 设置正在转动状态
        
        // 第一次转动 - 两个雨刷同时转动到结束角度
        StartCoroutine(RotateWindshield(windOne, endRoation, 0.5f));
        StartCoroutine(RotateWindshield(windTwo, endRoation, 0.5f));
        yield return new WaitForSeconds(0.5f);
        
        // 第一次返回 - 两个雨刷同时回到起始角度
        StartCoroutine(RotateWindshield(windOne, startRoation, 0.5f));
        StartCoroutine(RotateWindshield(windTwo, startRoation, 0.5f));
        yield return new WaitForSeconds(0.5f);
        
        // 第二次转动 - 两个雨刷同时转动到结束角度
        StartCoroutine(RotateWindshield(windOne, endRoation, 0.5f));
        StartCoroutine(RotateWindshield(windTwo, endRoation, 0.5f));
        yield return new WaitForSeconds(0.5f);
        
        // 第二次返回 - 两个雨刷同时回到起始角度
        StartCoroutine(RotateWindshield(windOne, startRoation, 0.5f));
        StartCoroutine(RotateWindshield(windTwo, startRoation, 0.5f));
        yield return new WaitForSeconds(0.5f);
        
        isWiping = false; // 重置转动状态
    }

    private IEnumerator RotateWindshield(Transform windshield, Vector3 targetRotation, float duration)
    {
        float elapsed = 0f;
        Quaternion startRotation = windshield.rotation;
        Quaternion targetQuaternion = Quaternion.Euler(targetRotation);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            windshield.rotation = Quaternion.Slerp(startRotation, targetQuaternion, t);
            yield return null;
        }

        windshield.rotation = targetQuaternion;
    }

    private void OnDestroy()
    {
        // 在对象销毁时移除事件监听，防止内存泄漏
        MyEvent.WindshieldEvent.RemoveListener(OnWindshieldEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
