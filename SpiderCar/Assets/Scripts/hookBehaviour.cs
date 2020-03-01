using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hookBehaviour : MonoBehaviour
{
    public float hookVelocity;
    bool isHooked = false;
    Rigidbody rb;
    GameObject hookBase;
    ConfigurableJoint joint;
    public float maxLength;
    public float hookToBase;

    //UI
    public Image crosshair;

    //shot raycasting
    int layerMask;
    RaycastHit shotHit;
    bool shotBool;

    //Rope
    List<Vector3> ropePoints;
    RaycastHit toBaseHit;
    bool toBaseBool;
    RaycastHit toPrevHit;
    bool toPrevBool;
    float remainingLength;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hookBase = GameObject.Find("Hook Base");

        joint = GetComponentInParent<ConfigurableJoint>();
        //initialPos = joint.connectedAnchor;
        ResetHook();

        layerMask = ~LayerMask.GetMask("Car"); //set layerMask to exclude Car layer
    }



    public bool GetIsHooked()
    {
        return isHooked;
    }

    private SoftJointLimit SetLinearLimit(ConfigurableJoint joint,  float Lenght)
    {
        SoftJointLimit softJoint = joint.linearLimit;
        softJoint.limit = Lenght;

        return softJoint;
    }

    void constrainPosition(bool boolean)
    {
        if (boolean)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition; 
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    public void HookRayCast()
    {
        shotBool = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out shotHit, maxLength, layerMask);
        HighlightCrosshair(shotBool);
    }

    public void ShootHook()
    {
        if(shotBool) //if the ray cast did hit something,
        {
            transform.position = shotHit.point; //move hook to hit point

            hookToBase = Vector3.Distance(transform.position, hookBase.transform.position); //distance to hook from base
            joint.linearLimit = SetLinearLimit(joint, hookToBase);

            transform.SetParent(shotHit.transform);
        }
        else
        {
            ResetHook();
        }
        isHooked = shotBool;
    }

    public void ResetHook()
    {
        transform.SetPositionAndRotation(hookBase.transform.position, hookBase.transform.rotation);
        UniversalFunctions.SetGlobalScale(transform, new Vector3 (0.1f,0.1f, 0.2f));

        transform.SetParent(joint.transform);
    }

    public void SetCrosshair(bool b)
    {
        crosshair.gameObject.SetActive(b);
    }

    private void HighlightCrosshair(bool b)
    {
        Color tempColor = Color.white;
        if (b)
        {
            tempColor.a = 1;
        }
        else
        {
            tempColor.a = 0.5f;
        }
        crosshair.color = tempColor;
    }
}