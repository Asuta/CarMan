using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//开启雨刷器按钮的代码
public class WindButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnWindButton()
    {
        MyEvent.WindshieldEvent.Invoke();
    }
}
