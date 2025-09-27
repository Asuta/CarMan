using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class CarDoor : MonoBehaviour
{
    public Transform rotateTarget;
    Vector3 rotateA = new Vector3(0, 0, 0);
    Vector3 rotateB = new Vector3(0, 90, 0);
    // Start is called before the first frame update
    void Start()
    {
        // 监听移动到终点事件
        MyEvent.MoveToSuspendPointEventStageTwoEnd.AddListener(OnMoveToEndPoint);
    }

    // 移动到终点事件处理方法
    private void OnMoveToEndPoint()
    {
        Debug.Log("接收到移动到终点事件，开始旋转车门");
        RotateToB(2.0f); // 旋转到角度B，持续2秒
    }

    /// <summary>
    /// 从角度A匀速旋转到角度B
    /// </summary>
    /// <param name="duration">旋转持续时间（秒）</param>
    [Button("RotateToB")]
    public void RotateToB(float duration = 1.0f)
    {
        if (rotateTarget != null)
        {
            StartCoroutine(RotateSmoothly(rotateA, rotateB, duration));
        }
    }

    /// <summary>
    /// 从角度B匀速旋转到角度A
    /// </summary>
    /// <param name="duration">旋转持续时间（秒）</param>
    public void RotateToA(float duration = 1.0f)
    {
        if (rotateTarget != null)
        {
            StartCoroutine(RotateSmoothly(rotateB, rotateA, duration));
        }
    }

    /// <summary>
    /// 平滑旋转协程
    /// </summary>
    private IEnumerator RotateSmoothly(Vector3 fromAngle, Vector3 toAngle, float duration)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = Quaternion.Euler(fromAngle);
        Quaternion targetRotation = Quaternion.Euler(toAngle);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            rotateTarget.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        // 确保最终精确到达目标角度
        rotateTarget.rotation = targetRotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 在对象销毁时移除事件监听，避免内存泄漏
    private void OnDestroy()
    {
        MyEvent.MoveToSuspendPointEventStageTwoEnd.RemoveListener(OnMoveToEndPoint);
    }
}
