using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunVisorStageThree : MonoBehaviour
{
    public Transform target;
    private bool hasLogged = false; // 记录是否已经输出过日志
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // 获取目标物体的本地欧拉角的X轴
            float xRotation = target.localEulerAngles.x;
            
            // 将欧拉角转换为-180到180度的范围
            if (xRotation > 180f)
            {
                xRotation -= 360f;
            }
            
            // 检查X轴旋转是否大于0度且尚未输出过日志
            if (xRotation > 88f && !hasLogged)
            {
                Debug.Log("sun 角度大于0了 " + xRotation);
                hasLogged = true; // 标记为已输出
                MyEvent.SunVisorEventStageThree.Invoke();
            }
        }
    }
}
