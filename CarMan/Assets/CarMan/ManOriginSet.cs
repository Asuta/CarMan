using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManOriginSet : MonoBehaviour
{
    public Transform fatherT;
    public Transform sonT;
    public Transform targetT;
    public bool isFollowing;

    // 用于存储跟随时的相对位置和角度
    private Vector3 initialRelativePosition;
    private Quaternion initialRelativeRotation;
    private bool isFollowingInitialized = false;



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

    /// <summary>
    /// 移动子物体到目标位置并更新相对位置和角度数据
    /// 该方法会先清除之前存储的相对位置和角度值，然后移动子物体到目标位置，
    /// 最后重新计算并存储新的相对位置和角度值，供父物体跟随功能使用。
    /// </summary>
    [ContextMenu("Move Child And Update Relative Transform")]
    public void MoveChildAndUpdateRelativeTransform()
    {
        if (fatherT == null || sonT == null || targetT == null)
        {
            Debug.LogWarning("请先在 Inspector 中指定 fatherT / sonT / targetT");
            return;
        }

        // 清除之前存储的相对位置和角度值
        isFollowingInitialized = false;

        // 使用AlignChildToTargetByMovingParent方法移动子物体到目标位置
        AlignChildToTargetByMovingParent(fatherT, sonT, targetT.position, targetT.rotation);

        // 重新计算并存储新的相对位置和角度值
        initialRelativePosition = Quaternion.Inverse(targetT.rotation) * (sonT.position - targetT.position);
        initialRelativeRotation = Quaternion.Inverse(targetT.rotation) * sonT.rotation;
        isFollowingInitialized = true;
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            ResetAll();   
        }

        // 如果开启了跟随模式
        if (isFollowing)
        {
            // 检查必要的Transform是否已设置
            if (fatherT == null || sonT == null || targetT == null)
            {
                Debug.LogWarning("请先在 Inspector 中指定 fatherT / sonT / targetT");
                return;
            }

            // 如果是第一次进入跟随模式，记录初始的相对位置和角度
            if (!isFollowingInitialized)
            {
                // 计算子物体相对于目标的位置和旋转
                initialRelativePosition = Quaternion.Inverse(targetT.rotation) * (sonT.position - targetT.position);
                initialRelativeRotation = Quaternion.Inverse(targetT.rotation) * sonT.rotation;
                isFollowingInitialized = true;
            }

            // 根据记录的相对位置和角度，计算子物体应该在的位置和旋转
            Vector3 targetPosition = targetT.position + targetT.rotation * initialRelativePosition;
            Quaternion targetRotation = targetT.rotation * initialRelativeRotation;

            // 直接将计算出的位置和旋转赋值给父物体
            fatherT.position = targetPosition;
            // fatherT.rotation = targetRotation;
        }
        else
        {
            // 如果关闭了跟随模式，重置初始化标志
            isFollowingInitialized = false;
        }
    }

    private void ResetAll()
    {

        isFollowing = false;
        // 3. 执行对齐子物体操作（只对齐Y轴，保持X轴和Z轴不变）
        AlignChildToTargetByMovingParent();
        isFollowing = true;
        

    }
}
