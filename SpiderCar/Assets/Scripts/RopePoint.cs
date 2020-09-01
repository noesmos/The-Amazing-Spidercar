using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePoint : MonoBehaviour
{
    public Vector3 point; // found
    public Vector3 ropeVector; // set
    public Vector3 crossVector; //found
    public Vector3 edgeVector; // found
    public Collider col; // set

    string name;

    public RopePoint(Vector3 point, Vector3 ropeVector, Collider col)
    {
        this.ropeVector = ropeVector;
        this.col = col;
        name = col.gameObject.name;
        FindPointAndEdge(point, col.GetComponent<MeshFilter>().mesh);
        FindCrossVector(edgeVector, ropeVector, (col.bounds.center - point));

    }

    public RopePoint(Vector3 point)
    {
        this.point = point;
    }


    private void FindPointAndEdge(Vector3 point, Mesh m) // not debugged
    {
        Debug.Log("mesh name: " + name);
        Debug.Log("Num of vertices: " + m.vertices.Length);
        Vector3 iVertex = Vector3.zero;
        Vector3 jVertex = Vector3.zero;

        float CurrentMag;
        float smallestMag = float.MaxValue;

        for (int i = 0; i < m.vertices.Length-1; i++)
        {
            for (int j = i+1; j < m.vertices.Length; j++)
            {
                CurrentMag = GetSqrMagFromEdge(point, m.vertices[i], m.vertices[j]);
                if (CurrentMag < smallestMag)
                {
                    Debug.Log("found closer edge");
                    smallestMag = CurrentMag;
                    iVertex = m.vertices[i];
                    jVertex = m.vertices[j];
                }
            }
        }

        edgeVector = (iVertex - jVertex);

        this.point = iVertex + Vector3.Project(point - iVertex, jVertex - iVertex);
    }

    private void FindCrossVector(Vector3 edgeVector, Vector3 ropeVector, Vector3 centerVector)
    {
        crossVector = Vector3.Cross(edgeVector, ropeVector);
        float dot = Vector3.Dot(crossVector.normalized, centerVector.normalized);
        
        if (dot > 0)
            crossVector *= -1;
    }

    //based on https://forum.unity.com/threads/find-the-nearest-edge-of-a-mesh-to-transform.197487/
    private float GetSqrMagFromEdge(Vector3 point, Vector3 iVertex, Vector3 jVertex)
    {
        float n = Vector3.Cross(point - iVertex, point - jVertex).sqrMagnitude;
        return n / (iVertex - jVertex).sqrMagnitude;
    }
}
