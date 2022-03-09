using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;

public class Articy
{
	public Dictionary<string, ArticyNamespace> Namespaces;

	public Articy(JSONNode root)
	{
		CreateNamespaces(root["GlobalVariables"]);
	}

	public void CreateNamespaces(JSONNode namespaces)
	{
		Namespaces = new Dictionary<string, ArticyNamespace>();
		foreach (JSONNode node in namespaces)
		{
			ArticyNamespace articyNamespace = new ArticyNamespace(node);
			Namespaces.Add(articyNamespace.Name, articyNamespace);
		}
	}
}

public enum VariableType { Integer, Boolean, String }

public class ArticyNamespace
{
	public string Name;
	public string Description;
	public ArticyVariable[] ArticyVariables;

	public ArticyNamespace(JSONNode globalVariableNode)
	{
		this.Name = globalVariableNode["Namespace"];
		this.Description = globalVariableNode["Description"];
		this.ArticyVariables = new ArticyVariable[globalVariableNode["Variables"].Count];
		for (int i = 0; i < globalVariableNode["Variables"].Count; i++)
		{
			JSONNode variableNode = globalVariableNode["Variables"][i];
			string variable = variableNode["Type"];

			switch (variable)
			{
				case "Integer":
					ArticyVariables[i] = new ArticyIntegerVariable(variableNode);
					break;
				case "Boolean":
					ArticyVariables[i] = new ArticyBooleanVariable(variableNode);
					break;
				case "String":
					ArticyVariables[i] = new ArticyStringVariable(variableNode);
					break;
				default:
					Debug.LogError("Unsupported variable type from Articy import.");
					break;
			}
		}
	}
}

public abstract class ArticyVariable
{
	public string Name;
	public VariableType VariableType;
	public string Description;
	public string StringValue;
}

public class ArticyIntegerVariable : ArticyVariable
{
	public int Value;
	public ArticyIntegerVariable(JSONNode variableNode)
	{
		this.Name = variableNode["Variable"];
		this.Description = variableNode["Description"];
		this.StringValue = variableNode["Value"];
		this.Value = Int32.Parse(variableNode["Value"]);
	}
}

public class ArticyBooleanVariable : ArticyVariable
{
	public bool Value;
	public ArticyBooleanVariable(JSONNode variableNode)
	{
		this.Name = variableNode["Variable"];
		this.Description = variableNode["Description"];
		this.StringValue = variableNode["Value"];
		string tempValue = variableNode["Value"];
		switch (tempValue)
		{
			case "True":
				this.Value = true;
				break;
			case "False":
				this.Value = false;
				break;
			default:
				Debug.LogError("String variable of type Bool could not be converted to boolean.");
				break;
		}
	}
	
}

public class ArticyStringVariable : ArticyVariable
{
	public string Value;
	public ArticyStringVariable(JSONNode variableNode)
	{
		this.Name = variableNode["Variable"];
		this.Description = variableNode["Description"];
		this.StringValue = variableNode["Value"];
		this.Value = variableNode["Value"];
	}
}

