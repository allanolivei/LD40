using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour 
{

	public static DontDestroy current;

	private void Awake()
	{
		if (current != null) 
		{
			Destroy (gameObject);
			return;
		}

		DontDestroyOnLoad (gameObject);	
		current = this;
	}

}
