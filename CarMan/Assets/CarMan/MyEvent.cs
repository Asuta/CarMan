using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public static class MyEvent
{
    public static UnityEvent LoadEvent = new UnityEvent();
    public static UnityEvent SystemStartEvent = new UnityEvent();
}






// MyEvent.LoadEvent.AddListener(SetOriginAsset);

// MyEvent.LoadEvent.RemoveListener(SetOriginAsset);