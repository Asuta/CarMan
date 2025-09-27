using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLight : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Material materialRed;
    public Material materialGreen;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        MyEvent.MoveToSuspendPointEventStageThree.AddListener(OnMoveToSuspendPointEventStageThree);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMoveToSuspendPointEventStageThree()
    {
        meshRenderer.material = materialRed;
        StartCoroutine(ChangeToGreenAfterDelay());
    }

    IEnumerator ChangeToGreenAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("哈哈哈");
        meshRenderer.material = materialGreen;
        MyEvent.lightToGreenEvent.Invoke();
    }

    void OnDestroy()
    {
        MyEvent.MoveToSuspendPointEventStageThree.RemoveListener(OnMoveToSuspendPointEventStageThree);
    }
}
