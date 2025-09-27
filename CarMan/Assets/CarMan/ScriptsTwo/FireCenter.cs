using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 保持物体世界坐标角度始终为 (0, 0, 0)
        transform.rotation = Quaternion.identity;
    }
}
