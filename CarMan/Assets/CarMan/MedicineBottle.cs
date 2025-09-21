using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MedicineBottle : MonoBehaviour
{
    public InputActionProperty LeftSecondButton;
    public InputActionProperty RightSecondButton;
    public bool isHolding;
    public GameObject coffeelidOne;
    public GameObject coffeelidTwo;
    private bool isFirstLidOpen = true;
    public bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        LeftSecondButton.action.Enable();
        RightSecondButton.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (LeftSecondButton.action.WasPressedThisFrame() || RightSecondButton.action.WasPressedThisFrame())
        {
            SwitchLid();
        }
    }

    void SwitchLid()
    {
        if (isHolding)
        {
            if (isFirstLidOpen)
            {
                // 关闭第一个盖子，打开第二个盖子
                coffeelidOne.SetActive(false);
                coffeelidTwo.SetActive(true);
                isOpen = true;
            }
            else
            {
                // 打开第一个盖子，关闭第二个盖子
                coffeelidOne.SetActive(true);
                coffeelidTwo.SetActive(false);
                isOpen = false;
            }

            // 切换状态
            isFirstLidOpen = !isFirstLidOpen;
        }
    }

    public void OnGrabObject()
    {
        isHolding = true;
    }

    public void OnReleaseObject()
    {
        isHolding = false;
    }
}
