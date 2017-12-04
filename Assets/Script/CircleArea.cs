using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleArea : MonoBehaviour 
{

	public float radius;

	private Transform trans;

	public Vector3 position { get { return trans.position; } }

	private void Awake()
	{
		this.trans = this.GetComponent<Transform> ();
	}

	#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (transform.position, radius);
	}
	#endif

}
