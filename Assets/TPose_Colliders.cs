using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPose_Colliders : MonoBehaviour {

   
    public static bool HandLeft;
    public static bool LeftElbow;
    public static bool HandRight;
    public static bool RightElbow;
    int count = 0;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("SkeletonHandLeft"))
        {
            HandLeft = true;
        }
        else if (other.gameObject.name.Equals("SkeletonElbowLeft"))
        {
            LeftElbow = true;
        }
        else if (other.gameObject.name.Equals("SkeletonHandRight"))
        {
            HandRight = true;
        }
        else if (other.gameObject.name.Equals("SkeletonElbowRight"))
        {
            RightElbow = true;
        }
        else
        {
            return;
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Equals("SkeletonHandLeft"))
        {
            HandLeft = true;
        }
        else if (other.gameObject.name.Equals("SkeletonElbowLeft"))
        {
            LeftElbow = true;
        }
        else if (other.gameObject.name.Equals("SkeletonHandRight"))
        {
            HandRight = true;
        }
        else if (other.gameObject.name.Equals("SkeletonElbowRight"))
        {
            RightElbow = true;
        }
        else
        {
            return;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("SkeletonHandLeft"))
        {
            HandLeft = false;
        }
        else if (other.gameObject.name.Equals("SkeletonElbowLeft"))
        {
            LeftElbow = false;
        }
        else if (other.gameObject.name.Equals("SkeletonHandRight"))
        {
            HandRight = false;
        }
        else if (other.gameObject.name.Equals("SkeletonElbowRight"))
        {
            RightElbow = false;
        }
        else
        {
            return;
        }
    }
}
