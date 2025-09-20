using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;


public class ManOriginSet : MonoBehaviour
{
    public Transform fatherT;
    public Transform sonT;
    public Transform targetT;

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
}
