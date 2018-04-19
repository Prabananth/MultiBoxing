using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GUIScript : MonoBehaviour {
    //private Dictionary<string, int> selectedExercise;
    //private List<string> selectedJoints;
    string currExercise;
    private int breakTime, sets;
    private string user;
    private GameObject loginUI, inter, skel;
    public NetManager NetworkManager;
    public GameObject GUI;
    public MapJoins MJ;
	public static GUIScript path = new GUIScript ();
	public string CreateFolder;

	// Use this for initialization
	void Start () {
        loginUI = GameObject.Find("Login");
        inter = GameObject.Find("Interface");
        skel = GameObject.Find("Skeleton");
        inter.SetActive(false);
        skel.SetActive(false);
        //currExercise = new Exercise();
        //selectedExercise = new Dictionary<string, int>();
        //selectedJoints = new List<string>();
	}
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(UnityEngine.Random.Range(1,3));
	}
    public void register()
    {
        StreamReader fread = new StreamReader("user.txt");
        string uname = GameObject.Find("NameField").GetComponent<InputField>().text;
        string pwd = GameObject.Find("PasswordField").GetComponent<InputField>().text;
        //Debug.Log(uname + " " + pwd);
        string temp;
		bool flag = false;
		CreateFolder = "G:\\Unity Projects\\Users\\" + uname;
        while ((temp = fread.ReadLine()) != null && flag == false)
        {
            temp = temp.Substring(0, temp.IndexOf(":"));
            //Debug.Log(temp);
            if(uname==temp)
            {
                flag = true;
                //EditorUtility.DisplayDialog("Error", "User Already Exists.", "Okay");
                break;
            }
        }
        fread.Close();
        if (flag == false)
        {
            StreamWriter file = new StreamWriter("user.txt");
            file.WriteLine(uname + ":" + pwd);
            file.Close();
        }
		if (!Directory.Exists (CreateFolder)) {
			Directory.CreateDirectory(CreateFolder);
		}
    }
    public void login()
    {
        StreamReader file = new StreamReader("user.txt");
        string uname = GameObject.Find("NameField").GetComponent<InputField>().text;
        string pwd = GameObject.Find("PasswordField").GetComponent<InputField>().text;
        string match = uname + ":" + pwd;
        string temp;
		bool flag = false;
		CreateFolder = "G:\\Unity Projects\\Users\\" + uname;
        while((temp=file.ReadLine())!=null && flag==false)
        {
            if (temp == match)
            {
                user = uname;
                file.Close();
                loginSuccess();
                flag = true;
                break;
            }
        }
        if (flag == false)
        {
            //EditorUtility.DisplayDialog("Error", "Invalid Username or Password.", "Okay");
            file.Close();
        }
		if (!Directory.Exists (CreateFolder)) {
			Directory.CreateDirectory(CreateFolder);
		}
    }
    public void loginSuccess()
    {
        loginUI.SetActive(false);
        inter.SetActive(true);
        skel.SetActive(true);
    }


    public void OKdemo(InputField ip)
    {
		if (ip.text != "")
		{
			
			if (ip.name == "HighKneesField") 
			{
				currExercise = "HighKness";
				Debug.Log (currExercise);
				MapJoins.selectedExercise.Add(currExercise, 0);
				MapJoins.ExerciseList.Add(currExercise);

				int rep = System.Convert.ToInt32(ip.text);
				MapJoins.selectedExercise[currExercise] = rep;

				foreach (KeyValuePair<string, int>  k in MapJoins.selectedExercise)
				{
					// Debug.Log(k.Key+":"+k.Value);
				}
				currExercise = null;

			}
			else if(ip.name == "SquatsField")
			{
				Debug.Log (ip.name);
				currExercise = "Squats";
				MapJoins.selectedExercise.Add(currExercise, 0);
				MapJoins.ExerciseList.Add(currExercise);

				int rep = System.Convert.ToInt32(ip.text);
				MapJoins.selectedExercise[currExercise] = rep;

				foreach (KeyValuePair<string, int>  k in MapJoins.selectedExercise)
				{
					// Debug.Log(k.Key+":"+k.Value);
				}
				currExercise = null;

			}else if(ip.name == "JJField")
			{
				Debug.Log (ip.name);
				currExercise = "JumpingJack";
				MapJoins.selectedExercise.Add(currExercise, 0);
				MapJoins.ExerciseList.Add(currExercise);

				int rep = System.Convert.ToInt32(ip.text);
				MapJoins.selectedExercise[currExercise] = rep;

				foreach (KeyValuePair<string, int>  k in MapJoins.selectedExercise)
				{
					// Debug.Log(k.Key+":"+k.Value);
				}
				currExercise = null;
			}else if(ip.name == "LeftLungesField")
			{
				Debug.Log (ip.name);
				currExercise = "LeftLunges";
				MapJoins.selectedExercise.Add(currExercise, 0);
				MapJoins.ExerciseList.Add(currExercise);

				int rep = System.Convert.ToInt32(ip.text);
				MapJoins.selectedExercise[currExercise] = rep;

				foreach (KeyValuePair<string, int>  k in MapJoins.selectedExercise)
				{
					// Debug.Log(k.Key+":"+k.Value);
				}
				currExercise = null;
			}else if(ip.name == "RightLungesField")
			{
				Debug.Log (ip.name);
				currExercise = "RightLunges";
				MapJoins.selectedExercise.Add(currExercise, 0);
				MapJoins.ExerciseList.Add(currExercise);

				int rep = System.Convert.ToInt32(ip.text);
				MapJoins.selectedExercise[currExercise] = rep;

				foreach (KeyValuePair<string, int>  k in MapJoins.selectedExercise)
				{
					// Debug.Log(k.Key+":"+k.Value);
				}
				currExercise = null;
			}
		}
		else
		{
			//EditorUtility.DisplayDialog("Error", "Enter the number of Reps.", "Okay");
		}
    }

    public void ExercisePressed(Button b)
    {
        if (b.image.color == Color.green)
        {
            //EditorUtility.DisplayDialog("Error", "You have already selected this exercise.", "Okay");
            currExercise = b.name;
        }
        else
        {
            b.image.color = Color.green;
			Debug.Log (b.name);
            currExercise = b.name;
            MapJoins.selectedExercise.Add(currExercise, 0);
            //selectedExercise.Add(currExercise, 0);
            MapJoins.ExerciseList.Add(currExercise);
        }
    }

    public void addJoint(Button b)
    {
        string jointName = b.name;
        if (MapJoins.selectedJoints.Contains(jointName))
        //if (selectedJoints.Contains(jointName))
        {
            MapJoins.selectedJoints.Remove(jointName);
            //selectedJoints.Remove(jointName);
            b.image.color = Color.white;
        }
        else
        {
            MapJoins.selectedJoints.Add(jointName);
            //selectedJoints.Add(jointName);
            b.image.color = Color.green;
        }
        foreach (string s in MapJoins.selectedJoints)
        //foreach (string s in selectedJoints)
        {
            //Debug.Log(s);
        }
    }

    public void finish()
    {
		Debug.Log (MapJoins.selectedExercise);
        InputField br = GameObject.Find("BreakField").GetComponent<InputField>();
        InputField set = GameObject.Find("SetsField").GetComponent<InputField>();
        if (MapJoins.selectedJoints.Count == 0)
        {
            //EditorUtility.DisplayDialog("Error", "You have not selected any joints.", "Okay");
        }
        else if (MapJoins.selectedExercise.Count == 0) {
            //EditorUtility.DisplayDialog("Error", "You have not selected any exercise.", "Okay");
        }
        else if (br.text == "") {
            //EditorUtility.DisplayDialog("Error", "You have not entered break time.", "Okay");
        }
        else if (set.text == "") {
            //EditorUtility.DisplayDialog("Error", "You have not entered number of sets.", "Okay");
        }
        else
        {
            MapJoins.breakTime = Convert.ToInt32(br.text);
            MapJoins.sets = Convert.ToInt32(set.text);
            inter.SetActive(false);
            skel.SetActive(false);
            //EditorUtility.DisplayDialog("Error", "Details Updated.", "Okay");
            //NetworkManager.StartS();
            CharacterMotion.CanWork = true;
            MJ.Initialized = true;
            MJ.Map();
            GUI.SetActive(false);
        }
    }
}
