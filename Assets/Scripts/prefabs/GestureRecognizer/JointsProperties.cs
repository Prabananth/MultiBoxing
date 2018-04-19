using System.Collections;
using System.Collections.Generic;

using Kinect = Windows.Kinect;
using UnityEngine;
using UnityEngine.UI;
using System;

public class JointsProperties : MonoBehaviour {


    public string name;
    public Kinect.TrackingState TrackingState;
    public double distanceFromGround;
    public Kinect.Vector4 floor;
    public Vector3 position;
    public HashSet<GameObject> adjacentJoints;
    public Dictionary<GameObject, float> distanceBtwnJoints;


    public ArrayList HandColliders = new ArrayList();
    int HandCollider_length = 0;	
	// Update is called once per frame
	void Update () {
        //UpdateDistanceFromGround();
	}

    public void UpdateDistanceFromGround()
    {
        double numerator = floor.X * transform.position.x + floor.Y * transform.position.y + floor.Z * transform.position.z + floor.W;
        double denominator = Math.Sqrt(floor.X * floor.X + floor.Y * floor.Y + floor.Z * floor.Z);

        distanceFromGround = numerator / denominator;
    }

    public void AddNeighbours(GameObject g)
    {
        adjacentJoints.Add(g);
        distanceBtwnJoints.Add(g, CalculateDistance(g));
    }

    public float CalculateDistance(GameObject g)
    {
        return Vector3.Distance(transform.position, g.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        /*BodyProperties Bp = new BodyProperties();

        //if(name.Equals("HandLeft") || name.Equals("HandRight"))
        //{
            //if (other.gameObject.CompareTag(name))
            if(other.name.Contains(this.name))
            {
            /*
            bool anglevalid = false;
            GameObject shoulder = transform.Find("ShoulderRight").gameObject;
            GameObject elbow = transform.Find("ElbowRight").gameObject;
            GameObject hand = transform.Find("WristRight").gameObject;
            Vector3 a = shoulder.transform.position - elbow.transform.position;
            Vector3 b = hand.transform.position - elbow.transform.position;

            if (Vector3.Angle(a, b) > 160 && Vector3.Angle(a, b) < 200)
                anglevalid = true;

            else
                countText.text = "Wrong angle of elbow";

            if(anglevalid)
                other.gameObject.SetActive(false);
            */
             /*   if (HandCollider_length == 0 || !HandColliders[HandCollider_length-1].Equals(other.gameObject.name))
                {
                    //Debug.Log(HandColliders.Count);
                    HandColliders.Add(other.gameObject.name);
                    HandCollider_length++;
                }

                BodyProperties BP = transform.parent.gameObject.GetComponent<BodyProperties>();
                bool footStatus = BP.CheckLegAboveGround();
                float footDist = BP.findDistanceBtwnFoots();
                BP.NoOfCoins--;*/
            //if (BP.NoOfCoins == 0)
                //Debug.Log(footStatus + " - " + footDist + "\n");
            
        //}
    }

}
