using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketOffice : MonoBehaviour
{
    public int ticketCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        MyEvent.AddCoinEvent.AddListener(AddTicket);
    }

    private void AddTicket()
    {
        ticketCount++;
        Debug.LogError("Ticket count: " + ticketCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
