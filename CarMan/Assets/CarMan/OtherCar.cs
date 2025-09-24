using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherCar : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject otherCarModel;
    // Start is called before the first frame update
    void Start()
    {
        // 监听到达point A位置事件
        MyEvent.ArriveAtPointAEvent.AddListener(OnArriveAtPointA);
    }

    // 处理到达point A位置事件的方法
    private void OnArriveAtPointA()
    {
        Debug.Log("hahaha");
        otherCarModel.SetActive(true);
        // 启动协程，等待5秒后播放音频
        StartCoroutine(PlayAudioAfterDelay());
    }

    // 协程：等待5秒后播放音频
    private IEnumerator PlayAudioAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 在对象销毁时移除事件监听，避免内存泄漏
    private void OnDestroy()
    {
        MyEvent.ArriveAtPointAEvent.RemoveListener(OnArriveAtPointA);
    }
}
