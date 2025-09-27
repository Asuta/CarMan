using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EyeBlackStageTwo : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Camera camera;
    public TextMeshPro textMeshPro;
    
    // Start is called before the first frame update
    void Start()
    {
        // 初始时关闭精灵渲染器
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
        
        // 设置相机只显示 Text 图层，并设置背景色为黑色
        if (camera != null)
        {
            int textLayer = LayerMask.NameToLayer("Text");
            if (textLayer != -1)
            {
                camera.cullingMask = 1 << textLayer;
            }
            else
            {
                Debug.LogWarning("Text 图层不存在，相机将看不到任何内容");
                camera.cullingMask = 0;
            }
            
            // 设置相机背景色为黑色
            camera.backgroundColor = Color.black;
        }
        
        // 启动协程：3秒后关闭文字，5秒后开启精灵渲染器并恢复相机设置
        StartCoroutine(TextAndCameraSequence());
    }

    // 延迟后渐隐协程：等待5秒后从1渐变到0
    private IEnumerator FadeOutAfterDelay()
    {
        // 等待5秒
        yield return new WaitForSeconds(5f);
        
        // 开始渐隐
        yield return StartCoroutine(FadeOutCoroutine());
    }

    // 渐隐协程：从 1 到 0 渐变 alpha 值
    private IEnumerator FadeOutCoroutine()
    {
        if (spriteRenderer == null) yield break;
        
        float duration = 2.0f; // 渐变持续时间（秒）
        float elapsedTime = 0f;
        Color color = spriteRenderer.color;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            spriteRenderer.color = color;
            yield return null;
        }
        
        // 确保最终 alpha 值为 0
        color.a = 0f;
        spriteRenderer.color = color;
    }


    // 文字和相机序列协程：3秒后关闭文字，5秒后开启精灵渲染器并恢复相机设置
    private IEnumerator TextAndCameraSequence()
    {
        // 3秒后关闭文字
        yield return new WaitForSeconds(3f);
        if (textMeshPro != null)
        {
            textMeshPro.enabled = false;
        }

        // 5秒后开启精灵渲染器并恢复相机设置
        yield return new WaitForSeconds(2f); // 总共等待5秒
        
        // 开启精灵渲染器
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
        
        // 恢复相机设置：可以看到所有图层，背景色改为天空盒
        if (camera != null)
        {
            camera.cullingMask = -1; // -1 表示所有图层
            camera.clearFlags = CameraClearFlags.Skybox; // 使用天空盒作为背景
        }
    }

    // Update is called once per frame
    void Update()
    {
        // //test
        // if(Input.GetKeyDown(KeyCode.B))
        // {
        //     MyEvent.EyeBlackFadeInEvent.Invoke();
        // }
    }
}
