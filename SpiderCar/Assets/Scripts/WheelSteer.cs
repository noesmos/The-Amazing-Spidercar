using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSteer : MonoBehaviour
{
    public bool isFrontWheel;
    float steerInput;
    public float steerAmount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isFrontWheel)
        steerInput = Input.GetAxisRaw("Horizontal");
        transform.RotateAround(transform.position, transform.up,steerAmount*steerInput*Time.deltaTime);
    }
}
