using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRailTrack : MonoBehaviour
{
    LineRenderer lr;
    Vector3[] points;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Start()
    {
        points = new Vector3[2];
        points[0] = new Vector3(.3057f, 1.3361f, .4382f);
        points[1] = new Vector3(.3057f, 1.0934f, .4382f);
    }

    public void SetUpRail(Vector3[] points)
    {
        this.points = points;
        lr.positionCount = points.Length;
        lr.SetPositions(points);
    }

}
