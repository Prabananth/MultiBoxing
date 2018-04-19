using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
class SecretData
{
    public int noOfFrames;
    public float handlengthdelta;
    public float shoulderdelta;
    public float heightdelta;
    public float angleadjust;
    public int NoOfCoins;
    public float Noofcollisions;
    public float totalcollisions;
    public bool started;
    public GameObject acc;
    public int totalInitial;
    public int InitialCollisions;
    public DateTime start, stop;
    public TimeSpan timeTaken;
    public bool timestarted;
    public int totalFrames;
    public int rate;
    public int MaxFramesAfterCollision;
    public string gestureType;
    public float realhandlength;
    public float realheight;
    public float realshoulder;
    public float realfootlevel;
    public static int FixedJoint;
    public SecretData()
    {
        realhandlength = 0.0f;
        realheight = 0.0f;
        realshoulder = 0.0f;
        realfootlevel = 0.0f;
        noOfFrames = 0;
        handlengthdelta = 0.0f;
        shoulderdelta = 0.0f;
        heightdelta = 0.0f;
        angleadjust = 0.0f;
        NoOfCoins = 0;
        Noofcollisions = 0;
        totalcollisions = 0;
        started = false;
        totalInitial = 0;
        InitialCollisions = 0;
        timestarted = false;
        totalFrames = 0;
        gestureType = " ";
        FixedJoint = -1;
        MaxFramesAfterCollision = 30;
    }
}
