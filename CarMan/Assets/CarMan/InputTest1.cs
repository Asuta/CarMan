using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest3 : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset inputActionAsset;

    // 输入动作引用 - 按钮事件
    private InputAction gripPressedR;
    private InputAction gripReleasedR;
    private InputAction triggerPressedR;
    private InputAction triggerReleasedR;
    private InputAction primaryPressedR;
    private InputAction primaryReleasedR;

    private InputAction gripPressedL;
    private InputAction gripReleasedL;
    private InputAction triggerPressedL;
    private InputAction triggerReleasedL;
    private InputAction primaryPressedL;
    private InputAction primaryReleasedL;

    // 输入动作引用 - 轴值
    private InputAction gripAxisR;
    private InputAction triggerAxisR;
    private InputAction gripAxisL;
    private InputAction triggerAxisL;

    // 摇杆输入动作引用（如果可用）
    private InputAction primary2DAxisR;
    private InputAction primary2DAxisL;
    
    // 用于跟踪按钮状态，避免重复日志
    private Dictionary<string, bool> buttonStates = new Dictionary<string, bool>();

    // 用于跟踪轴值，避免重复日志
    private Dictionary<string, float> axisValues = new Dictionary<string, float>();

    // 事件处理器引用，用于正确移除监听器
    private System.Action<InputAction.CallbackContext> gripRPressedHandler;
    private System.Action<InputAction.CallbackContext> triggerRPressedHandler;
    private System.Action<InputAction.CallbackContext> primaryRPressedHandler;
    private System.Action<InputAction.CallbackContext> gripLPressedHandler;
    private System.Action<InputAction.CallbackContext> triggerLPressedHandler;
    private System.Action<InputAction.CallbackContext> primaryLPressedHandler;

    // Start is called before the first frame update
    void Start()
    {
        // 初始化按钮状态字典
        InitializeButtonStates();

        // 初始化轴值字典
        InitializeAxisValues();

        // 查找所有输入动作
        FindInputActions();

        // 启用所有输入动作
        EnableAllActions();

        // 为按钮动作添加事件监听器
        SetupButtonListeners();

        Debug.Log("输入测试系统已初始化");
    }

    private void FindInputActions()
    {
        if (inputActionAsset == null)
        {
            Debug.LogError("InputActionAsset 未设置！请在Inspector中分配InputActionAsset。");
            return;
        }

        // 查找右手输入动作 - 使用正确的Action名称
        gripPressedR = FindAction("Grip Pressed (R)");
        gripReleasedR = FindAction("Grip Released (R)");
        triggerPressedR = FindAction("Trigger Pressed (R)");
        triggerReleasedR = FindAction("Trigger Released (R)");
        primaryPressedR = FindAction("Primary Pressed (R)");
        primaryReleasedR = FindAction("Primary Released (R)");
        gripAxisR = FindAction("Grip Axis (R)");
        triggerAxisR = FindAction("Trigger Axis (R)");

        // 查找左手输入动作
        gripPressedL = FindAction("Grip Pressed (L)");
        gripReleasedL = FindAction("Grip Released (L)");
        triggerPressedL = FindAction("Trigger Pressed (L)");
        triggerReleasedL = FindAction("Trigger Released (L)");
        primaryPressedL = FindAction("Primary Pressed (L)");
        primaryReleasedL = FindAction("Primary Released (L)");
        gripAxisL = FindAction("Grip Axis (L)");
        triggerAxisL = FindAction("Trigger Axis (L)");

        // 尝试查找摇杆输入（可能不存在于AutoHand InputActions中）
        primary2DAxisR = FindAction("Primary2DAxis") ?? FindAction("Move") ?? FindAction("Primary 2D Axis");
        primary2DAxisL = FindAction("Primary2DAxis") ?? FindAction("Move") ?? FindAction("Primary 2D Axis");

        if (primary2DAxisR != null || primary2DAxisL != null)
        {
            Debug.Log("找到摇杆输入动作");
        }
        else
        {
            Debug.LogWarning("未找到摇杆输入动作，可能需要使用包含摇杆的InputActionAsset");
        }

        Debug.Log("输入动作查找完成");
    }

    private InputAction FindAction(string actionName)
    {
        if (inputActionAsset == null)
        {
            Debug.LogError("InputActionAsset 为空");
            return null;
        }

        // 遍历所有ActionMap查找指定动作
        foreach (var actionMap in inputActionAsset.actionMaps)
        {
            foreach (var action in actionMap.actions)
            {
                if (action.name.Contains(actionName))
                {
                    Debug.Log($"找到输入动作: {action.name} 在 ActionMap: {actionMap.name}");
                    return action;
                }
            }
        }

        Debug.LogWarning($"未找到包含 '{actionName}' 的输入动作");
        return null;
    }
    
    // Update is called once per frame
    void Update()
    {
        // 检测所有轴值变化
        CheckAxisActions();
    }

    private void InitializeButtonStates()
    {
        // 初始化所有按钮状态为 false
        buttonStates["Grip_R"] = false;
        buttonStates["Trigger_R"] = false;
        buttonStates["Primary_R"] = false;
        buttonStates["Grip_L"] = false;
        buttonStates["Trigger_L"] = false;
        buttonStates["Primary_L"] = false;
    }

    private void InitializeAxisValues()
    {
        // 初始化所有轴值为 0
        axisValues["Grip_R"] = 0f;
        axisValues["Grip_L"] = 0f;
        axisValues["Trigger_R"] = 0f;
        axisValues["Trigger_L"] = 0f;

        // 初始化摇杆轴值（Vector2的x和y分量）
        axisValues["Primary2DAxis_R_X"] = 0f;
        axisValues["Primary2DAxis_R_Y"] = 0f;
        axisValues["Primary2DAxis_L_X"] = 0f;
        axisValues["Primary2DAxis_L_Y"] = 0f;
    }

    private void EnableAllActions()
    {
        // 启用右手按钮动作
        if (gripPressedR != null) { gripPressedR.Enable(); Debug.Log("右手Grip Pressed动作已启用"); }
        if (gripReleasedR != null) { gripReleasedR.Enable(); Debug.Log("右手Grip Released动作已启用"); }
        if (triggerPressedR != null) { triggerPressedR.Enable(); Debug.Log("右手Trigger Pressed动作已启用"); }
        if (triggerReleasedR != null) { triggerReleasedR.Enable(); Debug.Log("右手Trigger Released动作已启用"); }
        if (primaryPressedR != null) { primaryPressedR.Enable(); Debug.Log("右手Primary Pressed动作已启用"); }
        if (primaryReleasedR != null) { primaryReleasedR.Enable(); Debug.Log("右手Primary Released动作已启用"); }

        // 启用右手轴动作
        if (gripAxisR != null) { gripAxisR.Enable(); Debug.Log("右手Grip Axis动作已启用"); }
        if (triggerAxisR != null) { triggerAxisR.Enable(); Debug.Log("右手Trigger Axis动作已启用"); }

        // 启用左手按钮动作
        if (gripPressedL != null) { gripPressedL.Enable(); Debug.Log("左手Grip Pressed动作已启用"); }
        if (gripReleasedL != null) { gripReleasedL.Enable(); Debug.Log("左手Grip Released动作已启用"); }
        if (triggerPressedL != null) { triggerPressedL.Enable(); Debug.Log("左手Trigger Pressed动作已启用"); }
        if (triggerReleasedL != null) { triggerReleasedL.Enable(); Debug.Log("左手Trigger Released动作已启用"); }
        if (primaryPressedL != null) { primaryPressedL.Enable(); Debug.Log("左手Primary Pressed动作已启用"); }
        if (primaryReleasedL != null) { primaryReleasedL.Enable(); Debug.Log("左手Primary Released动作已启用"); }

        // 启用左手轴动作
        if (gripAxisL != null) { gripAxisL.Enable(); Debug.Log("左手Grip Axis动作已启用"); }
        if (triggerAxisL != null) { triggerAxisL.Enable(); Debug.Log("左手Trigger Axis动作已启用"); }

        // 启用摇杆动作（如果可用）
        if (primary2DAxisR != null) { primary2DAxisR.Enable(); Debug.Log("右手摇杆动作已启用"); }
        if (primary2DAxisL != null) { primary2DAxisL.Enable(); Debug.Log("左手摇杆动作已启用"); }
    }
    
    private void SetupButtonListeners()
    {
        // 创建事件处理器引用
        gripRPressedHandler = ctx => OnButtonPressed("Grip_R");
        triggerRPressedHandler = ctx => OnButtonPressed("Trigger_R");
        primaryRPressedHandler = ctx => OnButtonPressed("Primary_R");
        gripLPressedHandler = ctx => OnButtonPressed("Grip_L");
        triggerLPressedHandler = ctx => OnButtonPressed("Trigger_L");
        primaryLPressedHandler = ctx => OnButtonPressed("Primary_L");

        // 右手按钮监听器 - Pressed事件
        if (gripPressedR != null)
        {
            gripPressedR.performed += gripRPressedHandler;
            Debug.Log("右手Grip Pressed监听器已设置");
        }
        if (triggerPressedR != null)
        {
            triggerPressedR.performed += triggerRPressedHandler;
            Debug.Log("右手Trigger Pressed监听器已设置");
        }
        if (primaryPressedR != null)
        {
            primaryPressedR.performed += primaryRPressedHandler;
            Debug.Log("右手Primary Pressed监听器已设置");
        }

        // 右手按钮监听器 - Released事件
        if (gripReleasedR != null)
        {
            gripReleasedR.performed += ctx => OnButtonReleased("Grip_R");
            Debug.Log("右手Grip Released监听器已设置");
        }
        if (triggerReleasedR != null)
        {
            triggerReleasedR.performed += ctx => OnButtonReleased("Trigger_R");
            Debug.Log("右手Trigger Released监听器已设置");
        }
        if (primaryReleasedR != null)
        {
            primaryReleasedR.performed += ctx => OnButtonReleased("Primary_R");
            Debug.Log("右手Primary Released监听器已设置");
        }

        // 左手按钮监听器 - Pressed事件
        if (gripPressedL != null)
        {
            gripPressedL.performed += gripLPressedHandler;
            Debug.Log("左手Grip Pressed监听器已设置");
        }
        if (triggerPressedL != null)
        {
            triggerPressedL.performed += triggerLPressedHandler;
            Debug.Log("左手Trigger Pressed监听器已设置");
        }
        if (primaryPressedL != null)
        {
            primaryPressedL.performed += primaryLPressedHandler;
            Debug.Log("左手Primary Pressed监听器已设置");
        }

        // 左手按钮监听器 - Released事件
        if (gripReleasedL != null)
        {
            gripReleasedL.performed += ctx => OnButtonReleased("Grip_L");
            Debug.Log("左手Grip Released监听器已设置");
        }
        if (triggerReleasedL != null)
        {
            triggerReleasedL.performed += ctx => OnButtonReleased("Trigger_L");
            Debug.Log("左手Trigger Released监听器已设置");
        }
        if (primaryReleasedL != null)
        {
            primaryReleasedL.performed += ctx => OnButtonReleased("Primary_L");
            Debug.Log("左手Primary Released监听器已设置");
        }
    }
    
    private void OnButtonPressed(string buttonName)
    {
        if (!buttonStates[buttonName])
        {
            Debug.Log($"按钮按下: {buttonName}");
            buttonStates[buttonName] = true;
        }
    }

    private void OnButtonReleased(string buttonName)
    {
        if (buttonStates[buttonName])
        {
            Debug.Log($"按钮释放: {buttonName}");
            buttonStates[buttonName] = false;
        }
    }
    
    private void CheckAxisActions()
    {
        // 检测右手轴值变化
        if (gripAxisR != null)
            CheckAxis("Grip_R", gripAxisR.ReadValue<float>());

        if (triggerAxisR != null)
            CheckAxis("Trigger_R", triggerAxisR.ReadValue<float>());

        // 检测左手轴值变化
        if (gripAxisL != null)
            CheckAxis("Grip_L", gripAxisL.ReadValue<float>());

        if (triggerAxisL != null)
            CheckAxis("Trigger_L", triggerAxisL.ReadValue<float>());

        // 检测摇杆输入变化
        if (primary2DAxisR != null)
            Check2DAxis("Primary2DAxis_R", primary2DAxisR.ReadValue<Vector2>());

        if (primary2DAxisL != null)
            Check2DAxis("Primary2DAxis_L", primary2DAxisL.ReadValue<Vector2>());
    }

    private void CheckAxis(string axisName, float currentValue)
    {
        if (!axisValues.ContainsKey(axisName))
        {
            axisValues[axisName] = 0f;
        }

        float previousValue = axisValues[axisName];
        float threshold = 0.01f; // 阈值，避免微小变化产生日志

        if (Mathf.Abs(currentValue - previousValue) > threshold)
        {
            Debug.Log($"轴值变化: {axisName} = {currentValue:F3}");
            axisValues[axisName] = currentValue;
        }
    }

    private void Check2DAxis(string axisName, Vector2 currentValue)
    {
        string xAxisName = axisName + "_X";
        string yAxisName = axisName + "_Y";

        // 检查X轴
        if (!axisValues.ContainsKey(xAxisName))
        {
            axisValues[xAxisName] = 0f;
        }

        // 检查Y轴
        if (!axisValues.ContainsKey(yAxisName))
        {
            axisValues[yAxisName] = 0f;
        }

        float previousX = axisValues[xAxisName];
        float previousY = axisValues[yAxisName];
        float threshold = 0.01f; // 阈值，避免微小变化产生日志

        bool xChanged = Mathf.Abs(currentValue.x - previousX) > threshold;
        bool yChanged = Mathf.Abs(currentValue.y - previousY) > threshold;

        if (xChanged || yChanged)
        {
            Debug.Log($"摇杆变化: {axisName} = ({currentValue.x:F3}, {currentValue.y:F3})");
            axisValues[xAxisName] = currentValue.x;
            axisValues[yAxisName] = currentValue.y;
        }
    }
    
    private void OnDestroy()
    {
        // 移除所有监听器并禁用输入动作
        RemoveAllListeners();
        DisableAllActions();

        Debug.Log("输入测试系统已禁用");
    }

    private void RemoveAllListeners()
    {
        // 移除右手按钮监听器 - Pressed事件
        if (gripPressedR != null && gripRPressedHandler != null)
        {
            gripPressedR.performed -= gripRPressedHandler;
        }
        if (triggerPressedR != null && triggerRPressedHandler != null)
        {
            triggerPressedR.performed -= triggerRPressedHandler;
        }
        if (primaryPressedR != null && primaryRPressedHandler != null)
        {
            primaryPressedR.performed -= primaryRPressedHandler;
        }

        // 移除左手按钮监听器 - Pressed事件
        if (gripPressedL != null && gripLPressedHandler != null)
        {
            gripPressedL.performed -= gripLPressedHandler;
        }
        if (triggerPressedL != null && triggerLPressedHandler != null)
        {
            triggerPressedL.performed -= triggerLPressedHandler;
        }
        if (primaryPressedL != null && primaryLPressedHandler != null)
        {
            primaryPressedL.performed -= primaryLPressedHandler;
        }

        // 注意：Released事件使用了lambda表达式，无法完全移除，但在OnDestroy时会自动清理
    }

    private void DisableAllActions()
    {
        // 禁用右手按钮动作
        if (gripPressedR != null) gripPressedR.Disable();
        if (gripReleasedR != null) gripReleasedR.Disable();
        if (triggerPressedR != null) triggerPressedR.Disable();
        if (triggerReleasedR != null) triggerReleasedR.Disable();
        if (primaryPressedR != null) primaryPressedR.Disable();
        if (primaryReleasedR != null) primaryReleasedR.Disable();

        // 禁用右手轴动作
        if (gripAxisR != null) gripAxisR.Disable();
        if (triggerAxisR != null) triggerAxisR.Disable();

        // 禁用左手按钮动作
        if (gripPressedL != null) gripPressedL.Disable();
        if (gripReleasedL != null) gripReleasedL.Disable();
        if (triggerPressedL != null) triggerPressedL.Disable();
        if (triggerReleasedL != null) triggerReleasedL.Disable();
        if (primaryPressedL != null) primaryPressedL.Disable();
        if (primaryReleasedL != null) primaryReleasedL.Disable();

        // 禁用左手轴动作
        if (gripAxisL != null) gripAxisL.Disable();
        if (triggerAxisL != null) triggerAxisL.Disable();

        // 禁用摇杆动作
        if (primary2DAxisR != null) primary2DAxisR.Disable();
        if (primary2DAxisL != null) primary2DAxisL.Disable();
    }
}
