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
    List<Vector3> ropePoints = new List<Vector3>();
    RaycastHit baseHit;
    bool baseRayBlocked;
    bool prevRayBlocked;
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

    public void RopeBendingCheck()
    {
        Debug.Log("ropePoints " + ropePoints.Count);
        Vector3 hookToRopePoint = hookBase.transform.position - ropePoints[ropePoints.Count - 1];

        baseRayBlocked = Physics.Raycast(hookBase.transform.position, hookToRopePoint, out baseHit, hookToRopePoint.magnitude - 1, layerMask); // giving some leeway to max distance be subtracting 1
        if (baseRayBlocked)
        {
            AddToRopePoint(baseHit.point);
        }
        if (ropePoints.Count >=2) // if the rope is bending on some surface.
        {
            prevRayBlocked = Physics.Raycast(hookBase.transform.position, ropePoints[ropePoints.Count-2], maxLength, layerMask);
            if (!prevRayBlocked)
            {
                RemoveNewestRopePoint();
            }
        }
        DebugRope();
    }

    private void DebugRope()
    {
        for (int i = 1; i < ropePoints.Count; i++)
        {
            Debug.DrawRay(ropePoints[i-1], ropePoints[i] - ropePoints[i-1], Color.yellow, 1);
        }
        Debug.DrawRay(ropePoints[ropePoints.Count-1], hookBase.transform.position - ropePoints[ropePoints.Count - 1], Color.yellow, 1);
    }

    private SoftJointLimit SetLinearLimit(ConfigurableJoint joint, float Lenght)
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

    private void AddToRopePoint(Vector3 point)
    {
        transform.position = point;
        ropePoints.Add(point);
        SetRemainingLength();
    }
    private void RemoveNewestRopePoint()
    {
        ropePoints.Remove(ropePoints[ropePoints.Count - 1]);
        transform.position = ropePoints[ropePoints.Count - 1];
        SetRemainingLength();

    }

    private void SetRemainingLength()
    {
        remainingLength = maxLength;
        for (int i = 1; i < ropePoints.Count; i++)
        {
            remainingLength -= Vector3.Distance(ropePoints[i - 1], ropePoints[i]);
        }
        joint.linearLimit = SetLinearLimit(joint, remainingLength);
    }

    public void ShootHook()
    {
        if(shotBool) //if the ray cast did hit something,
        {
            AddToRopePoint(shotHit.point); //move hook to hit point

            transform.SetParent(shotHit.transform); // set the object that was hit as a parent
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
        ropePoints.Clear();

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