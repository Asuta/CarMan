using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class TicketOffice : MonoBehaviour
{
    public int ticketCount = 0;
    public RodAxis rodAxis;
    private bool hasOpened = false; // 标记是否已经触发过打开

    // Start is called before the first frame update
    void Start()
    {
        MyEvent.AddCoinEvent.AddListener(AddTicket);
    }

    private void AddTicket()
    {
        ticketCount++;
        Debug.LogError("票数: " + ticketCount);
        
        // 当票数达到5时触发硬币集齐事件
        if (ticketCount >= 5)
        {
            MyEvent.CoinCollectedEvent.Invoke();
            Debug.LogError("硬币收集事件已触发!");
        }
        
        // 当票数达到5且尚未触发过打开时，触发RodAxis的Open方法（不再检查男人是否醒着）
        if (ticketCount >= 5 && !hasOpened && rodAxis != null)
        {
            rodAxis.Open();
            hasOpened = true;
            Debug.LogError("票数达到5张! 正在打开栏杆轴。");
            
            // // 启动协程，等待3秒后触发眼睛渐变事件
            // StartCoroutine(TriggerEyeBlackFadeInAfterDelay());
            
            // 启动协程，等待5秒后触发栏杆完全打开事件
            StartCoroutine(TriggerRodAxisFullyOpenedAfterDelay());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update方法中不再需要检查条件，因为触发逻辑已经在AddTicket中处理
        // 这样可以避免重复触发协程的问题
    }

    // 在对象销毁时移除事件监听器
    private void OnDestroy()
    {
        MyEvent.AddCoinEvent.RemoveListener(AddTicket);
    }
    
    /// <summary>
    /// 等待3秒后触发眼睛渐变事件
    /// </summary>
    private IEnumerator TriggerEyeBlackFadeInAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        MyEvent.EyeBlackFadeInEvent.Invoke();
        Debug.LogError("3秒后触发眼睛黑色淡入事件!");
    }
    
    /// <summary>
    /// 等待5秒后触发栏杆完全打开事件
    /// </summary>
    private IEnumerator TriggerRodAxisFullyOpenedAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        MyEvent.RodAxisFullyOpenedEvent.Invoke();
        Debug.LogError("5秒后触发栏杆轴完全打开事件!");
    }

    [Button("TriggerRodAxisFullyOpened")]
    public void TriggerRodAxisFullyOpened()
    {
        MyEvent.RodAxisFullyOpenedEvent.Invoke();
        Debug.LogError("栏杆轴完全打开事件已触发!");
    }
}
