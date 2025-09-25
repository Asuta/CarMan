using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class TicketOfficeStop : MonoBehaviour
{
    public RodAxis rodAxis;

    void Start()
    {
        
        MyEvent.CupBreakEvent.AddListener(OpenRodAxis);
    }

    [Button]
    public void OpenRodAxis()
    {
        StartCoroutine(OpenRodAxisWithDelay());
    }

    private IEnumerator OpenRodAxisWithDelay()
    {
        yield return new WaitForSeconds(5f);
        rodAxis.Open();
        yield return new WaitForSeconds(5f);
        MyEvent.MoveContinueEvent.Invoke();
    }
}
