using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class Hunberger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button("OnGrabReleased")]
    public void OnGrabReleased()
    {
        MyEvent.ReleaseHunbergerEventStageTwo.Invoke();
    }
}
