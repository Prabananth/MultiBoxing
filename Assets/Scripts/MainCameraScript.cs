using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log(Input.gyro.attitude);
        transform.rotation = Quaternion.Inverse(Input.gyro.attitude);
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(3,3,3));
	}
}
