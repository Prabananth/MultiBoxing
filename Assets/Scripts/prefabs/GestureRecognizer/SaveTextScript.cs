using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public  static class SaveTextScript 
{
	public static List<GameData> savedGames = new List<GameData>();
	public static List<GUIScript> username = new List<GUIScript> ();
	private static object SaveScript;

	public static string now = DateTime.Now.ToString("dd.MM.yyyy");
	public static string file;
	//it's static so we can call it from anywhere
	public static void Save()
	{ 
		GameData.Current.date = DateTime.Now;
		SaveTextScript.savedGames.Add(GameData.Current);
		file = GUIScript.path.CreateFolder + "//" + now + ".txt";
		using (StreamWriter wr = File.AppendText (file)) 
		{
			wr.WriteLine (SaveLoad.savedGames);
		}
	}
}