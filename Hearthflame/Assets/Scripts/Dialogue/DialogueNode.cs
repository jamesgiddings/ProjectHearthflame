using GramophoneUtils.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogueNode : BaseNode {

	[Input] public int Entry;
	[Output] public int Output;

	public CharacterTemplate characterTemplate;

	public string speakerName;
	public string dialogueLine;
	public Sprite sprite;

	
	
	// Use this for initialization
	protected override void Init() {
		base.Init();
		sprite = characterTemplate.Icon;
		speakerName = characterTemplate.Name;
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}

	public override string GetString()
	{
		return "DialogueNode/" + speakerName + "/" + dialogueLine;
	}

	public override Sprite GetSprite()
	{
		return sprite;
	}

}