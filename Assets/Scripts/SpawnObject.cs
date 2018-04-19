using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {
    float time = 0;
	// Use this for initialization
	void Start(){
		
	}
	
	// Update is called once per frame
	void Update(){
        time += Time.deltaTime;
        if(time > 2.5f)
        {
            CreateShots();
            time = 0f;
        }
    }

    public void CreateShots()
    {
        GameObject sphereObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphereObj.transform.position = transform.position;
        sphereObj.transform.localScale = new Vector3(0.05f,0.05f,0.05f);
        Rigidbody so = sphereObj.AddComponent<Rigidbody>();
        so.mass = 10000;
        so.AddForce(transform.forward * -3000000);
        
    }
}
