using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour 
{

	public float speed = 100.0f;
	public float lifeDuration = 4.0f;

	public Ammunition ammunition;

	private Rigidbody2D body;
	private Transform trans;



	public void SetData( Vector3 position, Vector3 direction )
	{
		if( body == null ) 
			this.RecoveryCache ();
		
		trans.position = (Vector2)position;
		trans.rotation = Quaternion.AngleAxis(
			Mathf.Atan2 (direction.x , -direction.y) * Mathf.Rad2Deg - 90,
			Vector3.forward
		);
	}

	private void Awake()
	{
		this.RecoveryCache ();
	}

	private void RecoveryCache()
	{
		body = GetComponent<Rigidbody2D> ();
		trans = GetComponent<Transform> ();
	}

	private void OnEnable()
	{
		StartCoroutine ("WaitingLifeTime");
	}

	private void Update()
	{
		float rad = body.rotation * Mathf.Deg2Rad;
		body.velocity = new Vector2 (Mathf.Cos (rad), Mathf.Sin (rad)) * speed * Time.deltaTime;
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
