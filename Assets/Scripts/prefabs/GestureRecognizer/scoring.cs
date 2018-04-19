using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;

public class scoring : MonoBehaviour {

    int score;
	// Use this for initialization
	void Start () {
        score = 0;

	}
	
	// Update is called once per frame
	void Update () {
        calculateScore();
	}
    public void setGesture(int choice)
    {
        BodyProperties BP = GameObject.Find("skeleton").GetComponentInChildren<BodyProperties>();
        if(BP != null)
        {
            if (choice == 1)
            {
                BP.GD.gestureName = "highknees";
                Text abc = GameObject.Find("Text 3").GetComponent<Text>();
                abc.text = "Do " + BP.GD.gestureName;
                BP.updateGesture("highknees");
            }
            else if (choice == 2)
            {
                BP.GD.gestureName = "jumpingjack";
                Text abc = GameObject.Find("Text 3").GetComponent<Text>();
                abc.text = "Do " + BP.GD.gestureName;
                BP.updateGesture("jumpingjack");
            }
            else if (choice == 3)
            {
                BP.GD.gestureName = "squats";
                Text abc = GameObject.Find("Text 3").GetComponent<Text>();
                abc.text = "Do " + BP.GD.gestureName;
                BP.updateGesture("squats");
            }
            else if (choice == 4)
            {
                BP.GD.gestureName = "rightlunges";
                Text abc = GameObject.Find("Text 3").GetComponent<Text>();
                abc.text = "Do " + BP.GD.gestureName;
                BP.updateGesture("rightlunges");
            }
            else if (choice == 5)
            {
                BP.GD.gestureName = "leftlunges";
                Text abc = GameObject.Find("Text 3").GetComponent<Text>();
                abc.text = "Do " + BP.GD.gestureName;
                BP.updateGesture("leftlunges");
            }
            else if (choice == 6)
            {
                BP.GD.gestureName = "none";
                Text abc = GameObject.Find("Text 3").GetComponent<Text>();
                abc.text = "Do " + BP.GD.gestureName;
                BP.updateGesture("none");
            }
        }
    }
    void calculateScore()
    {
        BodyProperties BP = GameObject.Find("skeleton").GetComponentInChildren<BodyProperties>();
        if (BP != null && BP.startGame)
        {
            if (BP.GD.accuracy > 70)
            {
                score += 5;
                Text abc = GameObject.Find("Text 4").GetComponent<Text>();
                abc.text = "Score = " + score.ToString();
                //BP.GD.accuracy = 0;
            }
        }
    }
}
