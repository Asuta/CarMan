using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {

        Debug.Log("Trigger Enter");

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter");
    }
}
