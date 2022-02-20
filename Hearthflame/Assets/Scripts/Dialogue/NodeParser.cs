using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XNode;

public class NodeParser : MonoBehaviour
{
	public DialogueGraph graph;
	private Coroutine parser;

	public TextMeshProUGUI Speaker;
	public TextMeshProUGUI Dialogue;
	public Image SpeakerImage;

	private void Start()
	{
		foreach (BaseNode baseNode in graph.nodes)
		{
			if (baseNode.GetString() == "Start")
			{
				graph.Current = baseNode;
				break;
			}
		}
		parser = StartCoroutine(ParseNode());
	}

	private IEnumerator ParseNode()
	{
		BaseNode baseNode = graph.Current;
		string data = baseNode.GetString();
		string[] dataParts = data.Split('/');
		if(dataParts[0] == "Start")
		{
			NextNode("Output");
		}
		if (dataParts[0] == "DialogueNode")
		{
			Speaker.text = dataParts[1];
			Dialogue.text = dataParts[2];
			SpeakerImage.sprite = baseNode.GetSprite();

			yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
			yield return new WaitUntil(() => Input.GetMouseButtonUp(0));

			NextNode("Output");
		}
	}

	public void NextNode(string fieldName)
	{
		// Find the port with this name
		if (parser != null)
		{
			StopCoroutine(parser);
			parser = null;
		}
		foreach (NodePort nodePort in graph.Current.Ports)
		{
			if (nodePort.fieldName == fieldName)
			{
				if (nodePort.Connection != null)
				{
					graph.Current = nodePort.Connection.node as BaseNode;
				}
				break;
			}
		}
		parser = StartCoroutine(ParseNode());
	}
}
