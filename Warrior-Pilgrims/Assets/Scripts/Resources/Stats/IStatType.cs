using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
	public interface IStatType
	{
		string Name { get; }

		float DefaultValue { get; }

		float TuningMultiplier { get; } 
	}
}

