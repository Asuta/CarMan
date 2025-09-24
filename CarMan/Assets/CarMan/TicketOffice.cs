using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketOffice : MonoBehaviour
{
    public int ticketCount = 0;
    public RodAxis rodAxis;
    public bool isManAwake = false; // 男人是否醒着
    private bool hasOpened = false; // 标记是否已经触发过打开

    // Start is called before the first frame update
    void Start()
    {
        MyEvent.AddCoinEvent.AddListener(AddTicket);
        MyEvent.WakeUpManEvent.AddListener(OnManWokeUp);
    }

    private void AddTicket()
    {
        ticketCount++;
        Debug.LogError("Ticket count: " + ticketCount);
        
        // 当票数达到5时触发硬币集齐事件
        if (ticketCount >= 5)
        {
            MyEvent.CoinCollectedEvent.Invoke();
            Debug.LogError("Coin collected event triggered!");
        }
        
        // 当票数达到5且尚未触发过打开时，触发RodAxis的Open方法（不再检查男人是否醒着）
        if (ticketCount >= 5 && !hasOpened && rodAxis != null)
        {
            rodAxis.Open();
            hasOpened = true;
            Debug.LogError("Ticket count reached 5! Opening rod axis.");
            
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

    /// <summary>
    /// 处理男人醒来事件
    /// </summary>
    private void OnManWokeUp()
    {
        SetManAwake(true);
        Debug.LogError("Man woke up from event!");
    }
    
    /// <summary>
    /// 设置男人醒着的状态
    /// </summary>
    /// <param name="awake">是否醒着</param>
    public void SetManAwake(bool awake)
    {
        isManAwake = awake;
        Debug.LogError("Man awake state: " + isManAwake);
    }
    // 在对象销毁时移除事件监听器
    private void OnDestroy()
    {
        MyEvent.AddCoinEvent.RemoveListener(AddTicket);
        MyEvent.WakeUpManEvent.RemoveListener(OnManWokeUp);
    }
    
    /// <summary>
    /// 等待3秒后触发眼睛渐变事件
    /// </summary>
    private IEnumerator TriggerEyeBlackFadeInAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        MyEvent.EyeBlackFadeInEvent.Invoke();
        Debug.LogError("Eye black fade in event triggered after 3 seconds!");
    }
    
    /// <summary>
    /// 等待5秒后触发栏杆完全打开事件
    /// </summary>
    private IEnumerator TriggerRodAxisFullyOpenedAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        MyEvent.RodAxisFullyOpenedEvent.Invoke();
        Debug.LogError("Rod axis fully opened event triggered after 5 seconds!");
    }
}
