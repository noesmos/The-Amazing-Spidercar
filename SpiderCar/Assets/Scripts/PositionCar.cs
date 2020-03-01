using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCar : MonoBehaviour
{
    

    public Transform[] wheels;
    public float height;
    Rigidbody rb;

    Vector3 nextPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        nextPosition= GetCarPosition(wheels, height);

        rb.MovePosition(nextPosition);
    }



    Vector3 GetCarPosition(Transform[] wheels, float height)
    {
        Vector3 carPosition = Vector3.zero;

        for (int i = 0; i < wheels.Length; i++)
        {
            carPosition += wheels[i].position + height*transform.up;
        }

        carPosition = carPosition/wheels.Length;

        return carPosition;
    }
}
