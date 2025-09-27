using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PointPair
{
    public Transform pointA;
    public Transform pointB;
}
public static class MyEvent
{
    //stage One
    public static UnityEvent LoadEvent = new UnityEvent(); // 加载事件
    public static UnityEvent SystemStartEvent = new UnityEvent(); // 系统启动事件
    public static UnityEvent WindshieldEvent = new UnityEvent(); // 挡风玻璃事件
    public static UnityEvent HonkingEvent = new UnityEvent(); // 鸣笛事件
    public static UnityEvent EatMedicineEvent = new UnityEvent(); // 吃药事件
    public static UnityEvent AddCoinEvent = new UnityEvent(); // 添加硬币事件
    public static UnityEvent CoinCollectedEvent = new UnityEvent(); // 硬币集齐事件

    public static UnityEvent EyeBlackFadeInEvent = new UnityEvent(); // 黑眼睛渐变显示事件
    public static UnityEvent CameraBlackModeEvent = new UnityEvent(); // 相机黑屏模式事件
    public static UnityEvent TextFadeInEvent = new UnityEvent(); // 文字渐变显示事件
    public static UnityEvent ArriveAtPointAEvent = new UnityEvent(); // 到达point A位置事件
    public static UnityEvent RodAxisFullyOpenedEvent = new UnityEvent(); // 栏杆完全打开事件
    public static UnityEvent MoveToSuspendPointEvent = new UnityEvent(); // 移动到终点事件
    public static UnityEvent StopRodAxisFullyOpenedEvent = new UnityEvent(); // 停车厂栏杆完全打开事件
    public static UnityEvent CupBreakEvent = new UnityEvent(); // 杯子碎了事件
    public static UnityEvent MoveContinueEvent = new UnityEvent(); // 继续移动事件
    public static UnityEvent MoveToEndPointEvent = new UnityEvent(); // 移动到终点事件

    //stage Two
    public static UnityEvent TextFadeInEventTwo = new UnityEvent(); // 文字渐变显示事件
    public static UnityEvent TextFadeOutEventTwo = new UnityEvent(); // 文字渐变隐藏事件
    public static UnityEvent SystemStartEventTwo = new UnityEvent(); // 系统启动事件
    public static UnityEvent MoveToSuspendPointEventStageTwo = new UnityEvent(); // 移动到终点事件
    public static UnityEvent ReleaseHunbergerEventStageTwo = new UnityEvent(); // 释放汉堡事件
    public static UnityEvent MoveToSuspendPointEventStageTwoB = new UnityEvent(); // 移动到点B事件
    public static UnityEvent MoveToSuspendPointEventStageTwoC = new UnityEvent(); // 移动到点C事件
}






// MyEvent.LoadEvent.AddListener(SetOriginAsset);

// MyEvent.LoadEvent.RemoveListener(SetOriginAsset);