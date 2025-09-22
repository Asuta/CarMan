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
        
        // 当票数达到5、男人醒着且尚未触发过打开时，触发RodAxis的Open方法
        if (ticketCount >= 5 && isManAwake && !hasOpened && rodAxis != null)
        {
            rodAxis.Open();
            hasOpened = true;
            Debug.LogError("Ticket count reached 5 and man is awake! Opening rod axis.");
            
            // 启动协程，等待3秒后触发眼睛渐变事件
            StartCoroutine(TriggerEyeBlackFadeInAfterDelay());
        }
        else if (ticketCount >= 5 && !isManAwake && !hasOpened)
        {
            Debug.LogError("Ticket count reached 5, but man is not awake yet. Waiting...");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 持续检查条件，以防男人在硬币集齐后才醒来
        if (ticketCount >= 5 && isManAwake && !hasOpened && rodAxis != null)
        {
            rodAxis.Open();
            hasOpened = true;
            Debug.LogError("Conditions met! Opening rod axis.");
            
            // 启动协程，等待3秒后触发眼睛渐变事件
            StartCoroutine(TriggerEyeBlackFadeInAfterDelay());
        }
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
}
