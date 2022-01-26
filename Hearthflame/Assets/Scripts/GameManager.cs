using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
	{
		get
		{
			if(instance == null)
			{
				Debug.LogError("Game Manager is null.");
			}
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
	}
}
