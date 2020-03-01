using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //input Managers tasks:
    //register input from controller
    //differentiate between players
    //translate input into action to call in form of functions from other script
    //keeps track of the state of the car


    public int playerNumber = 0;

    //input button names
    string camY;
    string camX;
    string reset;
    string vertical;
    string horizontal;
    string accelerate;
    string reverse;
    string jump;
    string shoot;

    //state
    public bool IsOnGround;
    public bool IsDriving;
    bool HasSpikyWheels;
    bool HasJumped;
    bool IsUpsideDown;
    bool HasShot;
    bool HasHit;
    bool IsSwinging;

    //connected scripts
    moveBaiscs mb;
    hookBehaviour hb;
    CameraController cc;

    void Start()
    {
        //set input to playernumber
        camY = "CamY"+playerNumber;
        camX = "CamX"+playerNumber;
        reset = "Reset"+playerNumber;
        vertical = "Vertical"+playerNumber;
        horizontal = "Horizontal"+playerNumber;
        accelerate = "Accelerate"+playerNumber;
        reverse = "Reverse"+playerNumber;
        jump = "Jump"+playerNumber;
        shoot = "Shoot"+playerNumber;

        mb = GetComponent<moveBaiscs>();
        //hb = GameObject.Find("Hook").GetComponent<hookBehaviour>();
        //cc = GameObject.Find("Main Camera").GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Camera movement
        float camYVal = Input.GetAxis(camY);
        if(camYVal !=0)
        {
            Debug.Log("camY "+ camYVal);
        }

        float camXVal = Input.GetAxis(camX);
        if(camXVal !=0)
        {
            Debug.Log("camX "+ camXVal);
        }

        if(camYVal !=0|| camXVal !=0)
        {
            cc.RotateCamera(camYVal, camXVal);
        }

        if(Input.GetButtonDown(reset))
        {
           Debug.Log("Reset");
           cc.ResetCameraRotation();
        }

        //Car movement
        float verticalVal = Input.GetAxis(vertical);
        if(verticalVal !=0){
            Debug.Log("vetical "+ verticalVal);
        }

        float horizontalVal = Input.GetAxis(horizontal);
        if (horizontalVal !=0 && IsDriving && IsOnGround)
        {
            //Debug.Log("horixontal "+ horizontalVal);
            mb.steer(horizontalVal);
            Debug.Log("steer");
        }
        
        if(horizontalVal !=0 && !IsOnGround|| verticalVal !=0 && !IsOnGround)
        {
            mb.AerialSteer(verticalVal, horizontalVal);
        }


        if(Input.GetButton(accelerate) && IsOnGround)
        {
           //Debug.Log("Accelerate");
           IsDriving = true;
           mb.Accelerate();
        }
        if(Input.GetButtonUp(accelerate))
        {
           //Debug.Log("Accelerate");
           IsDriving = true;
           mb.Accelerate();
        }
        else if(IsDriving && IsOnGround)
        {
           Debug.Log("Decelerate");
            mb.Decelerate();
        }

        if(IsDriving && !IsOnGround)
        {
            mb.ResetAcceleration();
        }

        if(Input.GetButton(reverse))
        {
            if (IsDriving)
            {
                Debug.Log("Break");
                mb.Break();
            }
            else
            {
                Debug.Log("Reverse");
                //IsDriving = true;
                //mb.Reverse();
            }

        }

        //Abilities
        if(Input.GetButtonDown(jump) && IsOnGround)
        {
           Debug.Log("Jump");
           mb.Jump();
        }

        if(Input.GetButtonDown(shoot))
        {
           Debug.Log("Shoot");
           hb.ShootHook();
        }
    }

     void OnCollisionEnter(Collision theCollision)
    {
        if (theCollision.gameObject.layer == 9)
        {
            IsOnGround = true;
        }
    }
         void OnCollisionExit(Collision theCollision)
    {
        if (theCollision.gameObject.layer == 9)
        {
            IsOnGround = false;
        }
    }
}
