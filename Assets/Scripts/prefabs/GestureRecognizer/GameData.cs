using System.Collections.Generic;
using System;

[System.Serializable]
public class GameData {
    public static GameData Current = new GameData();
    public List<GestureData> Gestures = new List<GestureData>();
    public DateTime date;
    public int score;
    public List<float> Powers = new List<float>();
    public int NoOfHits = 0;

    public void UpdateGestureData(string gesture, float accuracy)
    {
        GestureData.currentGesture.date = DateTime.Now;
        GestureData.currentGesture.Gesture = gesture;
        GestureData.currentGesture.Accuracy = accuracy;
        Gestures.Add(GestureData.currentGesture);
    }

    public void NextPower(float power)
    {
        Powers.Add(power);
        NoOfHits++;
    }

    public void UpdateScore(int s)
    {
        score = s;
    }

}
