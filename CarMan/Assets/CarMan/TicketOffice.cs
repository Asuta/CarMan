using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.LogError("Ticket count: " + ticketCount);
        
        // 当票数达到5且尚未触发过打开时，触发RodAxis的Open方法
        if (ticketCount >= 5 && !hasOpened && rodAxis != null)
        {
            rodAxis.Open();
            hasOpened = true;
            Debug.LogError("Ticket count reached 5! Opening rod axis.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
