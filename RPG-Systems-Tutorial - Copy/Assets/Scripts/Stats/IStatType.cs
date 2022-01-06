using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
	public interface IStatType
	{
		string Name { get; } //getter

		float DefaultValue { get; } //getter
	}
}

