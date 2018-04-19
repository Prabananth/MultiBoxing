using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class skeleton : NetworkBehaviour
{
    public Material pickup;
    public GameObject BodySourceManager;
    public GameObject ground;
    public static float XHumanScalingFactor = 1;
    public static float YHumanScalingFactor = 1;
    public GameObject startCircle;

    public MapJoins MJ;
    public Kinect.Vector4 floorClipPlane;
    public int count = 0;

    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;

    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };

    private void Awake()
    {
        SaveLoad.Load();
    }

    void Update()
    {
        if (!isServer)
        {
            return;
        }   
        if (BodySourceManager == null)
        {
            return;
        }

        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }

        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }
        
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        int i = 0;
        foreach (var body in data)
        {   
            if (body == null)
            {
                continue;
            }
            
            if (body.IsTracked)
            {
                if (!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }
                
                RefreshBodyObject(body, _Bodies[body.TrackingId]);
            }
            i++;
        }

    }
   

    
    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);
        
        //body.transform.position = new Vector3(0.0f, 10.0f, 0.0f);

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            jointObj.name = "Skeleton"+jt.ToString();
            SphereCollider scj = jointObj.GetComponent<SphereCollider>();
            scj.isTrigger = true;
            
            if(jt == Kinect.JointType.HandLeft || jt == Kinect.JointType.HandRight)
            {
                scj.radius = 1.5f;
            }

            Rigidbody rbj = jointObj.AddComponent<Rigidbody>() as Rigidbody;
            rbj.useGravity = false;

            jointObj.AddComponent<colloideDetect>();
            JointsProperties joint = jointObj.AddComponent<JointsProperties>();
            joint.name = jt.ToString();
            joint.floor = floorClipPlane;

            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.material = pickup;
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            
            jointObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            jointObj.name = "Skeleton" + jt.ToString();
            jointObj.transform.parent = body.transform;
        }
        body.transform.parent = transform;
        body.tag = "SkeletonBody";
        BodyProperties BP = body.AddComponent<BodyProperties>();
        BP.ParentCircle = startCircle;
        BP.MJ = MJ;
        Rigidbody rb = body.AddComponent<Rigidbody>();
        CapsuleCollider cc = body.AddComponent<CapsuleCollider>();
        cc.isTrigger = true;
        cc.height = 2.5f;
        cc.radius = 0.1f;
        StartCircle.player = body;
        rb.useGravity = false;
        
        return body;
    }

    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        count++;
        Kinect.Joint s = body.Joints[Kinect.JointType.WristLeft];
        Kinect.Joint t = body.Joints[Kinect.JointType.WristRight];
        BodyProperties BP = bodyObject.GetComponent<BodyProperties>();

        Kinect.Joint bodyJoint = body.Joints[Kinect.JointType.SpineBase];
        bodyObject.transform.position = GetVector3FromJoint(bodyJoint);
        Kinect.Vector4 BodyOrientation = body.JointOrientations[Kinect.JointType.SpineBase].Orientation;
        bodyObject.transform.rotation = GetQuaterion(BodyOrientation);


        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {   
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;

            if (_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }
            Transform jointObj = bodyObject.transform.Find("Skeleton"+jt.ToString());
            jointObj.position = GetVector3FromJoint(sourceJoint);
            if (jointObj.name.Contains("Head")  && count%7==0)
            {
                JointsProperties jp = jointObj.GetComponent<JointsProperties>();
                //Debug.Log(jointObj.name + ": (" + jp.position.x + "," + jp.position.y + "," + jp.position.z + ")");
            }
            GameObject JointObject = jointObj.gameObject;
            JointsProperties joint = JointObject.GetComponent<JointsProperties>();
            joint.position = jointObj.position;
            joint.UpdateDistanceFromGround();
            joint.TrackingState = sourceJoint.TrackingState;
            JointObject.GetComponent<Renderer>().material.color = GetColorForState(sourceJoint.TrackingState);



            LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            if (targetJoint.HasValue)
            {
                lr.SetPosition(0, jointObj.position);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
                lr.startColor = GetColorForState(sourceJoint.TrackingState);
                lr.endColor = GetColorForState(targetJoint.Value.TrackingState);
            }
            else
            {
                lr.enabled = false;
            }
        }
    }

    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
            case Kinect.TrackingState.Tracked:
                return Color.green;

            case Kinect.TrackingState.Inferred:
                return Color.red;
            default:
                return Color.black;
        }
    }
      
    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * XHumanScalingFactor , joint.Position.Y * YHumanScalingFactor, joint.Position.Z);
    }

    private static Quaternion GetQuaterion(Kinect.Vector4 q)
    {
        Quaternion initial = Quaternion.identity;
        Quaternion rotation = new Quaternion(q.X, q.Y, q.Z, q.W) ;
        Vector3 rot = rotation.eulerAngles;
        Vector3 newRot = new Vector3(0f, rot.y, 0f);
        Quaternion newRotation = Quaternion.Euler(newRot);
        return (initial * newRotation);
    }

    private void OnApplicationQuit()
    {
        SaveLoad.Save();
    }
}
