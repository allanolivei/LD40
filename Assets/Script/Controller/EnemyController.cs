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
	public CircleArea attackArea;

	private float lastAttackTime;
	private NavMeshAgent agent;
	private VitalityComponent vitality;
	private Collider[] nonAllocCollider;

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

		this.RecoveryCache ();
	}

	private void Update()
	{
		agent.SetDestination (PlayerController.current.trans.position);

		if (agent.remainingDistance <= agent.stoppingDistance && Time.time - lastAttackTime > cooldown) 
			this.Attack ();
	}

	protected virtual void Attack()
	{
		lastAttackTime = Time.time;

		int amount = Physics.OverlapSphereNonAlloc (attackArea.position, attackArea.radius, nonAllocCollider);
		for (int i = 0; i < amount; i++)
			if (nonAllocCollider [i].tag == "Player") 
				nonAllocCollider [i].GetComponent<VitalityComponent> ().TakeDamage (damage, attackArea.position);
	}

	private void RecoveryCache()
	{
		agent = GetComponent<NavMeshAgent> ();
		vitality = GetComponent<VitalityComponent> ();

		vitality.onDeath.AddListener (Death);
	}

	private void Death()
	{
		gameObject.SetActive (false);
	}
		

}
