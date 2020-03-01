using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    LineRenderer line;

    GameObject hookBase;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        hookBase= GameObject.Find("Hook Base");
    }

    // Update is called once per frame
    void Update()
    {
         line.SetPosition(0, transform.position);
         line.SetPosition(1, hookBase.transform.position);
    }
}
