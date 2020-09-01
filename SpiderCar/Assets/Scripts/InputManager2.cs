using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager2 : MonoBehaviour
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
    string aimMode;
    string cancel;
    string quickReel;
    string pause;

    // stick values
    float verticalVal;
    float horizontalVal;
    float camYVal;
    float camXVal;
    string reelControl;



    //states
    public bool IsGrounded;
    public bool IsDriving;
    bool HasSpikyWheels;
    bool HasJumped;
    bool IsUpsideDown;
    bool HasShot;
    bool IsHooked;
    bool IsSwinging;
    bool BrakeNotReverse;

    bool HasTiltedHorizontal;
    bool HasTiltedVertical;

    //connected scripts
    DriveController dc;
    
    SwingController sc;
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
        aimMode = "AimMode" + playerNumber;
        reelControl = "DPadY" + playerNumber;
        cancel = "Cancel" + playerNumber;
        quickReel = "QuickReel" + playerNumber;
        pause = "Pause" + playerNumber;

        dc = GetComponent<DriveController>();
        sc = GetComponent<SwingController>();
        hb = GameObject.Find("Hook").GetComponent<hookBehaviour>();
        cc = GameObject.Find("Main Camera").GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        verticalVal = Input.GetAxis(vertical);
        horizontalVal = Input.GetAxis(horizontal);

        camYVal = Input.GetAxis(camY);
        camXVal = Input.GetAxis(camX);

        //on the ground

        //in the air

        // aiming

        //when aiming in the air

        //when swinging


        //******Camera movement******
        CameraControl();

// ******Car movement******

        if(IsGrounded)
        {
            //Steering
            if (IsDriving && HasTiltedHorizontal)
            {
                
                dc.Steer(horizontalVal);
            }
            else
            {
                dc.Steer(0);
            }
            
            
            //turning
            if (!IsDriving && HasTiltedHorizontal)
            {
                dc.Turn(horizontalVal);
            }
            else
            {
                dc.ResetTurningStiffness();
            }
        }
        else if(!IsGrounded &&  (HasTiltedHorizontal || HasTiltedVertical))
        {
            //Debug.Log("air steer");
            dc.AerialSteer(horizontalVal, verticalVal);
        }

        //Accelerate
        if(Input.GetButton(accelerate))
        {
           //Debug.Log("Accelerate");
           IsDriving = true;
           dc.Accelerate(1);
        }
        if(Input.GetButtonUp(accelerate))
        {
           //Debug.Log("Accelerate");
           dc.Accelerate(0);
        }

        //Reverse or Brake
        if(Input.GetButtonDown(reverse))
        {
            if(IsDriving)
            {
                BrakeNotReverse = true;
            }
            else
            {
                BrakeNotReverse = false;
            }
        }

        if (Input.GetButton(reverse) && IsGrounded)
        {
            if (BrakeNotReverse)
            {
                //Debug.Log("Brake");
                dc.Brake(1);
            }
            else
            {
                IsDriving = true;
                dc.Reverse(1);
            }
        }

        if (Input.GetButtonUp(reverse))
        {
           dc.Brake(0);
           dc.Reverse(0);
        }

        if (Input.GetButtonDown(cancel))
        {
            if (hb.GetIsHooked())
            {
                hb.ResetHook();
            }
        }
        if (Input.GetButtonDown(pause))
        {
            Debug.Break();
        }

//*****Abilities*****
        if(Input.GetButtonDown(jump) && !HasJumped)
        {
           //Debug.Log("Jump");
           dc.Jump();
           HasJumped=true;
        }


        if (Input.GetButtonDown(shoot))
        {

            if (!IsGrounded)
            {
                dc.StopCarTorque();
                cc.AddCameraRotationToPlayer();
            }
            hb.SetCrosshair(true);
            cc.GoToAimView();
        }
        if (Input.GetButton(shoot))
        {
            hb.HookRayCast();
        }
        if (Input.GetButtonUp(shoot))
        {
            hb.ResetHook();
            hb.ShootHook();
            if (hb.GetIsHooked())
            {
                cc.GoToFarView();
            }
            else
            {
                cc.GoToDefView();
            }
            hb.SetCrosshair(false);
        }
        if (hb.GetIsHooked())
        {
            hb.RopeBendingCheck();

            hb.ReelRope(Input.GetAxis(reelControl));
        }
        if (Input.GetButton(quickReel))
        {
            if (hb.GetIsHooked())
            {
                hb.QuickReel();
            }
        }



        if (Input.GetButtonDown(aimMode))
        {
            //Debug.Log("Shoot");
            cc.GoToFarView();
        }

        //Check States
        IsGrounded = dc.CheckWheelsGrounded();
        HasJumped = !IsGrounded;
        IsHooked = hb.GetIsHooked();
        HasTiltedHorizontal = SetStickBool(horizontalVal);
        HasTiltedVertical = SetStickBool(verticalVal);


    }   
    
    void CameraControl()
    {
        if (camYVal != 0 || camXVal != 0)
        {
            cc.RotateCamera(camYVal, camXVal);
        }

        if (Input.GetButtonDown(reset))
        {
            cc.ResetCameraRotation();
        }

        if (Input.GetKeyDown("z"))
        {
            //Debug.Log("zooom");
            cc.GoToFarView();
        }
    }


    public void SetIsDriving(bool boolean)
    {
        IsDriving = boolean;
    }

    private bool SetStickBool(float val)
    {
        if(val == 0)
        {
            return false;
        }
        return true;
    }


    
}
