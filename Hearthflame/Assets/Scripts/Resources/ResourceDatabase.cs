using GramophoneUtils.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GramophoneUtils.Stats;
using System;
using UnityEditor;

[Serializable]
[CreateAssetMenu(fileName = "Resource Database", menuName = "Resources/Resource Database")]
public class ResourceDatabase : ScriptableObject
{
	[SerializeField] private List<Resource> resources;
	[SerializeField] private CharacterClass[] characterClasses;
	[SerializeField] private StatType[] stats;

	private Dictionary<string, Resource> database = new Dictionary<string, Resource>();

	public List<Resource> Resources => resources; 

	public CharacterClass[] CharacterClasses => characterClasses; // getter
	public StatType[] Stats => stats; // getter

	private void Awake()
	{
		foreach (Resource resource in resources)
		{
			AddResource(resource);
		}
	}

	protected virtual void OnValidate()
	{
		foreach (Resource resource in resources)
		{
			AddResource(resource);
		}
		RepopulateResourcesListFromDatabase();
	}

	public void RepopulateResourcesListFromDatabase()
    {
		resources = new List<Resource>(database.Values.ToList());
	}

	public void AddResource(Resource resource)
	{
		if (resource == null)
		{
			Debug.LogWarning(resource.name + " is null. Not added to resource database.");
			return;
		}
		if (resource.UID == "")
		{
			Debug.LogWarning(resource.name + "'s resource.UID == null. Not added to resource database.");
			return;
		}
		
		if (!database.ContainsKey(resource.UID))
		{
			database.Add(resource.UID, resource);
			return;
		}
        else
        {
			Debug.LogWarning("Database already contains " + resource.name + "'s resource.UID: " + resource.UID);
			return;
        }
	}

	public void RemoveResource(Resource resource)
	{
		if (database.ContainsKey(resource.UID))
		{
			database.Remove(resource.UID);
			RepopulateResourcesListFromDatabase();
			Debug.Log("Removed" + resource.UID);
		}
	}


	public Resource GetResourceByUID(string UID)
	{
		if (database.ContainsKey(UID))
		{
			return database[UID];
		}
		else
			return null;
	}

	public bool Contains(string UID)
	{
		if (database.ContainsKey(UID))
		{
			return true;
		}
		return false;
	}

	public bool Contains(Resource resource)
	{
		if (database.ContainsValue(resource))
		{
			return true;
		}
		return false;
	}

	public void Print()
	{
		Debug.Log("Resource Database print called.");
		Debug.Log("Database Count: " + database.Values.Count);
		Debug.Log("Resources Count: " + resources.Count);
		foreach (KeyValuePair<string, Resource> entry in database)
		{
			Debug.Log(entry.Value.Name + ": " + entry.Key);
		}
	}
}