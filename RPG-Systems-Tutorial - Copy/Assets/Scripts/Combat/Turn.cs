using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Turn
{
	public static Action OnTurnElapsed;

	public static void AdvanceTurn()
	{
		OnTurnElapsed?.Invoke();
	}
}
