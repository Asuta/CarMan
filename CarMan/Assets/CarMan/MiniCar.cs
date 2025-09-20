using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCar : MonoBehaviour
{
    public Transform BigCenter;
    public Transform BigTarget;
    public Transform MiniCenter;
    public Transform MiniTarget;
    public float scale = 0.03f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 检查所有必要的Transform是否都已设置
        if (BigCenter == null || BigTarget == null || MiniCenter == null || MiniTarget == null)
        {
            Debug.LogWarning("请确保所有Transform引用都已设置！");
            return;
        }

        // 计算大目标相对于大中心的位置向量
        Vector3 relativePosition = BigTarget.position - BigCenter.position;
        relativePosition = new Vector3(relativePosition.x, 0, relativePosition.z);

        // 将相对位置按缩放倍数进行缩放
        Vector3 scaledRelativePosition = relativePosition * scale;

        // 考虑小中心的旋转角度，将缩放后的相对位置转换到小中心的本地坐标系
        // 这样可以确保小目标在小中心的旋转平面内正确移动
        Vector3 rotatedRelativePosition = MiniCenter.rotation * scaledRelativePosition;

        // 计算小目标的位置：小中心位置 + 旋转后的相对位置
        MiniTarget.position = MiniCenter.position + rotatedRelativePosition;

        MiniTarget.rotation = MiniCenter.rotation;
    }
}
