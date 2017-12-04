using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(VitalityComponent))]
public class EnemyController : MonoBehaviour 
{
	public float moveSpeed = 100.0f;
	public float cooldown = 1.0f;
	public float damage = 10.0f;
	public float aimDuration = 0.2f;
	public float delayToApplyDamage = 0.1f;
	public CircleArea attackArea;
	public float attackAngleOffset;
	public Weapon enemyWeapon;
	public ParticleSystem attackEffect;
	public bool seePlayer = false;

	private float lastAttackTime;
	private NavMeshAgent agent;
	private Animator anim;
	private VitalityComponent vitality;
	private Collider[] nonAllocCollider;
	private Transform trans;

	public void Spawn( Vector3 position )
	{
		if (agent == null)
			this.RecoveryCache ();

		this.agent.Warp (position);
	}

	public VitalityComponent GetVitality()
	{
		if (vitality == null)
			this.RecoveryCache ();
		return vitality;
	}

	private void Awake()
	{
		this.nonAllocCollider = new Collider[5];

		if (attackEffect)
			ParticleManager.GetInstance ().Register (attackEffect);

		this.RecoveryCache ();
	}

	private void Update()
	{
		Vector3 targetPosition = PlayerController.current.trans.position;

		if ( Vector3.SqrMagnitude(targetPosition - this.trans.position) <= agent.stoppingDistance * agent.stoppingDistance ) 
		{
			anim.SetBool ("Walk", false);

			agent.isStopped = true;
			agent.ResetPath ();

			if (Time.time - lastAttackTime > cooldown) 
			{
				this.Attack ();
			}
			else if ( seePlayer ) 
			{
				Vector3 direction = (PlayerController.current.trans.position - trans.position); direction.y = 0;
				this.trans.rotation = Quaternion.FromToRotation (Vector3.forward, direction.normalized) * Quaternion.Euler (0, attackAngleOffset, 0);
			}
		}
		else 
		{
			anim.SetBool ("Walk", true);

			agent.isStopped = false;
			agent.SetDestination (PlayerController.current.trans.position);
		}
	}


	protected virtual void Attack()
	{
		anim.SetTrigger ("Attack");
		StartCoroutine ("AimToTarget");
		StartCoroutine ("ApplyDamage");
	}

	protected IEnumerator AimToTarget()
	{
		float duration = aimDuration;
		float initTime = Time.time;

		Vector3 direction = (PlayerController.current.trans.position - trans.position); direction.y = 0;
		Quaternion targetRotation = Quaternion.FromToRotation(Vector3.forward, direction.normalized) * Quaternion.Euler(0,attackAngleOffset,0);
		Quaternion initRotation = trans.rotation;

		while (true) 
		{
			float percent = Mathf.Min (1.0f, (Time.time - initTime) / duration);

			trans.rotation = Quaternion.Lerp (initRotation, targetRotation, percent);

			if (percent >= 1) break;
			yield return null;
		}

	}

	protected virtual IEnumerator ApplyDamage()
	{
		lastAttackTime = Time.time + delayToApplyDamage;

		yield return new WaitForSeconds (delayToApplyDamage);

		if( attackEffect )
			ParticleManager.GetInstance ().Show (attackEffect.name, attackArea.position);

		if (enemyWeapon) 
		{
			enemyWeapon.Fire ();
		} 
		else 
		{
			int amount = Physics.OverlapSphereNonAlloc (attackArea.position, attackArea.radius, nonAllocCollider);
			for (int i = 0; i < amount; i++)
				if (nonAllocCollider [i].tag == "Player") 
					nonAllocCollider [i].GetComponent<VitalityComponent> ().TakeDamage (damage, attackArea.position);
		}


	}

	private void RecoveryCache()
	{
		agent = GetComponent<NavMeshAgent> ();
		vitality = GetComponent<VitalityComponent> ();
		anim = GetComponent<Animator> ();
		trans = GetComponent<Transform> ();

		vitality.onDeath.AddListener (Death);
	}

	private void Death()
	{
		gameObject.SetActive (false);
	}
		

}
