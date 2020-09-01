using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float x, y, z;

    Vector3 rotationVector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotationVector = new Vector3(x, y, z)*Time.deltaTime;
        transform.Rotate(rotationVector);
    }
}
