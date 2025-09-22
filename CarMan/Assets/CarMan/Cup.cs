using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    public bool isCanBeGrab = false;
    // Start is called before the first frame update
    void Start()
    {
        // 订阅硬币集齐事件
        MyEvent.CoinCollectedEvent.AddListener(OnCoinCollected);
    }
    
    // 硬币集齐事件处理函数
    private void OnCoinCollected()
    {
        isCanBeGrab = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGrabObject()
    {
        if(isCanBeGrab)
        {
            DisableKinematic();
        }
    }



    /// <summary>
    /// 关闭刚体的运动学属性
    /// </summary>
    public void DisableKinematic()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            Debug.LogError("Cup kinematic disabled");
        }
        else
        {
            Debug.LogError("No Rigidbody found on Cup");
        }
    }
    
    // 在对象销毁时移除事件监听器
    private void OnDestroy()
    {
        MyEvent.CoinCollectedEvent.RemoveListener(OnCoinCollected);
    }
}
