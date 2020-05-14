using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public bool BDebug = true;
    public float Radius = 2.0f;
    public Vector3[] PointA;

    public float Length
    {
        get { return PointA.Length; }
    }

    public Vector3 GetPoint(int index)
    {
        return PointA[index];
    }

    private void OnDrawGizmos()
    {
        if (!BDebug) return;
        for(int i = 0; i < PointA.Length; i++)
        {
            if(i + 1 < PointA.Length)
            {
                Debug.DrawLine(PointA[i], PointA[i + 1], Color.red);
            }
        }
    }
}
