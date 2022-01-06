using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceDatabase
{
	private static Dictionary<string, Resource> database = new Dictionary<string, Resource>();
	public static void AddResource(Resource resource)
	{
		if (!database.ContainsKey(resource.UID))
		{
			database.Add(resource.UID, resource);
		}
	}

	public static Resource GetResourceByUID(string UID)
	{
		if (database.ContainsKey(UID))
		{
			return database[UID];
		}
		else
			return null;
	}
}
