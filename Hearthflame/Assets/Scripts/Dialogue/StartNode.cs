using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : BaseNode
{
	[Output] public int Output;

	public override string GetString()
	{
		return "Start";
	}
}
