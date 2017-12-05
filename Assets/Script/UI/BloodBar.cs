using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodBar : MonoBehaviour 
{

	public VitalityData data;
	public Transform bar;

	private void Update()
	{
		bar.localScale = new Vector3 (1.0f * Mathf.Max(0, (data.GetLife()/data.initialLife)),1,1);
	}

}
