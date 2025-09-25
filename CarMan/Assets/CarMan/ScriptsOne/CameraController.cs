using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        // 注册相机黑屏模式事件监听
        MyEvent.CameraBlackModeEvent.AddListener(SetCameraToBlackMode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        // 移除相机黑屏模式事件监听
        MyEvent.CameraBlackModeEvent.RemoveListener(SetCameraToBlackMode);
    }

    /// <summary>
    /// 设置相机视图为全黑模式
    /// 1. 可视图层设置为Nothing（什么都看不到）
    /// 2. 背景设置为纯色块
    /// 3. 背景颜色设置为黑色
    /// </summary>
    [Button("SetCameraToBlackMode")]
    public void SetCameraToBlackMode()
    {
        if (mainCamera != null)
        {
            // 设置可视图层为只能看到第13层
            mainCamera.cullingMask = 1 << 13;
            
            // 设置清除标志为纯色
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            
            // 设置背景颜色为黑色
            mainCamera.backgroundColor = Color.black;
        }
        
        // 启动协程，等待2秒后触发文字渐变事件
        StartCoroutine(DelayedTextFadeIn());
    }

    // 协程：延迟2秒后触发文字渐变事件
    private IEnumerator DelayedTextFadeIn()
    {
        yield return new WaitForSeconds(2f);
        MyEvent.TextFadeInEvent.Invoke();
    }
}
