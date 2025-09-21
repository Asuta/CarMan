using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class HandBox : MonoBehaviour
{
    public Transform axisCube;
    private float closeRotationZ = -90f;
    private float openRotationZ = -50f;
    
    private bool isRotating = false;
    private Coroutine currentRotationCoroutine;
    
    // 旋转速度参数
    public float rotationDuration = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 初始化为关闭状态
        if (axisCube != null)
        {
            Vector3 currentRotation = axisCube.localEulerAngles;
            currentRotation.z = closeRotationZ;
            axisCube.localEulerAngles = currentRotation;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button("OpenBox")]
    public void OpenBox()
    {
        if (isRotating && currentRotationCoroutine != null)
        {
            StopCoroutine(currentRotationCoroutine);
        }
        
        currentRotationCoroutine = StartCoroutine(RotateZAxis(openRotationZ));
    }

    [Button("CloseBox")]
    public void CloseBox()
    {
        if (isRotating && currentRotationCoroutine != null)
        {
            StopCoroutine(currentRotationCoroutine);
        }
        
        currentRotationCoroutine = StartCoroutine(RotateZAxis(closeRotationZ));
    }
    
    private IEnumerator RotateZAxis(float targetZ)
    {
        isRotating = true;
        
        float startZ = axisCube.localEulerAngles.z;
        float elapsedTime = 0f;
        
        while (elapsedTime < rotationDuration)
        {
            // 使用简单的缓动函数实现先快后慢的效果
            float t = elapsedTime / rotationDuration;
            t = 1f - Mathf.Pow(1f - t, 3f); // 使用三次方缓出函数
            
            // 只修改Z轴的旋转
            Vector3 currentRotation = axisCube.localEulerAngles;
            currentRotation.z = Mathf.LerpAngle(startZ, targetZ, t);
            axisCube.localEulerAngles = currentRotation;
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // 确保最终位置准确
        Vector3 finalRotation = axisCube.localEulerAngles;
        finalRotation.z = targetZ;
        axisCube.localEulerAngles = finalRotation;
        
        isRotating = false;
        currentRotationCoroutine = null;
    }
}
