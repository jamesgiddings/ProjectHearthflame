using GramophoneUtils.Items.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Resource Database", menuName = "Resources/Resource Database")]
public class ResourceDatabase : ScriptableObject
{
	[SerializeField] private Resource[] resources;

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
	}

	private Dictionary<string, Resource> database = new Dictionary<string, Resource>();

	public void AddResource(Resource resource)
	{
		if (!database.ContainsKey(resource.UID))
		{
			Debug.Log("WE ARE ADDING A RESOURCE");
			database.Add(resource.UID, resource);
		}
	}

	public void RemoveResource(Resource resource)
	{
		if (!database.ContainsKey(resource.UID))
		{
			database.Remove(resource.UID);
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
		Debug.Log("Resources Length: " + resources.Length);
		foreach (KeyValuePair<string, Resource> entry in database)
		{
			Debug.Log(entry.Value.Name + ": " + entry.Key);
		}
	}
}
