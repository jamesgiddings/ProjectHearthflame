using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TabUI : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI tabLabel;

	public void SetTabText(string tabText)
	{
		tabLabel.text = tabText;
	}
}
