using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour
{
    public Vector3 startPosition;
    public Quaternion startRotation;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGrabObject()
    {
        var thisRb = GetComponent<Rigidbody>();
        thisRb.isKinematic = false;
    }

    public void OnReleaseObject()
    {
        var thisRb = GetComponent<Rigidbody>();
        thisRb.isKinematic = true;
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
    }
}
