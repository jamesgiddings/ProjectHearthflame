using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XNode;
using Dialogue;
using GramophoneUtils.SavingLoading;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class DialogueUI : MonoBehaviour, ISaveable
{
	[SerializeField] private DialogueGraph dialogueGraph;

	[SerializeField] private Transform optionParent;
	[SerializeField] private GameObject optionPrefab;

	[SerializeField] private GameObject dialoguePanel;

	public TextMeshProUGUI SpeakerNameTextField;
	public TextMeshProUGUI DialogueTextField;
	public Image SpeakerImage;
	
	public void StartDialogue()
	{
		dialoguePanel.SetActive(true);
		dialogueGraph.Restart();
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

		DialogueBaseNode baseNode = dialogueGraph.current;
		if (baseNode is Dialogue.End)
		{
			dialoguePanel.SetActive(false);
		}
		else if (baseNode is Dialogue.Chat)
		{
			Dialogue.Chat chat = (Chat)baseNode;
			SpeakerNameTextField.text = chat.character.Name;
			SpeakerImage.sprite = chat.character.Portrait;
			DialogueTextField.text = chat.text;

			NodePort port = chat.GetOutputPort("output");

			if (chat.answers.Count == 0 && (port.GetConnections().Count > 0))
			{
				Button optionButton = UnityEngine.Object.Instantiate(optionPrefab, optionParent).GetComponent<Button>();
				TextMeshProUGUI optionButtonText = optionButton.GetComponentInChildren<TextMeshProUGUI>();
				optionButtonText.text = "...";
				optionButton.onClick.AddListener(() => dialogueGraph.AnswerQuestion(optionButton.transform.GetSiblingIndex()));
				optionButton.onClick.AddListener(ParseCurrentNode);
			}


			for (int i = 0; i < chat.answers.Count; i++)
			{
				Button optionButton = UnityEngine.Object.Instantiate(optionPrefab, optionParent).GetComponent<Button>();
				TextMeshProUGUI optionButtonText = optionButton.GetComponentInChildren<TextMeshProUGUI>();
				optionButtonText.text = chat.answers[i].text;
				optionButton.onClick.AddListener(() => dialogueGraph.AnswerQuestion(optionButton.transform.GetSiblingIndex()));
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

    public object CaptureState()
    {
        List<DialogueInt.DialogueIntSaveData> ints = new List<DialogueInt.DialogueIntSaveData>();
        List<DialogueBool.DialogueBoolSaveData> bools = new List<DialogueBool.DialogueBoolSaveData>();



        foreach (Node node in dialogueGraph.nodes)
        {
            if (node is Branch)
            {

            }
            Debug.LogError("This is not fully implemented. Need to do condition where node is a branch (or do I? changes to them should be saved elsewhere?, and need to do Restore State");
            if (node is Dialogue.Event)
            {
                Dialogue.Event dialogueEvent = node as Dialogue.Event;
                foreach (UnityEvent unityEvent in dialogueEvent.trigger)
                {
                    for (int i = 0; i < unityEvent.GetPersistentEventCount(); i++)
                    {
                        UnityEngine.Object listener = unityEvent.GetPersistentTarget(i);
                        if (listener as DialogueBool)
                        {
                            DialogueBool dialogueBool = listener as DialogueBool;
                            bools.Add(new DialogueBool.DialogueBoolSaveData(dialogueBool.Value));
                        }
                        if (listener as DialogueInt)
                        {
                            DialogueInt dialogueInt = listener as DialogueInt;
                            ints.Add(new DialogueInt.DialogueIntSaveData(dialogueInt.Value));
                        }
                    }
                }
                
            }
        }

        return new DialogueSaveData(ints, bools);
    }

    public void RestoreState(object state)
    {
        throw new System.NotImplementedException();
    }

    [Serializable]
    public struct DialogueSaveData
    {
        public List<DialogueInt.DialogueIntSaveData> Ints;
        List<DialogueBool.DialogueBoolSaveData> Bools;

        public DialogueSaveData(List<DialogueInt.DialogueIntSaveData> ints, List<DialogueBool.DialogueBoolSaveData> bools)
        {
            this.Ints = ints;
            this.Bools = bools;
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
