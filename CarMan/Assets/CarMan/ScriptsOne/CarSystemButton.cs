using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CarSystemButton : MonoBehaviour
{
    public UnityEvent OnButtonPressed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
            Debug.Log("Button Pressed");
            OnButtonPressed.Invoke();
        }
        else
        {
            Debug.Log("Button Not Pressed"+collision.gameObject.layer);
        }
    }
}
