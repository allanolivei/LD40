using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Vitality")]
public class Vitality : ScriptableObject
{

	[SerializeField]
	private float life = 100.0f;

	public float GetLife()
	{
		return life;
	}

	public bool TakeDamage( float value )
	{
		life -= value;
		return life < 0;
	}


}
