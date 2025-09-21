using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicines : MonoBehaviour
{
    public Transform targetA;
    public Transform targetB;
    
    private bool isWaitingForMedicine = false;
    private bool medicineTaken = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 检查targetA和targetB是否已设置
        if (targetA != null && targetB != null)
        {
            // 计算两个目标之间的距离
            float distance = Vector3.Distance(targetA.position, targetB.position);
            
            // 如果距离大于0.5且没有在等待吃药，并且药还没有被吃掉，则触发延迟吃药事件
            if (distance > 0.5f && !isWaitingForMedicine && !medicineTaken)
            {
                StartCoroutine(DelayedEatMedicine());
            }
        }
    }
    
    IEnumerator DelayedEatMedicine()
    {
        isWaitingForMedicine = true;
        yield return new WaitForSeconds(3f);
        MyEvent.EatMedicineEvent.Invoke();
        medicineTaken = true; // 标记药物已被服用
        isWaitingForMedicine = false;
    }
}
