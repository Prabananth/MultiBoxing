using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;


public class StartCircle : MonoBehaviour {
    private string TagName = "SkeletonBody";
    public static StartCircle SC;
    public static GameObject player;
    public static bool CanMove = false;
    public static bool CanStart = false;
    public Transform GroundPlane;
    //private static Transform SC = 
    private Vector3 InitialPosition;
    private Quaternion InitialRotation;
    private Vector3 LeftFootPosition = Vector3.zero;
    private Vector3 RightFootPosition = Vector3.zero;
    private Vector3 SpineBasePosition = Vector3.zero;
    private Vector3 FixedJointPosition = Vector3.zero;
    float TimeCount = 0f;
    public GeneralData GD;

    public MapJoins MJ;

    private Dictionary<int, Kinect.JointType> _JointMap = new Dictionary<int, Kinect.JointType>()
    {
        { 14, Kinect.JointType.AnkleLeft },
        { 13, Kinect.JointType.KneeLeft },
        { 12, Kinect.JointType.HipLeft },
        { 15, Kinect.JointType.FootLeft },

        { 18, Kinect.JointType.AnkleRight },
        { 17, Kinect.JointType.KneeRight },
        { 16, Kinect.JointType.HipRight },
        { 19, Kinect.JointType.FootRight },

        { 7, Kinect.JointType.HandLeft },
        { 21, Kinect.JointType.HandTipLeft },
        { 6, Kinect.JointType.WristLeft },
        { 5, Kinect.JointType.ElbowLeft },
        { 4, Kinect.JointType.ShoulderLeft },
        { 22, Kinect.JointType.ThumbLeft },

        { 11, Kinect.JointType.HandRight },
        { 23, Kinect.JointType.HandTipRight },
        { 10, Kinect.JointType.WristRight },
        { 9, Kinect.JointType.ElbowRight },
        { 8, Kinect.JointType.ShoulderRight },
        { 24, Kinect.JointType.ThumbRight },

        { 1, Kinect.JointType.SpineMid },
        { 20, Kinect.JointType.SpineShoulder },
        { 2, Kinect.JointType.Neck },
        { 3, Kinect.JointType.Head },
        { 0, Kinect.JointType.SpineBase }
    };


    // Use this for initialization
    void Start () {
        InitialPosition = transform.position;
        InitialRotation = transform.rotation;
        Debug.Log(InitialPosition + "s" + InitialRotation);
	}
	
	// Update is called once per frame
	void Update () {
        if (player != null && SecretData.FixedJoint != -1)
        {
            Kinect.JointType FixedJoint = _JointMap[SecretData.FixedJoint];
            Transform FixedJointT = player.transform.Find("Skeleton" + FixedJoint.ToString());
            Transform SpineBaseT = player.transform.Find("Skeleton" + "SpineBase");
            Vector3 NewFixedJointPosition = FixedJointT.position;
            Vector3 NewSpineBasePosition = SpineBaseT.position;
            Vector3 BufferPosition = new Vector3(0.01f,0.01f,0.01f);
            JointsProperties jpf = FixedJointT.gameObject.GetComponent<JointsProperties>();
            JointsProperties jps = SpineBaseT.gameObject.GetComponent<JointsProperties>();
            TimeCount += Time.deltaTime;
            if(jpf.TrackingState == Kinect.TrackingState.Tracked && jps.TrackingState == Kinect.TrackingState.Tracked)
            {
                TimeCount = 0f;
                if (CanMove && CanStart)
                {
                    bool SpineBaseChange = isChangedPosition(NewSpineBasePosition, SpineBasePosition);
                    bool FixedJointChange = isChangedPosition(NewFixedJointPosition, FixedJointPosition);
                    float xfactor = 0.15f;
                    float yfactor = 0.01f;
                    float zfactor = 0.1f;
                    if (GD != null && (GD.gestureName == "rightlunges" || GD.gestureName == "leftlunges"))
                    {
                        xfactor = 0.2f;
                    }
                    if (!GD.gestureName.ToLower().Equals("none") && FixedJointChange)
                    {
                        Vector3 playerPosition = player.transform.position;
                        Vector3 NewCirclePosition = new Vector3(playerPosition.x + xfactor, GroundPlane.position.y + yfactor, playerPosition.z + zfactor);
                        transform.position = NewCirclePosition;
                        transform.rotation = GetCircleRotation(player.transform.rotation);
                    }
                }
                else
                {
                    transform.position = InitialPosition;
                    transform.rotation = InitialRotation;
                }
                FixedJointPosition = NewFixedJointPosition;
                SpineBasePosition = NewSpineBasePosition;
            }
        }
        else
        {
            FixedJointPosition = Vector3.zero;
            //LeftFootPosition = Vector3.zero;
            //RightFootPosition = Vector3.zero;
        }
	} 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName))
        {
            CanStart = true;
            MJ.CanWork = true;
            MJ.StartCoroutine("StartTimer");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagName))
        {
            CanStart = false;
            MJ.CanWork = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(TagName) && !CanStart)
        {
            CanStart = true;
            MJ.CanWork = true;
        }
    }

    public bool isChangedPosition(Vector3 p1, Vector3 p2)
    {
        const float Buffer = 0.01f;
        float ForwardBuffer = Buffer;
        if (GD != null  && (GD.gestureName == "rightlunges"  || GD.gestureName == "leftlunges"))
        {
            //Debug.Log("Gesture is Lunges");
            ForwardBuffer = 0.05f;
        }
        //Debug.Log("ForwardBuffer:" + ForwardBuffer);

        if (Mathf.Abs(p1.x - p2.x) > Buffer)
            return true;
        if (Mathf.Abs(p1.y - p2.y) > Buffer)
            return true;
        if (Mathf.Abs(p1.z - p2.z) > Buffer && Mathf.Abs(p1.z - p2.z) > ForwardBuffer)
            return true;
        return false;
    }

    public void SetInitialPosistion()
    {
        transform.position = InitialPosition;
        //transform.position = getInitialPosition();
        transform.rotation = InitialRotation;
    }

    public Quaternion GetCircleRotation(Quaternion r)
    {
        Vector3 euler = r.eulerAngles;
        Vector3 newEuler = new Vector3(0, euler.y + 180, 0);
        return Quaternion.Euler(newEuler);
    }

    private Vector3 getInitialPosition()
    {
        return new Vector3(InitialPosition.x, GroundPlane.position.y + 0.01f, InitialPosition.z);
    }

    public Vector3 GetPostion()
    {
        return transform.position;
    }

    public void setPosition(Vector3 position)
    {
        transform.position = position;
    }
}
