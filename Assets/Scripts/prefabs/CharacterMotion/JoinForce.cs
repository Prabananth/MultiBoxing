using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Kinect = Windows.Kinect;

public class JoinForce : NetworkBehaviour {

    private Vector3 previousPosition;
    private TimeSpan previousFrameTime;
    private int LastFrameCount;
    public float P_FinalVelocity;
    float TimeInterval;
    float time = 1f;
    public Queue<float> FinalVelocities = new Queue<float>(4);
    public float sum = 0;


    // Use this for initialization
    void Start () {
        previousPosition = CharacterMotion.RightWrist;
        P_FinalVelocity = 0f;
        TimeInterval = 0f;
        LastFrameCount = 1;
        previousFrameTime = new TimeSpan(0,0,0,0,0);
    }
	
	// Update is called once per frame
	void Update () {
        if (!isServer)
        {
            return;
        }
        int FrameCount = BodySourceManager.GetFrameCount();
        TimeSpan FrameTime = BodySourceManager.GetKinectDataTime();
        Vector3 Position = CharacterMotion.RightWrist;
       
        if(FrameCount > 1)
        {
            if(FrameCount > LastFrameCount)
            {
                float distance = Vector3.Distance(previousPosition, transform.position);
                if(distance == 0f)
                {
                    P_FinalVelocity = 0f;
                }

                if (time <  0f)
                {
                    float poped_out_value = 0;
                    TimeInterval = GetTimeSpanDifference(FrameTime, previousFrameTime);
                    P_FinalVelocity = distance / TimeInterval;
                    if(FinalVelocities.Count >= 4)
                    {
                        poped_out_value = FinalVelocities.Dequeue();
                    }
                    if (FinalVelocities.Count < 4)
                    {
                        FinalVelocities.Enqueue(P_FinalVelocity);
                        sum += P_FinalVelocity - poped_out_value;
                    }
                }
                else
                {
                    time -= Time.deltaTime;
                }

                previousPosition = Position;
                LastFrameCount = FrameCount;
                previousFrameTime = FrameTime;
            }
        }
        else
        {
            previousPosition = Position;
            previousFrameTime = FrameTime;
            P_FinalVelocity = 0;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        previousPosition = CharacterMotion.RightWrist;
        P_FinalVelocity = 0f;
        TimeInterval = 0f;
    }

    private void OnApplicationQuit()
    {
    }

    private float  GetTimeSpanDifference(TimeSpan S1, TimeSpan S2)
    {
        return Mathf.Abs(((float)(S1.Ticks - S2.Ticks))/100000000000f);
    }

   
}

