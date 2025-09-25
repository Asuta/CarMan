using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VInspector;

public class TextFadeOutTwo : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    public float fadeDuration = 2.0f; // 渐变持续时间
    
    // Start is called before the first frame update
    void Start()
    {
        // 初始化时将字体阿尔法值设为0（完全透明）
        if (textMeshPro != null)
        {
            Color originalColor = textMeshPro.color;
            textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }
        
        // 注册文字渐变事件监听
        MyEvent.TextFadeInEventTwo.AddListener(StartFadeIn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        // 移除文字渐变事件监听
        MyEvent.TextFadeInEvent.RemoveListener(StartFadeIn);
    }

    // 启动字体渐变效果（从0到1）
    [Button("StartFadeIn")]
    public void StartFadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    // 协程：字体阿尔法值从0渐变到1
    private IEnumerator FadeInCoroutine()
    {
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro reference is not set!");
            yield break;
        }

        // 保存原始颜色
        Color originalColor = textMeshPro.color;
        float elapsedTime = 0f;

        // 设置初始透明度为0
        textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // 渐变过程：从0到1
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // 确保最终透明度为1
        textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        // 停留3秒钟
        yield return new WaitForSeconds(3f);

        // 开始淡出效果：从1到0
        StartCoroutine(FadeOutCoroutine());
    }

    // 协程：字体阿尔法值从1渐变到0
    private IEnumerator FadeOutCoroutine()
    {
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro reference is not set!");
            yield break;
        }

        // 保存原始颜色
        Color originalColor = textMeshPro.color;
        float elapsedTime = 0f;

        // 渐变过程：从1到0
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // 确保最终透明度为0
        textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        MyEvent.TextFadeOutEventTwo.Invoke();
    }
}
