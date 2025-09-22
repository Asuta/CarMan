using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public static class MyEvent
{
    public static UnityEvent LoadEvent = new UnityEvent();
    public static UnityEvent SystemStartEvent = new UnityEvent();
    public static UnityEvent WindshieldEvent = new UnityEvent();
    public static UnityEvent HonkingEvent = new UnityEvent();
    public static UnityEvent EatMedicineEvent = new UnityEvent();
    public static UnityEvent AddCoinEvent = new UnityEvent();
    public static UnityEvent CoinCollectedEvent = new UnityEvent(); // 硬币集齐事件
    public static UnityEvent WakeUpManEvent = new UnityEvent(); // 叫醒男人事件
    public static UnityEvent EyeBlackFadeInEvent = new UnityEvent(); // 黑眼睛渐变显示事件
    public static UnityEvent CameraBlackModeEvent = new UnityEvent(); // 相机黑屏模式事件
    public static UnityEvent TextFadeInEvent = new UnityEvent(); // 文字渐变显示事件
}






// MyEvent.LoadEvent.AddListener(SetOriginAsset);

// MyEvent.LoadEvent.RemoveListener(SetOriginAsset);