using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class Coin : MonoBehaviour
{
    public InputActionProperty LeftSecondButton;
    public InputActionProperty RightSecondButton;
    public bool isHolding;


    // Start is called before the first frame update
    void Start()
    {
        LeftSecondButton.action.Enable();
        RightSecondButton.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
      
    }


    public void OnGrabObject()
    {
        isHolding = true;
    }

    public void OnReleaseObject()
    {
        MyEvent.AddCoinEvent.Invoke();
        isHolding = false;
        Destroy(gameObject);
    }
}
