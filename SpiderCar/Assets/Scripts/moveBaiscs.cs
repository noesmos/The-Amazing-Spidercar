using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class moveBaiscs : MonoBehaviour
{


    Rigidbody rb;
    InputManager im;
    Vector3 localDownForce = Vector3.zero; 

    public float driveSpeed =1;
    public float maxDriveVelocity =100;
    public float Deceleration = 0.2f;
    public float DecelerationThreshold  = 1;

    public float jumpForce;
    
    public float steerRotation = 30;
    public float aerialRotation = 30;
    public float reverseSpeed = 1;
    public float maxReverseVelocity = 10;

    Vector3 accelerationForce = Vector3.zero;
    Vector3 decelerationForce = Vector3.zero;


    Vector3 totalForce =Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        im = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(im.IsOnGround)
        {
            localDownForce = -accelerationForce.magnitude /10 * transform.up;
        }
            
        //sum up forces
        totalForce = accelerationForce + localDownForce;

        //totalForce = transform.forward *totalForce.magnitude;

        //rb.velocity=totalForce;
        rb.AddForce(totalForce);
        Debug.DrawRay(transform.position, rb.velocity,Color.red,0.1f);

    }

    public void Accelerate()
    {
        MoveForward(driveSpeed,maxDriveVelocity);
    }
    public void Decelerate()
    {
        if(rb.velocity.magnitude > DecelerationThreshold)
        {
            accelerationForce += -accelerationForce*Deceleration;
        }
        else
        {
            accelerationForce = Vector3.zero;
            im.IsDriving = false;
        }
        //driveForce = Vector3.zero;
    }
    public void Reverse()
    {
        MoveForward(-reverseSpeed,maxReverseVelocity);
    }

    public void Break()
    {
        rb.AddForce(-accelerationForce*2);
    }

    public void ResetAcceleration()
    {
        accelerationForce = Vector3.zero;
    }

    public void steer(float horizontal)
    {
        transform.RotateAround(transform.position, transform.up,steerRotation *horizontal*Time.deltaTime);
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up*jumpForce,ForceMode.Impulse);
    }

    void MoveForward(float speed,float maxSpeed)
    {
        if(rb.velocity.magnitude < maxSpeed)
        {
            accelerationForce += transform.forward *speed;
            float velocity = accelerationForce.magnitude;
            accelerationForce = transform.forward*velocity;
        }
        else
        {
            rb.velocity = rb.velocity/rb.velocity.magnitude*maxSpeed; // the naughty way to set maximum velocity
        }
    }

    public void AerialSteer(float vertical, float horizontal)
    {
        transform.RotateAround(transform.position, -transform.forward,aerialRotation*horizontal*Time.deltaTime);
        transform.RotateAround(transform.position, transform.right,aerialRotation*vertical*Time.deltaTime);
    } 
}
