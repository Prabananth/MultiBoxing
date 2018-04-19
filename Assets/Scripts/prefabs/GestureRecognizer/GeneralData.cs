using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;

public class GeneralData
{
    public bool gestureTrue;
    public float FrameTime;
    public string totalTime;
    public float accuracy;
    public string gestureName;
    public float MaxFrameTime;
    public float MinFrameTime;
    public GeneralData()
    {
        gestureTrue = false;
        totalTime = "";
        accuracy = 0;
        gestureName = null;
        FrameTime = 0f;
        MaxFrameTime = 0f;
        MinFrameTime = 0f;
    }
}
