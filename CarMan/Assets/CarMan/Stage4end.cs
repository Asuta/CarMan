using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4end : MonoBehaviour
{
    public EyeBlackStageTwo  eyeBlackStageTwo;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(12f);
        eyeBlackStageTwo.StartFadeInSequence();
    }
}
