using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollowMe : MonoBehaviour 
{

	public Transform followTarget;
	public Vector3 offset;

	private Transform trans;

	private void Awake()
	{
		trans = GetComponent<Transform> ();
	}

	private void OnEnable()
	{
		
	}

	private void OnDisable()
	{
		
	}

	private void Update () 
	{
		this.trans.position = followTarget.position + offset;
	}
}
