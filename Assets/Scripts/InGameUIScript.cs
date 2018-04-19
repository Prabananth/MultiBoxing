using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InGameUIScript: NetworkBehaviour{
    public Text ScoreText;
    public Text PowerText;
    public Text TimeInSecondsText;
    public Text ExerciseText;
    public Text AccuracyText;
    public Text CaloriesText;
    public Text HeartRateText;

    public string[] Exercises = {"Punches", "Highness", "Jumpimg Jack", "Squat", "Left Lunges", "Right Lunges"};

    private NetworkIdentity objNetId;

    [SyncVar(hook = "OnTimeChanged")]
    private int _time;
    [SyncVar(hook = "OnPowerChanged")]
    private int _power;
    [SyncVar(hook = "OnScoreChanged")]
    private int _score;
    [SyncVar(hook = "OnAccuracyChanged")]
    private int _accuracy;
    [SyncVar(hook = "OnCaloriesChanged")]
    private float _calories;
    [SyncVar(hook = "OnHeartRateChanged")]
    private float _heartrate;
    [SyncVar(hook = "OnExerciseChanged")]
    private int _exercise;

    public int Time
    {
        get { return _time;}
        set
        {
            _time = value;
            TimeInSecondsText.text = _time == -1 ? "--" : _time.ToString();
        }
    }

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            ScoreText.text = _score == -1 ? "--" : _score.ToString();
        }
    }

    public int Accuracy
    {
        get { return _accuracy; }
        set
        {
            _accuracy = value;
            AccuracyText.text = _accuracy == -1 ? "--" : _accuracy.ToString();
        }
    }

    public int Power
    {
        get { return _power; }
        set
        {
            _power = value;
            PowerText.text = _power == -1 ? "--/--" : _power.ToString();
        }
    }

    public float Calories
    {
        get { return _calories; }
        set
        {
            _calories = value;
            CaloriesText.text = _calories== -1f ? "--/--" : _calories.ToString();
        }
    }
    public float HeartRate
    {
        get { return _heartrate; }
        set
        {
            _heartrate = value;
            HeartRateText.text = _heartrate == -1f ? "--/--" : _heartrate.ToString();
        }
    }

    public int Exercise
    {
        get { return _exercise; }
        set
        {
            _exercise = value;
            ExerciseText.text = _exercise == -1 ? "REST" :   Exercises[_exercise].ToUpper();
            //CmdExerciseChanged(_exercise);
            //ExerciseChanged(_exercise);
        }
    }

    public void OnTimeChanged(int time)
    {
        TimeInSecondsText.text = time == -1 ? "--" : time.ToString();
    }

    public void OnScoreChanged(int score)
    {
        ScoreText.text = score == -1 ? "--" : score.ToString();
    }

    public void OnPowerChanged(int power)
    {
        PowerText.text = power == -1 ? "--/--" : power.ToString();
    }

    public void OnAccuracyChanged(int accuracy)
    {
        AccuracyText.text = accuracy == -1 ? "--" : accuracy.ToString();
    }

    public void OnCaloriesChanged(float calories)
    {
        CaloriesText.text = calories == -1f ? "--/--" : calories.ToString();
    }
    
    public void OnHeartRateChanged(float heartrate)
    {
        HeartRateText.text = heartrate == -1f ? "--/--" : heartrate.ToString();
    }

    public void OnExerciseChanged(int exercise)
    {
        ExerciseText.text = _exercise == -1 ? "REST" : Exercises[_exercise].ToUpper();
    }

    /*[ClientRpc]
    public void RpcExerciseChanged(string exercise)
    {
        ExerciseText.text = _exercise == "" ? "REST" : _exercise.ToUpper();
    }

    [Command]
    public void CmdExerciseChanged(string exercise)
    {
        objNetId = ExerciseText.GetComponent<NetworkIdentity>();
        objNetId.AssignClientAuthority(connectionToClient);
        RpcExerciseChanged(exercise);
        objNetId.RemoveClientAuthority(connectionToClient);
    }

    [Server]
    public void ExerciseChanged(string exercise)
    {
        objNetId = ExerciseText.GetComponent<NetworkIdentity>();
        //objNetId.AssignClientAuthority(connectionToClient);
        RpcExerciseChanged(exercise);
        //objNetId.RemoveClientAuthority(connectionToClient);
    }*/

    public override void OnStartLocalPlayer()
    {
        //ExerciseChanged(_exercise);
        OnExerciseChanged(_exercise);
        //CmdExerciseChanged(_exercise);
        //RpcExerciseChanged(_exercise);
    }
}

