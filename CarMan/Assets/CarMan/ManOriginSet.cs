using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;


public class ManOriginSet : MonoBehaviour
{
    public Transform fatherT;
    public Transform sonT;
    public Transform targetT;
    public InputActionProperty RightSecondButton;

    [Header("跟踪模式设置")]
    [Tooltip("是否处于跟踪模式")]
    public bool isTrackingMode = false;

    [Tooltip("跟踪模式下父物体相对于目标物体的位置")]
    private Vector3 fatherRelativePosition;

    [Tooltip("跟踪模式下父物体相对于目标物体的旋转")]
    private Quaternion fatherRelativeRotation;

    /// <summary>
    /// 通过"移动/旋转父物体"，让指定子物体在世界空间达到目标位置与角度。
    /// 注意：不修改缩放。若存在非均匀缩放，结果可能有轻微误差（旋转正交化后尽量贴合）。
    /// 修改：位置完全移动到目标位置，但角度只对齐y轴，x轴和z轴的旋转保持不变。
    /// 使用组件公开字段 fatherT, sonT, targetT。
    /// </summary>
    [ContextMenu("Align Child To Target By Moving Parent")]
    [Button("wefwef")]
    public void AlignChildToTargetByMovingParent()
    {
        // 参数验证
        if (fatherT == null || sonT == null || targetT == null)
        {
            Debug.LogWarning("请先在 Inspector 中指定 fatherT / sonT / targetT");
            return;
        }
        if (!sonT.IsChildOf(fatherT))
        {
            Debug.LogError("参数错误：son 必须是 father 的子孙节点");
            return;
        }

        // 父到子的相对变换 L_pc，使得 Wc = Wp * L_pc
        Matrix4x4 L_pc = fatherT.worldToLocalMatrix * sonT.localToWorldMatrix;

        // 位置完全移动到目标位置
        Vector3 modifiedTargetPos = targetT.position;

        // 只使用目标旋转的y轴分量，保持x和z轴旋转不变
        Vector3 targetEuler = targetT.rotation.eulerAngles;
        Vector3 currentEuler = sonT.rotation.eulerAngles;
        Quaternion modifiedTargetRot = Quaternion.Euler(currentEuler.x, targetEuler.y, currentEuler.z);

        // 期望的子物体世界矩阵 D（保持当前世界缩放，目标位置与角度）
        Matrix4x4 D = Matrix4x4.TRS(modifiedTargetPos, modifiedTargetRot, sonT.lossyScale);

        // 由 D = Wp' * L_pc 推出 Wp' = D * inverse(L_pc)
        Matrix4x4 WpPrime = D * L_pc.inverse;

        // 从矩阵中提取位置与旋转（去除缩放的影响）- 内联 ExtractTR_NoScale 逻辑
        Vector3 newPos = WpPrime.GetColumn(3);
        Vector3 up = WpPrime.GetColumn(1);
        Vector3 forward = WpPrime.GetColumn(2);

        // 正交化，保证旋转为单位正交基
        Vector3 f = forward.normalized;
        Vector3 u = (up - Vector3.Dot(up, f) * f).normalized; // 去除与 forward 的投影
        if (u.sqrMagnitude < 1e-6f)
        {
            // 当 up 与 forward 接近平行时，退化处理
            u = Vector3.up;
            if (Mathf.Abs(Vector3.Dot(u, f)) > 0.99f)
                u = Vector3.right; // 再次避免平行
            u = (u - Vector3.Dot(u, f) * f).normalized;
        }
        // 可由 u 与 f 推出 r，但 Quaternion.LookRotation 会自行构造正交基
        Quaternion newRot = Quaternion.LookRotation(f, u);

        fatherT.SetPositionAndRotation(newPos, newRot);
    }

    /// <summary>
    /// 进入跟踪模式：计算父物体与目标物体之间的相对位置和角度，并开始跟踪
    /// </summary>
    [ContextMenu("Enter Tracking Mode")]
    [Button("进入跟踪模式")]
    public void EnterTrackingMode()
    {
        // 参数验证
        if (fatherT == null || sonT == null || targetT == null)
        {
            Debug.LogWarning("请先在 Inspector 中指定 fatherT / sonT / targetT");
            return;
        }
        if (!sonT.IsChildOf(fatherT))
        {
            Debug.LogError("参数错误：son 必须是 father 的子孙节点");
            return;
        }

        // 计算父物体相对于目标物体的位置和旋转
        fatherRelativePosition = fatherT.position - targetT.position;
        fatherRelativeRotation = Quaternion.Inverse(targetT.rotation) * fatherT.rotation;

        isTrackingMode = true;
        Debug.Log("已进入跟踪模式");
    }

    /// <summary>
    /// 退出跟踪模式：停止跟踪目标物体
    /// </summary>
    [ContextMenu("Exit Tracking Mode")]
    [Button("退出跟踪模式")]
    public void ExitTrackingMode()
    {
        isTrackingMode = false;
        Debug.Log("已退出跟踪模式");
    }

    /// <summary>
    /// 切换跟踪模式：如果当前在跟踪模式则退出，否则进入跟踪模式
    /// </summary>
    [ContextMenu("Toggle Tracking Mode")]
    [Button("切换跟踪模式")]
    public void ToggleTrackingMode()
    {
        if (isTrackingMode)
        {
            ExitTrackingMode();
        }
        else
        {
            EnterTrackingMode();
        }
    }


    void Start()
    {
        RightSecondButton.action.Enable();
    }
    void Update()
    {
        // 如果处于跟踪模式，更新父物体的位置和旋转以保持与目标物体的相对关系
        if (isTrackingMode && fatherT != null && sonT != null && targetT != null)
        {
            // 计算父物体应该到达的世界位置和旋转
            Vector3 targetFatherPosition = targetT.position + targetT.rotation * fatherRelativePosition;
            Quaternion targetFatherRotation = targetT.rotation * fatherRelativeRotation;

            // 直接设置父物体的位置和旋转
            fatherT.SetPositionAndRotation(targetFatherPosition, targetFatherRotation);
        }

        // // 空格键测试功能：退出跟踪模式 -> 对齐 -> 重新进入跟踪模式
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     ExitTrackingMode();
        //     AlignChildToTargetByMovingParent();
        //     EnterTrackingMode();
        // }

        //input
        // 使用现成的方法检测按钮按下的瞬间
        if (RightSecondButton.action.WasPressedThisFrame())
        {
            ExitTrackingMode();
            AlignChildToTargetByMovingParent();
            EnterTrackingMode();
        }
    }
}
