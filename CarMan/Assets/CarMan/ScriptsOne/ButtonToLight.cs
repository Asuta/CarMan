using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToLight : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonToLight()
    {
        spriteRenderer.color = Color.red;
    }
}
