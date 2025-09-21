using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//百吉饼相关代码
public class Bagel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerHead")
        {
            Debug.Log("Player hit the bagel");
        }
        else
        {
            //log other 
        }
    }
}
