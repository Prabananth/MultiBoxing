using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using UnityEngine;
using System;

public class groundTilt : MonoBehaviour {

    //public GameObject ground;
    public GameObject BodySourceManager;
    private BodySourceManager _BodyManager;
    public Kinect.Vector4 floorClipPlane;

    // Use this for initialization
    void Start () {
        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
    }
	
	// Update is called once per frame
	void Update () {
        floorClipPlane = _BodyManager.GetFloorClipPlane();
        Vector3 InNormal = new Vector3(floorClipPlane.X, floorClipPlane.Y, floorClipPlane.Z);
        float floorDistance = floorClipPlane.W;
        float tiltAngle = (float)(Math.Atan(InNormal.z / InNormal.y) * (180.0 / Math.PI));
        transform.position = new Vector3(0, 0 - (floorClipPlane.W), 0);

        transform.rotation = Quaternion.AngleAxis(tiltAngle, Vector3.down);

    }
}
