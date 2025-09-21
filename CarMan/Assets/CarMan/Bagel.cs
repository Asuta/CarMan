using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//百吉饼相关代码
public class Bagel : MonoBehaviour
{
    public GameObject bagelModelOne;
    public GameObject bagelModelTwo;
    public GameObject bagelModelThree;
    public GameObject bagelModelFour;
    
    private int collisionCount = 0;
    private bool canCollide = true;
    private float collisionCooldown = 0.5f; // 碰撞冷却时间（秒）
    
    // Start is called before the first frame update
    void Start()
    {
        // 初始化模型状态：只显示模型一
        bagelModelOne.SetActive(true);
        bagelModelTwo.SetActive(false);
        bagelModelThree.SetActive(false);
        bagelModelFour.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerHead" && canCollide)
        {
            // 立即禁用碰撞，防止重复触发
            canCollide = false;
            
            collisionCount++;
            Debug.Log("Player hit the bagel - Collision count: " + collisionCount);
            
            switch (collisionCount)
            {
                case 1:
                    // 第一次碰撞：关闭模型一，打开模型二
                    bagelModelOne.SetActive(false);
                    bagelModelTwo.SetActive(true);
                    Debug.Log("第一次碰撞：切换到模型二");
                    break;
                    
                case 2:
                    // 第二次碰撞：关闭模型二，打开模型三
                    bagelModelTwo.SetActive(false);
                    bagelModelThree.SetActive(true);
                    Debug.Log("第二次碰撞：切换到模型三");
                    break;
                    
                case 3:
                    // 第三次碰撞：关闭模型三，打开模型四
                    bagelModelThree.SetActive(false);
                    bagelModelFour.SetActive(true);
                    Debug.Log("第三次碰撞：切换到模型四");
                    break;
                    
                case 4:
                    // 第四次碰撞：关闭所有模型，销毁自己
                    bagelModelOne.SetActive(false);
                    bagelModelTwo.SetActive(false);
                    bagelModelThree.SetActive(false);
                    bagelModelFour.SetActive(false);
                    Debug.Log("第四次碰撞：关闭所有模型并销毁百吉饼");
                    Destroy(gameObject);
                    return; // 直接返回，不需要重新启用碰撞
                    break;
                    
                default:
                    // 如果超过4次碰撞，直接销毁
                    Debug.Log("超过4次碰撞，直接销毁百吉饼");
                    Destroy(gameObject);
                    return; // 直接返回，不需要重新启用碰撞
                    break;
            }
            
            // 启动协程来重新启用碰撞
            StartCoroutine(EnableCollisionAfterCooldown());
        }
    }
    
    private System.Collections.IEnumerator EnableCollisionAfterCooldown()
    {
        // 等待指定的冷却时间
        yield return new WaitForSeconds(collisionCooldown);
        
        // 重新启用碰撞
        canCollide = true;
        Debug.Log("碰撞已重新启用");
    }
}
