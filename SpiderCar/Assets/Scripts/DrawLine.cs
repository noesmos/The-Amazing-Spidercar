using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    LineRenderer line;

    GameObject hookBase;

    List<GameObject> ropePoints;
    hookBehaviour hb;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        hb = GetComponent<hookBehaviour>();
        hookBase= GameObject.Find("Hook Base");
    }

    // Update is called once per frame
    void Update()
    {
        ropePoints = hb.GetRopePoints();
        Debug.Log("ropePoints: " + ropePoints.Count);
        line.positionCount = ropePoints.Count+1;

        for (int i = 0; i < ropePoints.Count; i++)
        {
            line.SetPosition(i, ropePoints[i].transform.position);
        }


        line.SetPosition(ropePoints.Count, hookBase.transform.position);
    }
}
