using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRoll : MonoBehaviour
{
    public bool frontWheel;

    GameObject body;

    Rigidbody rb;
    float rotateForward;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        body = GameObject.Find("Car/Body");
    }

    // Update is called once per frame
    void Update()
    {
        rotateForward = Input.GetAxisRaw("Vertical"); 
        rb.AddRelativeTorque(0,rotateForward,0);
        
    }
}
