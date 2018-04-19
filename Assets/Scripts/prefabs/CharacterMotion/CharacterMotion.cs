using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;
using System;
using UnityEngine.Networking;


public class CharacterMotion : NetworkBehaviour {

    public GameObject BodySourceManager;
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;
    private Animator AnimatorComp;
    public static Vector3 RightWrist;
    public static Vector3 InitialPosition;
    public static bool CanWork;

    private readonly Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
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
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder},
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head }
    };


    private readonly Dictionary<Kinect.JointType, HumanBodyBones> _CharacaterMap = new Dictionary<Kinect.JointType, HumanBodyBones>()
    {
        { Kinect.JointType.FootLeft, HumanBodyBones.LeftToes },
        { Kinect.JointType.AnkleLeft, HumanBodyBones.LeftFoot },
        { Kinect.JointType.KneeLeft, HumanBodyBones.LeftLowerLeg },
        { Kinect.JointType.HipLeft, HumanBodyBones.LeftUpperLeg },

        { Kinect.JointType.FootRight, HumanBodyBones.RightToes },
        { Kinect.JointType.AnkleRight, HumanBodyBones.RightFoot },
        { Kinect.JointType.KneeRight, HumanBodyBones.RightLowerLeg },
        { Kinect.JointType.HipRight,  HumanBodyBones.RightUpperLeg},

        { Kinect.JointType.HandTipLeft, HumanBodyBones.LeftMiddleDistal },
        { Kinect.JointType.ThumbLeft, HumanBodyBones.LeftThumbDistal },
        { Kinect.JointType.HandLeft, HumanBodyBones.LeftMiddleProximal },
        { Kinect.JointType.WristLeft, HumanBodyBones.LeftHand },
        { Kinect.JointType.ElbowLeft, HumanBodyBones.LeftLowerArm },
        { Kinect.JointType.ShoulderLeft, HumanBodyBones.LeftUpperArm },

        { Kinect.JointType.HandTipRight, HumanBodyBones.RightMiddleDistal },
        { Kinect.JointType.ThumbRight, HumanBodyBones.RightThumbDistal },
        { Kinect.JointType.HandRight, HumanBodyBones.RightMiddleProximal },
        { Kinect.JointType.WristRight, HumanBodyBones.RightHand },
        { Kinect.JointType.ElbowRight, HumanBodyBones.RightLowerArm },
        { Kinect.JointType.ShoulderRight, HumanBodyBones.RightUpperArm },

        { Kinect.JointType.SpineBase, HumanBodyBones.Hips },
        { Kinect.JointType.SpineMid, HumanBodyBones.Chest },
        { Kinect.JointType.SpineShoulder, HumanBodyBones.UpperChest },
        { Kinect.JointType.Neck, HumanBodyBones.Neck },
        { Kinect.JointType.Head, HumanBodyBones.Head }
    };

    Quaternion InitialSpineBase;
    Quaternion InitialSpineMid;
    Quaternion InitialNeckQ;
    Quaternion InitialSpineShoulder;
    Quaternion InitialShoulderLeft;
    Quaternion InitialShoulderRight;
    Quaternion InitialElbowLeft;
    Quaternion InitialWristLeft;
    Quaternion InitialHandLeft;
    Quaternion InitialElbowRight;
    Quaternion InitialWristRight;
    Quaternion InitialHandRight;
    Quaternion InitialKneeLeft;
    Quaternion InitialAnkleLeft;
    Quaternion InitialKneeRight;
    Quaternion InitialAnkleRight;

    private void Awake()
    {
        SaveLoad.Load();
    }

    // Use this for initialization
    void Start()
    {
        InitialPosition = transform.position;
        Debug.Log(InitialPosition);
        AnimatorComp = GetComponent<Animator>();
        CanWork = false;

        InitialSpineBase = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.SpineBase]).rotation;
        InitialSpineMid = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.SpineMid]).rotation;
        InitialNeckQ = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.Neck]).rotation;
        InitialSpineShoulder = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.SpineShoulder]).rotation;
        InitialShoulderLeft = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.ShoulderLeft]).rotation;
        InitialShoulderRight = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.ShoulderRight]).rotation;
        InitialElbowLeft = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.ElbowLeft]).rotation;
        InitialWristLeft = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.WristLeft]).rotation;
        InitialHandLeft = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.HandLeft]).rotation;
        InitialElbowRight = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.ElbowRight]).rotation;
        InitialWristRight = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.WristRight]).rotation;
        InitialHandRight = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.HandRight]).rotation;
        InitialKneeLeft = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.KneeLeft]).rotation;
        InitialAnkleLeft = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.AnkleLeft]).rotation;
        InitialKneeRight = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.KneeRight]).rotation;
        InitialAnkleRight = AnimatorComp.GetBoneTransform(_CharacaterMap[Kinect.JointType.AnkleRight]).rotation;

    }

    // Update is called once per frame
    void Update()
    {
        if (!CanWork || !isServer)
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
                RefreshBodyObject(body);
            }
            i++;
        }


    }

    private void RefreshBodyObject(Kinect.Body body)
    {
        if (AnimatorComp != null)
        {

            Vector3 floorPlane = new Vector3(0f, 0f, 0f);

            var joints = body.JointOrientations;

            Quaternion comp = Quaternion.FromToRotation(new Vector3(floorPlane.x, floorPlane.y, floorPlane.z), Vector3.up);

            Quaternion SpineBase = InitialSpineBase * VToQ(joints[Kinect.JointType.SpineBase].Orientation, comp);
            Quaternion SpineMid = InitialSpineMid * VToQ(joints[Kinect.JointType.SpineMid].Orientation, comp);
            Quaternion NeckQ = InitialNeckQ * VToQ(joints[Kinect.JointType.Neck].Orientation, comp);
            Quaternion SpineShoulder = InitialSpineShoulder * VToQ(joints[Kinect.JointType.SpineShoulder].Orientation, comp);
            Quaternion ShoulderLeft = InitialShoulderLeft * VToQ(joints[Kinect.JointType.ShoulderLeft].Orientation, comp);
            Quaternion ShoulderRight = InitialShoulderRight * VToQ(joints[Kinect.JointType.ShoulderRight].Orientation, comp);
            Quaternion ElbowLeft = InitialElbowLeft * VToQ(joints[Kinect.JointType.ElbowLeft].Orientation, comp);
            Quaternion WristLeft = InitialWristLeft * VToQ(joints[Kinect.JointType.WristLeft].Orientation, comp);
            Quaternion HandLeft = InitialHandLeft * VToQ(joints[Kinect.JointType.HandLeft].Orientation, comp);
            Quaternion ElbowRight = InitialElbowRight * VToQ(joints[Kinect.JointType.ElbowRight].Orientation, comp);
            Quaternion WristRight = InitialWristRight * VToQ(joints[Kinect.JointType.WristRight].Orientation, comp);
            Quaternion HandRight = InitialHandRight * VToQ(joints[Kinect.JointType.HandRight].Orientation, comp);
            Quaternion KneeLeft = InitialKneeLeft * VToQ(joints[Kinect.JointType.KneeLeft].Orientation, comp);
            Quaternion AnkleLeft = InitialAnkleLeft * VToQ(joints[Kinect.JointType.AnkleLeft].Orientation, comp);
            Quaternion KneeRight = InitialKneeRight * VToQ(joints[Kinect.JointType.KneeRight].Orientation, comp);
            Quaternion AnkleRight = InitialAnkleRight * VToQ(joints[Kinect.JointType.AnkleRight].Orientation, comp);



            /*Quaternion SpineBase = VToQ(joints[Kinect.JointType.SpineBase].Orientation, comp);
            Quaternion SpineMid = VToQ(joints[Kinect.JointType.SpineMid].Orientation, comp);
            Quaternion NeckQ =  VToQ(joints[Kinect.JointType.Neck].Orientation, comp);
            Quaternion SpineShoulder = VToQ(joints[Kinect.JointType.SpineShoulder].Orientation, comp);
            Quaternion ShoulderLeft = VToQ(joints[Kinect.JointType.ShoulderLeft].Orientation, comp);
            Quaternion ShoulderRight = VToQ(joints[Kinect.JointType.ShoulderRight].Orientation, comp);
            Quaternion ElbowLeft = VToQ(joints[Kinect.JointType.ElbowLeft].Orientation, comp);
            Quaternion WristLeft = VToQ(joints[Kinect.JointType.WristLeft].Orientation, comp);
            Quaternion HandLeft = VToQ(joints[Kinect.JointType.HandLeft].Orientation, comp);
            Quaternion ElbowRight = VToQ(joints[Kinect.JointType.ElbowRight].Orientation, comp);
            Quaternion WristRight = VToQ(joints[Kinect.JointType.WristRight].Orientation, comp);
            Quaternion HandRight = VToQ(joints[Kinect.JointType.HandRight].Orientation, comp);
            Quaternion KneeLeft = VToQ(joints[Kinect.JointType.KneeLeft].Orientation, comp);
            Quaternion AnkleLeft = VToQ(joints[Kinect.JointType.AnkleLeft].Orientation, comp);
            Quaternion KneeRight = VToQ(joints[Kinect.JointType.KneeRight].Orientation, comp);
            Quaternion AnkleRight = VToQ(joints[Kinect.JointType.AnkleRight].Orientation, comp);*/


            Quaternion q = transform.rotation;
            transform.rotation = Quaternion.identity;

            MoveAvatarJoints(Kinect.JointType.SpineBase, SpineBase * Quaternion.AngleAxis(180, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-0, new Vector3(0, 0, 1)));

            MoveAvatarJoints(Kinect.JointType.SpineMid, SpineMid * Quaternion.AngleAxis(180, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-0, new Vector3(0, 0, 1)));

            MoveAvatarJoints(Kinect.JointType.SpineShoulder, SpineShoulder * Quaternion.AngleAxis(180, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-0, new Vector3(0, 0, 1)));

            MoveAvatarJoints(Kinect.JointType.Neck, NeckQ * Quaternion.AngleAxis(180, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-0, new Vector3(0, 0, 1)));

            MoveAvatarJoints(Kinect.JointType.ShoulderRight, ElbowRight * Quaternion.AngleAxis(90, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(90, new Vector3(0, 0, 1)));

            MoveAvatarJoints(Kinect.JointType.ElbowRight, WristRight * Quaternion.AngleAxis(90, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(90, new Vector3(0, 0, 1)) * Quaternion.AngleAxis(90, new Vector3(1, 0, 0)));

            //MoveAvatarJoints(Kinect.JointType.WristRight, HandRight * Quaternion.AngleAxis(90, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(90, new Vector3(0, 0, 1)));

            MoveAvatarJoints(Kinect.JointType.ShoulderLeft, ElbowLeft * Quaternion.AngleAxis(-90, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-90, new Vector3(0, 0, 1)));

            MoveAvatarJoints(Kinect.JointType.ElbowLeft, WristLeft * Quaternion.AngleAxis(-90, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-90, new Vector3(0, 0, 1)));

            //MoveAvatarJoints(Kinect.JointType.WristLeft, HandLeft * Quaternion.AngleAxis(-90, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-90, new Vector3(0, 0, 1)));

            MoveAvatarJoints(Kinect.JointType.HipRight, KneeRight * Quaternion.AngleAxis(90, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-0, new Vector3(0, 0, 1)) * Quaternion.AngleAxis(180, new Vector3(1, 0, 0)));

            MoveAvatarJoints(Kinect.JointType.KneeRight, AnkleRight * Quaternion.AngleAxis(90, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-0, new Vector3(0, 0, 1)) * Quaternion.AngleAxis(180, new Vector3(1, 0, 0)));

            MoveAvatarJoints(Kinect.JointType.HipLeft, KneeLeft * Quaternion.AngleAxis(-90, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-0, new Vector3(0, 0, 1)) * Quaternion.AngleAxis(180, new Vector3(1, 0, 0)));

            MoveAvatarJoints(Kinect.JointType.KneeLeft, AnkleLeft * Quaternion.AngleAxis(-90, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-0, new Vector3(0, 0, 1)) * Quaternion.AngleAxis(180, new Vector3(1, 0, 0)));

            //transform.rotation = q;
            PlayerMovement(body.Joints[Kinect.JointType.SpineBase]);
            RightWrist = GetVector3FromJoint(body.Joints[Kinect.JointType.WristRight]);
        }
        else
        {
            Debug.Log("ANimator null");
        }
    }
    private void PlayerMovement(Kinect.Joint PlayerPosition)
    {
        float MoveBuffer = 2.25f;
        transform.position = new Vector3(-PlayerPosition.Position.X + InitialPosition.x, InitialPosition.y, (PlayerPosition.Position.Z - MoveBuffer + InitialPosition.z));
    }

    private Quaternion VToQ(Windows.Kinect.Vector4 kinectQ, Quaternion comp)
    {
        return Quaternion.Inverse(comp) * (new Quaternion(-kinectQ.X, -kinectQ.Y, kinectQ.Z, kinectQ.W));
    }

    public void MoveAvatarJoints(Kinect.JointType jt, Quaternion rotation)
    {

        Transform tran = AnimatorComp.GetBoneTransform(_CharacaterMap[jt]);
        tran.rotation = rotation;

    }
    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z);
    }

    private void OnApplicationQuit()
    {
        SaveLoad.Save();
    }
}



