using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollowMe : MonoBehaviour 
{

	public Transform followTarget;
	public Vector3 offset;

	private Transform trans;
	private ParticleSystem ps;

	private void Awake()
	{
		trans = GetComponent<Transform> ();
		ps = GetComponent<ParticleSystem>();
	}

	private void Update () 
	{
		this.trans.position = followTarget.position + offset;
		if( !followTarget.gameObject.activeInHierarchy ) ps.Stop();
	}
}
