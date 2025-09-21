using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    // 向上飘动的速度
    public float floatSpeed = 2.0f;
    
    // 生命周期时间（秒）
    public float lifeTime = 5.0f;
    
    // 计时器
    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        // 初始化计时器
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // 让云朵向上飘动
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        
        // 更新计时器
        timer += Time.deltaTime;
        
        // 如果超过生命周期，销毁云朵
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
