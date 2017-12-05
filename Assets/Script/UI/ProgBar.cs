using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgBar : MonoBehaviour 
{

	public Transform bar;

	public void SetValue( float value )
	{
		bar.localScale = new Vector3 (Mathf.Clamp(value, 0.0f, 1.0f), 1, 1);
	}

}
