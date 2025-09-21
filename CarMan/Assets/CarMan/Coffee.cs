using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Coffee : MonoBehaviour
{
    public InputActionProperty LeftSecondButton;
    public bool isHolding;
    public GameObject coffeelidOne;
    public GameObject coffeelidTwo;
    private bool isFirstLidOpen = true;
    public bool isOpen = false;
    public bool isHot = true;
    public Transform cloudPoint;
    public GameObject cloudPrefab;
    private Coroutine coolingCoroutine;
    private float cloudSpawnTimer = 0f;
    private float cloudSpawnInterval = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        LeftSecondButton.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (LeftSecondButton.action.WasPressedThisFrame())
        {
            SwitchLid();
        }

        // 检查是否既是热的又是开启状态
        if (isHot && isOpen)
        {
            // 更新云生成计时器
            cloudSpawnTimer += Time.deltaTime;
            
            // 检查是否达到生成间隔
            if (cloudSpawnTimer >= cloudSpawnInterval)
            {
                // 在cloudPoint位置生成cloudPrefab
                Instantiate(cloudPrefab, cloudPoint.position, cloudPoint.rotation);
                
                // 重置计时器
                cloudSpawnTimer = 0f;
            }
        }
        else
        {
            // 如果不满足条件，重置计时器
            cloudSpawnTimer = 0f;
        }

        // //test
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     SwitchLid();
        // }
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
                
                // 开始冷却计时
                StartCoolingTimer();
            }
            else
            {
                // 打开第一个盖子，关闭第二个盖子
                coffeelidOne.SetActive(true);
                coffeelidTwo.SetActive(false);
                isOpen = false;
                
                // 停止冷却计时
                StopCoolingTimer();
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
    
    private void StartCoolingTimer()
    {
        // 停止之前可能存在的协程
        StopCoolingTimer();
        
        // 启动新的协程
        coolingCoroutine = StartCoroutine(CoolingTimer());
    }
    
    private void StopCoolingTimer()
    {
        if (coolingCoroutine != null)
        {
            StopCoroutine(coolingCoroutine);
            coolingCoroutine = null;
        }
    }
    
    private IEnumerator CoolingTimer()
    {
        // 等待5秒
        yield return new WaitForSeconds(5.0f);
        
        // 5秒后将咖啡设置为不热
        isHot = false;
        
        // 清空协程引用
        coolingCoroutine = null;
    }
}
