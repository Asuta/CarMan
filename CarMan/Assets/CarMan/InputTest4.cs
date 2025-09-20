using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
 
public class InputTest4 : MonoBehaviour
{
    public InputActionProperty menuButton;
    public InputActionProperty stickL;

    public bool testBool;

    // Start is called before the first frame update
    void Start()
    {
        // 启用输入动作
        menuButton.action.Enable();
        stickL.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // 使用现成的方法检测按钮按下的瞬间
        if (menuButton.action.WasPressedThisFrame())
        {
            Debug.Log("=== 菜单按钮被按下的瞬间 ===");
        }

        // 使用现成的方法检测按钮抬起的瞬间
        if (menuButton.action.WasReleasedThisFrame())
        {
            Debug.Log("=== 菜单按钮被抬起的瞬间 ===");
        }

        // 检测菜单按钮当前是否被按下
        bool isCurrentlyPressed = menuButton.action.IsPressed();
        if (isCurrentlyPressed)
        {
            // 按钮处于按住状态时，布尔值为false
            testBool = false;
        }
        else
        {
            // 按钮处于抬起状态时，布尔值为true
            testBool = true;
        }

        // 输出当前状态用于调试
        // Debug.Log("菜单按钮状态: " + (isCurrentlyPressed ? "按下" : "抬起") + ", testBool: " + testBool);

        // 读取并输出摇杆的值
        Vector2 stickValue = stickL.action.ReadValue<Vector2>();
        // Debug.Log("摇杆值 - X: " + stickValue.x + ", Y: " + stickValue.y);
    }
}
