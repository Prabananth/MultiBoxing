using UnityEngine;

public class T_Pose : MonoBehaviour {
    private GameObject TPOSE;
    public MapJoins MJ;
    void Start()
    {
        TPOSE = GameObject.Find("TPose");
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void TPosition()
    {
        if (TPose_Colliders.HandLeft == true && TPose_Colliders.HandRight == true && TPose_Colliders.LeftElbow == true && TPose_Colliders.RightElbow == true)
        {
            TPOSE.SetActive(false);
            MJ.Initialized = true;
            MJ.Map();
        }
    }
}
