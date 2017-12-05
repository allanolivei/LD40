using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour 
{
	private static Collider[] nonAlloc = new Collider[10];

	public float speed = 100.0f;
	public float damage = 10.0f;
	public float lifeDuration = 4.0f;
	public float damageRadius = 0.0f;
	public LayerMask damageMask = -1;
	public int resistance = 1;
	public ParticleSystem explosionEffectPrefab;
	public ParticleSystem trailParticleEffectPrefab;
	public AudioClip impactClip;
	public float impactClipVolume = 1.0f;

	[System.NonSerialized]
	public Ammunition ammunition;

	private Rigidbody body;
	private Transform trans;
	private TrailRenderer trail;
	private int collisionAmount = 0;



	public void SetData( Vector3 position, Vector3 direction )
	{
		if( body == null ) 
			this.RecoveryCache ();
		
		if(trail) trail.Clear ();

		trans.position = position;
		trans.rotation = Quaternion.LookRotation (direction, Vector3.up);
	}

	private void Awake()
	{
		this.RecoveryCache ();

		if (trailParticleEffectPrefab != null)
			ParticleManager.GetInstance ().Register (trailParticleEffectPrefab);

		if (explosionEffectPrefab != null)
			ParticleManager.GetInstance ().Register (explosionEffectPrefab);
	}

	private void RecoveryCache()
	{
		body = GetComponent<Rigidbody> ();
		trans = GetComponent<Transform> ();
		trail = GetComponent<TrailRenderer> ();
	}

	private void OnEnable()
	{
		collisionAmount = 0;
		StartCoroutine ("WaitingLifeTime");

		if( trailParticleEffectPrefab != null )
		{
			ParticleSystem ps = ParticleManager.GetInstance().Show(trailParticleEffectPrefab.name, this.trans.position);
			ps.GetComponent<ParticleFollowMe>().followTarget = this.trans;
		}
	}

	private void FixedUpdate()
	{
		if( resistance > 1 )
			trans.position +=  trans.forward * speed * Time.deltaTime * Time.deltaTime;
		else
			body.velocity =  trans.forward * speed * Time.deltaTime;
	}

	private void OnCollisionEnter( Collision other )
	{
		VitalityComponent vitality = other.gameObject.GetComponent<VitalityComponent> ();
		if (vitality != null) 
			vitality.TakeDamage (damage, other.contacts[0].point);

		if (damageRadius > 0.001f) 
		{
			int amount = Physics.OverlapSphereNonAlloc (this.trans.position, damageRadius, nonAlloc, damageMask);
			for (int i = 0; i < amount; i++) 
			{
				VitalityComponent vit = nonAlloc [i].gameObject.GetComponent<VitalityComponent> ();
				if ( vit && vit.tag != "Player" && vit != vitality ) 
					vit.TakeDamage (
						damage * Vector3.Distance (this.trans.position, vit.transform.position) / damageRadius,
						vit.transform.position
					);
			}
		}

		if (impactClip != null)
			AudioSource.PlayClipAtPoint (impactClip, Vector3.zero, impactClipVolume);

		if( explosionEffectPrefab != null )
			ParticleManager.GetInstance ().Show (explosionEffectPrefab.name, this.trans.position);

		collisionAmount++;
		if (ammunition != null && (resistance <= collisionAmount || vitality == null) ) ammunition.Recycle (this);
	}

	private IEnumerator WaitingLifeTime()
	{
		yield return new WaitForSeconds (lifeDuration);

		if (ammunition != null) ammunition.Recycle (this);
	}

}
