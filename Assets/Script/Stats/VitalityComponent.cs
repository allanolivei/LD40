using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VitalityComponent : MonoBehaviour 
{
	public SpriteRenderer splash;
	public VitalityData data;
	[Header("Use when data is null")]
	public float initialLife = 100;

	public UnityEvent onDeath;

	public bool IsDepth()
	{
		return data.GetLife() <= 0;
	}

	public bool TakeDamage( float value, Vector3 position )
	{
		bool result = data.TakeDamage (value);
		if( result ) onDeath.Invoke ();

		if( splash != null )
			Instantiate (splash, position, splash.transform.rotation);

		return result;
	}
		
	private void Awake()
	{
		if (this.data == null) 
		{
			this.data = ScriptableObject.CreateInstance<VitalityData> ();
			this.data.initialLife = initialLife;
		}
	}

	private void OnEnable()
	{
		this.data.Reset ();
	}

}
