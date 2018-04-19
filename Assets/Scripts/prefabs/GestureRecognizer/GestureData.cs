using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GestureData{
    public static GestureData currentGesture = new GestureData();
    public DateTime date;
    public string Gesture;
    public float Accuracy;
}
