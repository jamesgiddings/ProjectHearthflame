using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XNode;
namespace Dialogue
{
	[NodeTint("#FFFFAA")]
	public class End : Chat
	{
		//public SerializableEvent[] trigger; // Could use UnityEvent here, but UnityEvent has a bug that prevents it from serializing correctly on custom EditorWindows. So i implemented my own.

		public override void Trigger()
		{
			(graph as DialogueGraph).current = this;
		}
	}
}