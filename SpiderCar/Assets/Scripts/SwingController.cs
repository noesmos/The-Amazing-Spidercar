using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingController : MonoBehaviour
{
    GameObject hook;
    
    // Start is called before the first frame update
    void Start()
    {
        hook = GameObject.Find("Hook");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void rotateCarTowardsHook()
    {
        Vector3 direction= Vector3.Normalize(hook.transform.position - transform.position);
        transform.forward = direction;
    }

}
