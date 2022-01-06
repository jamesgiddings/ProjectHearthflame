using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GramophoneUtils.Stats
{
	public interface IBlueprint
	{
		public T CreateBlueprintInstance<T>(object source = null);
	}
}

