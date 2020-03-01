using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyInfo : MonoBehaviour
{

    Rigidbody rb;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = rb.velocity.magnitude;
    }
}
