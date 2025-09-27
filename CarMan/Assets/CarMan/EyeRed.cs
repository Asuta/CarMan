using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeRed : MonoBehaviour
{
    public SpriteRenderer target;
    // Start is called before the first frame update
    void Start()
    {
        MyEvent.SunVisorEventStageThree.AddListener(OnSunVisorEventStageThree);
        target.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSunVisorEventStageThree()
    {
        target.enabled = true;
        StartCoroutine(WaitAndLog());
    }

    private IEnumerator WaitAndLog()
    {
        yield return new WaitForSeconds(6f);
        Debug.Log("哈哈哈");
    }
    
    private void OnDestroy()
    {
        MyEvent.SunVisorEventStageThree.RemoveListener(OnSunVisorEventStageThree);
    }
    }
