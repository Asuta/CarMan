using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Transform pointA;
    public float moveDuration = 2.0f; // 移动持续时间（秒），可在编辑器中调整
    // Start is called before the first frame update
    void Start()
    {
        // 监听吃药事件
        MyEvent.EatMedicineEvent.AddListener(OnEatMedicine);
    }

    // Update is called once per frame
    void Update()
    {
        //test
        // 检测是否按下了A键
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("按下A键，触发吃药事件");
            // 触发吃药事件
            MyEvent.EatMedicineEvent.Invoke();
        }
    }
    
    // 处理吃药事件的方法
    private void OnEatMedicine()
    {
        Debug.Log("吃药事件被触发了！车辆正在移动到point A位置...");
        // 开始移动到point A的协程
        StartCoroutine(MoveToPointA());
    }
    
    // 移动到point A的协程
    private IEnumerator MoveToPointA()
    {
        // 检查pointA是否已设置
        if (pointA != null)
        {
            // 使用公共变量中的移动持续时间
            float elapsedTime = 0f;
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = pointA.position;
            
            while (elapsedTime < moveDuration)
            {
                // 计算移动进度
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / moveDuration;
                
                // 使用线性插值平滑移动
                transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
                
                yield return null;
            }
            
            // 确保最终位置准确
            transform.position = targetPosition;
            Debug.Log("车辆已到达point A位置！");
        }
        else
        {
            Debug.LogWarning("Point A未设置，无法移动车辆！");
        }
    }
    
    // 在对象销毁时移除事件监听，避免内存泄漏
    private void OnDestroy()
    {
        MyEvent.EatMedicineEvent.RemoveListener(OnEatMedicine);
    }
}
