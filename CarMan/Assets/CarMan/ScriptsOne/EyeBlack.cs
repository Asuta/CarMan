using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBlack : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        // 注册事件监听器
        MyEvent.EyeBlackFadeInEvent.AddListener(OnEyeBlackFadeIn);
        
        // 初始时设置 alpha 为 0
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 0f;
            spriteRenderer.color = color;
        }
    }

    // 事件触发时的处理函数
    private void OnEyeBlackFadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    // 渐变协程：从 0 到 1 渐变 alpha 值
    private IEnumerator FadeInCoroutine()
    {
        if (spriteRenderer == null) yield break;
        
        float duration = 2.0f; // 渐变持续时间（秒）
        float elapsedTime = 0f;
        Color color = spriteRenderer.color;
        
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
        
        // 渐变完成后触发相机黑屏模式事件
        MyEvent.CameraBlackModeEvent.Invoke();
    }

    // 清理事件监听器
    private void OnDestroy()
    {
        MyEvent.EyeBlackFadeInEvent.RemoveListener(OnEyeBlackFadeIn);
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
