using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour 
{

	public float speed = 100.0f;
	public float lifeDuration = 4.0f;

	[System.NonSerialized]
	public Ammunition ammunition;

	private Rigidbody body;
	private Transform trans;



	public void SetData( Vector3 position, Vector3 direction )
	{
		if( body == null ) 
			this.RecoveryCache ();
		
		trans.position = position;
		trans.rotation = Quaternion.LookRotation (direction, Vector3.up);
	}

	private void Awake()
	{
		this.RecoveryCache ();
	}

	private void RecoveryCache()
	{
		body = GetComponent<Rigidbody> ();
		trans = GetComponent<Transform> ();
	}

	private void OnEnable()
	{
		//StartCoroutine ("WaitingLifeTime");
	}

	private void FixedUpdate()
	{
		body.velocity =  trans.forward * speed * Time.deltaTime;
	}

	private void OnCollisionEnter2D( Collision2D other )
	{
		if (ammunition != null) ammunition.Recycle (this);
	}

	private IEnumerator WaitingLifeTime()
	{
		yield return new WaitForSeconds (lifeDuration);

		if (ammunition != null) ammunition.Recycle (this);
	}

}
