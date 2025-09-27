using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandStageThree : MonoBehaviour
{
    public GameObject handBagel1;
    public GameObject handBagel2;
    // Start is called before the first frame update
    void Start()
    {
        MyEvent.SystemStartEventStageThree.AddListener(OnSystemStartEventTriggered);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSystemStartEventTriggered()
    {
        handBagel1.SetActive(true);
        handBagel2.SetActive(true);
    }

    private void OnDestroy()
    {
        MyEvent.SystemStartEventStageThree.RemoveListener(OnSystemStartEventTriggered);
    }
}
