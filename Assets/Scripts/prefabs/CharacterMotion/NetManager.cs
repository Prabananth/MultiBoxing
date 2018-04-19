using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetManager : NetworkManager {

	// Use this for initialization
	void Start () {
        dontDestroyOnLoad = false;
        autoCreatePlayer = false;
        networkAddress = "192.168.0.107";
        networkPort = 7777;
        StartS();

    }

    public void StartS()
    {
        StartServer();
    }
	
}
