using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MapJoins : NetworkBehaviour {
    public GameObject ElbowLeft;
    public GameObject HandLeft;
    public GameObject ElbowRight;
    public GameObject HandRight;
    public GameObject KneeLeft;
    public GameObject AnkleLeft;
    public GameObject KneeRight;
    public GameObject AnkleRight;

    public InGameUIScript GameMainScreen;

    private int gestureCount,punchCount;
    private string GestureName;

    public static Dictionary<string, int> selectedExercise;
    public static List<string> ExerciseList;
    public static List<string> selectedJoints;

    public static int breakTime, sets;

    public bool isPower, isAccuracy;

    private NetworkIdentity objNetId;

    public readonly Dictionary<int, GameObject[]> dictionary = new Dictionary<int, GameObject[]>();

    public readonly Dictionary<string, int> ExercisesMap = new Dictionary<string, int>()
    {
        {"punches", 0},
        {"highness", 1},
        {"jumpingjack", 2},
        {"squat", 3},
        {"leftlunges", 4},
        {"rightlunges", 5}
    };

    GameObject[] PunchBags;
    GameObject[] Hand;
    GameObject[] Elbow;
    GameObject[] Knee;
    GameObject[] Ankle;

    public readonly Dictionary<string, GameObject> JointMap = new Dictionary<string, GameObject>();
    public readonly Dictionary<GameObject, string> SkelCharJointMap = new Dictionary<GameObject, string>();
    public readonly Dictionary<GameObject, GameObject[]> JointPunchBagMap = new Dictionary<GameObject, GameObject[]>();

    public GameObject CurrentPunchBag;
    public GameObject CurrentJoint;

    public bool CanWork = false;
    public bool Initialized = false; 
    
    // Use this for initialization
    void Start () {
        GameMainScreen.Power = -1;
        GameMainScreen.Score = -1;
        gestureCount = 0;
        punchCount = 0;
        GameMainScreen.Exercise = -1;
        GestureName = "None";
        isPower = false;
        isAccuracy = false;
        PunchBags = GameObject.FindGameObjectsWithTag("PunchBags");
        RandomFirst();
        selectedExercise = new Dictionary<string, int>();
        selectedJoints = new List<string>();
        ExerciseList = new List<string>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(3);
        Map();
        yield break;
    }

    IEnumerator ReduceTimer()
    {
        while ( GameMainScreen.Time >= 0)
        {   
            yield return new WaitForSeconds(1);
            GameMainScreen.Time--;
            isPower = false;
            isAccuracy = false;
        }
        Map();
    }
     
    void RandomFirst()
    {
        Hand = new GameObject[] { PunchBags[0], PunchBags[1], PunchBags[4], PunchBags[5], PunchBags[6] };
        Elbow = new GameObject[] { PunchBags[5], PunchBags[6] };
        Knee = new GameObject[] { PunchBags[0], PunchBags[1], PunchBags[4] };
        Ankle = new GameObject[] { PunchBags[2], PunchBags[3], PunchBags[4], PunchBags[5], PunchBags[6] };

        JointMap.Add("HandLeft", HandLeft);
        JointMap.Add("HandRight", HandRight);
        JointMap.Add("ElbowLeft", ElbowLeft);
        JointMap.Add("ElbowRight", ElbowRight);
        JointMap.Add("KneeLeft", KneeLeft);
        JointMap.Add("KneeRight", KneeRight);
        JointMap.Add("AnkleLeft", AnkleLeft);
        JointMap.Add("AnkleRight", AnkleRight);

        JointPunchBagMap.Add(HandLeft, Hand);
        JointPunchBagMap.Add(HandRight, Hand);
        JointPunchBagMap.Add(ElbowLeft, Elbow);
        JointPunchBagMap.Add(ElbowRight, Elbow);
        JointPunchBagMap.Add(KneeLeft, Knee);
        JointPunchBagMap.Add(KneeRight, Knee);
        JointPunchBagMap.Add(AnkleLeft, Ankle);
        JointPunchBagMap.Add(AnkleRight, Ankle);

        SkelCharJointMap.Add(HandLeft, "HandLeft");
        SkelCharJointMap.Add(HandRight, "HandRight");
        SkelCharJointMap.Add(ElbowLeft, "ElbowLeft");
        SkelCharJointMap.Add(ElbowRight, "ElbowRight");
        SkelCharJointMap.Add(KneeLeft, "KneeLeft");
        SkelCharJointMap.Add(KneeRight, "KneeRight");
        SkelCharJointMap.Add(AnkleLeft, "AnkleLeft");
        SkelCharJointMap.Add(AnkleRight, "AnkleRight");
    }

    public void Map()
    {
        if (!CanWork || !Initialized ||!isServer)
        {
            return;
        }
        GameMainScreen.Time = 15;
        StopCoroutine("ReduceTimer");
        StartCoroutine("ReduceTimer");
        if (CurrentPunchBag)
        {
            Render(CurrentPunchBag, new Color32(38, 23, 223, 255));
            RpcRender(CurrentPunchBag, new Color32(38, 23, 223, 255));
            CmdRender(CurrentPunchBag, new Color32(38, 23, 223, 255));
        }
        if (CurrentJoint)
        {
            Render(CurrentJoint, Color.white);
            RpcRender(CurrentJoint, Color.white);
            CmdRender(CurrentJoint, Color.white);
        }
        if (!isPower)
        {
            GameMainScreen.Power = -1;
        }
        if (!isAccuracy)
        {
            GameMainScreen.Accuracy = -1;
        }
        if(gestureCount > 0)
        {
            DoGesture(GestureName);
        }else if(punchCount > 0)
        {
            DoPunches();
        }
        else
        {
            int n = Random.Range(1, 2);
            Debug.Log(n);
            switch (n)
            {
                case 1 :
                    CurrentJoint = null;
                    CurrentPunchBag = null;
                    int ExerciseNo = Random.Range(0, ExerciseList.Count);
                    Debug.Log(ExerciseNo);
                    GestureName = ExerciseList[ExerciseNo];
                    Debug.Log(GestureName);
                    gestureCount = selectedExercise[GestureName];
                    ExerciseList.Remove(GestureName);
                    selectedExercise.Remove(GestureName);
                    GameMainScreen.Exercise = ExercisesMap[GestureName.ToLower()];
                    DoGesture(GestureName);
                    break;
                case 2 : GestureName = "None";
                    punchCount = 5;
                    GameMainScreen.Exercise = ExercisesMap["PUNCHES".ToLower()];
                    DoPunches();
                    break;
            }
        }
        
    }

    [ClientRpc]
    void RpcRender(GameObject objectname, Color color)
    {
        objectname.GetComponent<Renderer>().material.color = color;
    }

    [Command]
    void CmdRender(GameObject objectname, Color color)
    {
        objNetId = objectname.GetComponent<NetworkIdentity>();
        objNetId.AssignClientAuthority(connectionToClient);
        objectname.GetComponent<Renderer>().material.color = color;
        RpcRender(objectname, color);
        objNetId.RemoveClientAuthority(connectionToClient);
    }

    [Server]
    void Render(GameObject objectname, Color color)
    {
        objNetId = objectname.GetComponent<NetworkIdentity>();
        objNetId.AssignClientAuthority(connectionToClient);
        objectname.GetComponent<Renderer>().material.color = color;
        RpcRender(objectname, color);
        objNetId.RemoveClientAuthority(connectionToClient);
    }

    public void setPower(GameObject joint)
    {
        JoinForce jf = joint.GetComponent<JoinForce>();
        GameMainScreen.Power = (int) jf.P_FinalVelocity;
        
        Debug.Log("Collided:"+GameMainScreen.Power.ToString());
        float [] FV = jf.FinalVelocities.ToArray();
        int length = jf.FinalVelocities.Count;
        GameMainScreen.Power = (int) jf.sum / length;
        GameData.Current.NextPower(GameMainScreen.Power);
        //setPowerText();
    }


    public void DoGesture(string gestureName)
    {
        BodyProperties BP = GameObject.Find("skeleton").GetComponentInChildren<BodyProperties>();
        if(BP != null)
        {
            BP.updateGesture(gestureName);
            gestureCount--;
        }
    }
    public void DoPunches()
    {
        BodyProperties BP = GameObject.Find("skeleton").GetComponentInChildren<BodyProperties>();
        if (BP != null)
        {
            BP.updateGesture("None");
        }

        CurrentJoint = JointMap[selectedJoints[Random.Range(0, selectedJoints.Count)]];
        GameObject[] CurrentPunchBagSet = JointPunchBagMap[CurrentJoint];
        CurrentPunchBag = CurrentPunchBagSet[Random.Range(0,CurrentPunchBagSet.Length)];
        Render(CurrentJoint, new Color32(38, 23, 223, 255));
        RpcRender(CurrentJoint, new Color32(38, 23, 223, 255));
        CmdRender(CurrentJoint, new Color32(38, 23, 223, 255));
        Render(CurrentPunchBag, Color.red);
        RpcRender(CurrentPunchBag, Color.red);
        CmdRender(CurrentPunchBag, Color.red);
        
        punchCount--;
    }
}
