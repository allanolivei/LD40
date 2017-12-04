using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Vitality")]
public class VitalityData : ScriptableObject
{

	public float initialLife =  100.0f;

	[SerializeField]
	private float life = 100.0f;

	public void Reset()
	{
		this.life = initialLife;
	}

	public void SetLife( float value )
	{
		this.life = value;
	}

	public float GetLife()
	{
		return life;
	}

	public bool TakeDamage( float value )
	{
		//life -= value;
		return life < 0;
	}

	private void Awake()
	{
		this.Reset ();
	}

	private void OnEnable()
	{
		this.Reset ();
	}


}
