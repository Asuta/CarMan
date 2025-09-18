using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManOriginSet : MonoBehaviour
{
    public Transform fatherT;
    public Transform sonT;
    public Transform targetT;


    /// <summary>
    /// 通过"移动/旋转父物体"，让指定子物体在世界空间达到目标位置与角度。
    /// 注意：不修改缩放。若存在非均匀缩放，结果可能有轻微误差（旋转正交化后尽量贴合）。
    /// 修改：位置完全移动到目标位置，但角度只对齐y轴，x轴和z轴的旋转保持不变。
    /// </summary>
    [ContextMenu("Align Child To Target By Moving Parent")]
    public static void AlignChildToTargetByMovingParent(Transform father, Transform son, Vector3 targetWorldPos, Quaternion targetWorldRot)
    {
        if (father == null || son == null)
        {
            Debug.LogWarning("father 或 son 为空");
            return;
        }
        if (!son.IsChildOf(father))
        {
            Debug.LogError("参数错误：son 必须是 father 的子孙节点");
            return;
        }

        // 父到子的相对变换 L_pc，使得 Wc = Wp * L_pc
        Matrix4x4 L_pc = father.worldToLocalMatrix * son.localToWorldMatrix;

        // 位置完全移动到目标位置
        Vector3 modifiedTargetPos = targetWorldPos;
        
        // 只使用目标旋转的y轴分量，保持x和z轴旋转不变
        Vector3 targetEuler = targetWorldRot.eulerAngles;
        Vector3 currentEuler = son.rotation.eulerAngles;
        Quaternion modifiedTargetRot = Quaternion.Euler(currentEuler.x, targetEuler.y, currentEuler.z);

        // 期望的子物体世界矩阵 D（保持当前世界缩放，目标位置与角度）
        Matrix4x4 D = Matrix4x4.TRS(modifiedTargetPos, modifiedTargetRot, son.lossyScale);

        // 由 D = Wp' * L_pc 推出 Wp' = D * inverse(L_pc)
        Matrix4x4 WpPrime = D * L_pc.inverse;

        // 从矩阵中提取位置与旋转（去除缩放的影响）
        ExtractTR_NoScale(WpPrime, out Vector3 newPos, out Quaternion newRot);

        father.SetPositionAndRotation(newPos, newRot);
    }

    /// <summary>
    /// 使用组件公开字段 fatherT, sonT, targetT 的便捷调用。
    /// </summary>
    [ContextMenu("Align Child To Target By Moving Parent")]
    public void AlignChildToTargetByMovingParent()
    {
        if (fatherT == null || sonT == null || targetT == null)
        {
            Debug.LogWarning("请先在 Inspector 中指定 fatherT / sonT / targetT");
            return;
        }
        AlignChildToTargetByMovingParent(fatherT, sonT, targetT.position, targetT.rotation);
    }

    // 从 4x4 矩阵提取位置与旋转（将旋转基向量单位化，剔除缩放影响）
    private static void ExtractTR_NoScale(in Matrix4x4 m, out Vector3 pos, out Quaternion rot)
    {
        pos = m.GetColumn(3);
        Vector3 up = m.GetColumn(1);
        Vector3 forward = m.GetColumn(2);

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
        rot = Quaternion.LookRotation(f, u);
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }
}
