using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArticyDatabase
{
	public Settings Settings;
    public Project Project;
    public GlobalVariable[] GlobalVariables;
    public ObjectDefinition[] ObjectDefinitions;
}

[System.Serializable]
public class Settings
{
    public bool set_Localization;
    public string set_TextFormatter;
    public string[] set_IncludedNodes;
    public bool set_UseScriptSupport;
    public float ExportVersion;
}

[System.Serializable]
public class Project
{
    public string Name;
    public string DetailName;
    public string Guid;
    public string TechnicalName;
}

[System.Serializable]
public class GlobalVariable
{
    public string Namespace;
    public string Description;
    public _Variable[] Variables;
}

[System.Serializable]
public class _Variable
{
    public string Variable;
    public string Type;
    public string Value;
    public string Description;
}

[System.Serializable]
public class ObjectDefinition
{
    public string Type;
    public string Class;
    public string DisplayName;
    public Values Values;
    public DisplayNames DisplayNames;
    public Properties[] Properties;
}

[System.Serializable]
public class Properties
{
    public string Property;
    public string Type;
    public string ItemType;
}

[System.Serializable]
public class Values
{
	public string Male;
	public string Female;
	public string Unknown;
}

[System.Serializable]
public class DisplayNames
{
    public string Male;
    public string Female;
    public string Unkown;
}


[System.Serializable]
public class Packages
{

}

[System.Serializable]
public class ScriptMethods
{

}

[System.Serializable]
public class Hierarchy
{

}
