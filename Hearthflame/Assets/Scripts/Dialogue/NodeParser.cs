using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XNode;
using Dialogue;

public class NodeParser : MonoBehaviour
{
	public DialogueGraph graph;
	private Coroutine parser;

	[SerializeField] private Transform optionParent;
	[SerializeField] private GameObject optionPrefab;

	public TextMeshProUGUI SpeakerNameTextField;
	public TextMeshProUGUI DialogueTextField;
	public Image SpeakerImage;

	private void Start()
	{
		graph.Restart();
		ParseCurrentNode();
	}

	private void ParseCurrentNode()
	{
		for (int i = 0; i < optionParent.transform.childCount; i++)
		{
			GameObject child = optionParent.GetChild(i).gameObject;
			Button optionButton = child.GetComponent<Button>();
			if (optionButton != null)
			{
				optionButton.onClick.RemoveAllListeners();
			}
			Destroy(child);
		}

		DialogueBaseNode baseNode = graph.current;

		if (baseNode is Dialogue.Chat)
		{
			Dialogue.Chat chat = (Chat)baseNode;
			SpeakerNameTextField.text = chat.character.Name;
			SpeakerImage.sprite = chat.character.Icon;
			DialogueTextField.text = chat.text;

			NodePort port = chat.GetOutputPort("output");

			if (chat.answers.Count == 0 && (port.GetConnections().Count > 0))
			{
				Button optionButton = UnityEngine.Object.Instantiate(optionPrefab, optionParent).GetComponent<Button>();
				TextMeshProUGUI optionButtonText = optionButton.GetComponentInChildren<TextMeshProUGUI>();
				optionButtonText.text = "...";
				optionButton.onClick.AddListener(() => graph.AnswerQuestion(optionButton.transform.GetSiblingIndex()));
				optionButton.onClick.AddListener(ParseCurrentNode);
			}


			for (int i = 0; i < chat.answers.Count; i++)
			{
				Button optionButton = UnityEngine.Object.Instantiate(optionPrefab, optionParent).GetComponent<Button>();
				TextMeshProUGUI optionButtonText = optionButton.GetComponentInChildren<TextMeshProUGUI>();
				optionButtonText.text = chat.answers[i].text;
				optionButton.onClick.AddListener(() => graph.AnswerQuestion(optionButton.transform.GetSiblingIndex()));
				optionButton.onClick.AddListener(ParseCurrentNode);
			}
		}
		else if (baseNode is Dialogue.Branch)
		{

		}
		else if (baseNode is Dialogue.Event)
		{

		}
		else
		{
			Debug.LogError("Unexpected type found in DialogueGraph.");
		}
	}

	//private void Start()
	//{
	//	foreach (DialogueBaseNode baseNode in graph.nodes)
	//	{
	//		if (baseNode.GetString() == "Start")
	//		{
	//			graph.Current = baseNode;
	//			break;
	//		}
	//	}
	//	parser = StartCoroutine(ParseNode());
	//}

	//private IEnumerator ParseNode()
	//{
	//	DialogueBaseNode baseNode = graph.Current;
	//	string data = baseNode.GetString();
	//	string[] dataParts = data.Split('/');
	//	if (dataParts[0] == "Start")
	//	{
	//		NextNode("Output");
	//	}
	//	if (dataParts[0] == "DialogueNode")
	//	{
	//		Speaker.text = dataParts[1];
	//		Dialogue.text = dataParts[2];
	//		SpeakerImage.sprite = baseNode.GetSprite();

	//		yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
	//		yield return new WaitUntil(() => Input.GetMouseButtonUp(0));



	//		NextNode("Output");
	//	}

	//	for (int i = 0; i < 3; i++)
	//	{
	//		UnityEngine.Object.Instantiate(optionPrefab, optionParent);
	//	}



	//}

	//public void NextNode(string fieldName)
	//{
	//	// Find the port with this name
	//	if (parser != null)
	//	{
	//		StopCoroutine(parser);
	//		parser = null;
	//	}
	//	foreach (NodePort nodePort in graph.Current.Ports)
	//	{
	//		if (nodePort.fieldName == fieldName)
	//		{
	//			if (nodePort.Connection != null)
	//			{
	//				graph.Current = nodePort.Connection.node as BaseNode;
	//			}
	//			break;
	//		}
	//	}
	//	parser = StartCoroutine(ParseNode());
	//}
}
