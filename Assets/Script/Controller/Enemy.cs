using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour 
{
	public float moveSpeed = 100.0f;

	private Rigidbody2D body;

	private void Awake()
	{
		this.RecoveryCache ();
	}

	private void FixedUpdate()
	{
		
	}

	private void RecoveryCache()
	{
		body = GetComponent<Rigidbody2D> ();
	}

}
