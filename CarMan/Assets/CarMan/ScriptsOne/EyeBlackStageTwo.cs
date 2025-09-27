using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VInspector;

public class EyeBlackStageTwo : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Camera camera;
    public TextMeshPro textMeshPro;
    public TextMeshPro textMeshPro2;

    // Start is called before the first frame update
    void Start()
    {
        // 初始时设置精灵渲染器 alpha 为 1（完全显示）
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
        }

        // 初始时设置文字 alpha 为 1（完全显示）
        if (textMeshPro != null)
        {
            Color textColor = textMeshPro.color;
            textColor.a = 1f;
            textMeshPro.color = textColor;
        }

        // 初始时设置文字2 alpha 为 0（完全透明）
        if (textMeshPro2 != null)
        {
            Color textColor = textMeshPro2.color;
            textColor.a = 0f;
            textMeshPro2.color = textColor;
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

        // 启动协程：3秒后文字渐隐，文字渐隐完成后精灵渐隐
        StartCoroutine(TextAndSpriteSequence());

        // 监听 MoveToSuspendPointEventStageTwoEnd 事件
        MyEvent.MoveToSuspendPointEventStageTwoEnd.AddListener(OnMoveToSuspendPointEventStageTwoEnd);
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


    // 文字和精灵序列协程：3秒后文字渐隐，文字渐隐完成后精灵渐隐
    private IEnumerator TextAndSpriteSequence()
    {
        // 3秒后文字渐隐（2秒内alpha从1到0）
        yield return new WaitForSeconds(3f);
        if (textMeshPro != null)
        {
            yield return StartCoroutine(FadeOutTextCoroutine());
        }

        // 文字渐隐完成后，设置相机可以看到所有图层
        if (camera != null)
        {
            camera.cullingMask = -1; // -1 表示所有图层
        }

        // 然后精灵渐隐（2秒内alpha从1到0）
        if (spriteRenderer != null)
        {
            yield return StartCoroutine(FadeOutCoroutine());
        }
    }

    // 文字渐隐协程：从 1 到 0 渐变 alpha 值
    private IEnumerator FadeOutTextCoroutine()
    {
        if (textMeshPro == null) yield break;

        float duration = 2.0f; // 渐变持续时间（秒）
        float elapsedTime = 0f;
        Color color = textMeshPro.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            textMeshPro.color = color;
            yield return null;
        }

        // 确保最终 alpha 值为 0
        color.a = 0f;
        textMeshPro.color = color;
    }

    // 新方法：精灵渐显并设置相机只能看到Text图层，同时二号文字渐显
    [Button("StartFadeInSequence")]
    public void StartFadeInSequence()
    {
        // 启动协程执行渐显序列
        StartCoroutine(FadeInSequenceCoroutine());
    }

    // 渐显序列协程
    private IEnumerator FadeInSequenceCoroutine()
    {
        // 首先将精灵渲染器的Alpha值从0渐变到1
        if (spriteRenderer != null)
        {
            yield return StartCoroutine(FadeInSpriteCoroutine());
        }

        // 设置相机只能看到Text图层
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
        }

        // 同时开启协程将二号文字的Alpha值从0渐变到1
        if (textMeshPro2 != null)
        {
            StartCoroutine(FadeInText2Coroutine());
        }
    }

    // 精灵渐显协程：从 0 到 1 渐变 alpha 值
    private IEnumerator FadeInSpriteCoroutine()
    {
        if (spriteRenderer == null) yield break;

        float duration = 2.0f; // 渐变持续时间（秒）
        float elapsedTime = 0f;
        Color color = spriteRenderer.color;
        color.a = 0f; // 初始alpha为0
        spriteRenderer.color = color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            spriteRenderer.color = color;
            yield return null;
        }

        // 确保最终 alpha 值为 1
        color.a = 1f;
        spriteRenderer.color = color;
    }

    // 二号文字渐显协程：从 0 到 1 渐变 alpha 值
    private IEnumerator FadeInText2Coroutine()
    {
        if (textMeshPro2 == null) yield break;

        float duration = 2.0f; // 渐变持续时间（秒）
        float elapsedTime = 0f;
        Color color = textMeshPro2.color;
        color.a = 0f; // 初始alpha为0
        textMeshPro2.color = color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            textMeshPro2.color = color;
            yield return null;
        }

        // 确保最终 alpha 值为 1
        color.a = 1f;
        textMeshPro2.color = color;
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

    // 当对象被销毁时移除事件监听
    void OnDestroy()
    {
        MyEvent.MoveToSuspendPointEventStageTwoEnd.RemoveListener(OnMoveToSuspendPointEventStageTwoEnd);
    }

    // MoveToSuspendPointEventStageTwoEnd 事件处理
    private void OnMoveToSuspendPointEventStageTwoEnd()
    {
        // 启动协程：等待5秒后执行渐显序列
        StartCoroutine(DelayedStartFadeInSequence());
    }

    // 延迟执行渐显序列协程
    private IEnumerator DelayedStartFadeInSequence()
    {
        // 等待5秒
        yield return new WaitForSeconds(5f);
        
        // 执行渐显序列
        StartFadeInSequence();
    }
}
