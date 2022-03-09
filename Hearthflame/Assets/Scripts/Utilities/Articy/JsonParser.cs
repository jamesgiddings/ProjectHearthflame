using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class JsonParser : MonoBehaviour
{
	[SerializeField] string jsonString;
	[SerializeField] public TextAsset jsonFile;

	private void Start()
	{
		//ArticyDatabase articyDatabase = JsonUtility.FromJson<ArticyDatabase>(jsonFile.text);
		//Debug.Log(articyDatabase.Settings.set_Localization);
		//Debug.Log(articyDatabase.GlobalVariables[0].Namespace);
		//Debug.Log(articyDatabase.GlobalVariables[0].Variables[0].Description);
		//Debug.Log(articyDatabase.ObjectDefinitions[0].Class);
		//Debug.Log(articyDatabase.ObjectDefinitions[0].Properties[0].Property);
		//Debug.Log(articyDatabase.ObjectDefinitions[0].Properties[0].ItemType); // Should be null and is
		//Debug.Log(articyDatabase.ObjectDefinitions[11].DisplayName); // Should be null and is
		//															 //Debug.Log(articyDatabase.ObjectDefinitions[11].Values["Male"]); // Should be null and is

		JSONNode root = JSON.Parse(jsonFile.text);

		//Debug.Log(root["GlobalVariables"].ToString());

		Articy articy = new Articy(root);
		Debug.Log(articy.Namespaces["FungusChapter1"].ArticyVariables[1]);

		// Variables



		//foreach (var item in collection)
		//{

		//}
		//foreach (JSONNode node in root)
		//{

		//	foreach (JSONNode subNode in node)
		//	{
		//		Debug.Log(subNode.ToString());
		//	}
	}


	}
	
		//.FromJson<ArticyGlobalVariable>(json);


public class FirstNameObject
{
	public string firstName;
}
