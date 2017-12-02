using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(VitalityComponent))]
public class EnemyController : MonoBehaviour 
{
	public float moveSpeed = 100.0f;

	private NavMeshAgent agent;
	private VitalityComponent vitality;

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
		this.RecoveryCache ();
	}

	private void Update()
	{
		agent.SetDestination (PlayerController.current.trans.position);
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
