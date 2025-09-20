using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackCarSystem : MonoBehaviour
{
    public SpriteRenderer LightSystem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeOutLightSystem()
    {
        float duration = 2.0f; // 渐变持续时间（秒）
        float currentTime = 0f;
        
        // 获取当前颜色
        Color startColor = LightSystem.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float progress = currentTime / duration;
            
            // 线性插值计算当前alpha值
            LightSystem.color = Color.Lerp(startColor, endColor, progress);
            
            yield return null;
        }
        
        // 确保最终alpha值为0
        LightSystem.color = endColor;
    }

    void OnCollisionEnter(Collision collision)
    {
        //log name of the layer
        Debug.Log(collision.gameObject.layer);
        //if layer = 8
        if(collision.gameObject.layer == 8)
        {
            StartCoroutine(FadeOutLightSystem());
        }
    }
}
