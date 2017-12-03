using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour 
{
	
	public Transform[] spawnersEnemy;
	public EnemyController[] enemiesPrefab;
	public UnityEvent OnComplete;

	private UnityObjectPooling<EnemyController>[] enemiePool;
	private List<EnemyController> enemies;

	private void Awake()
	{
		enemies = new List<EnemyController> ();
		enemiePool = new UnityObjectPooling<EnemyController>[enemiesPrefab.Length];
		for (int i = 0; i < enemiePool.Length; i++)
			enemiePool [i] = new UnityObjectPooling<EnemyController> (enemiesPrefab [i]);
	}

	public void Spawn( int[] amountOfEnemies )
	{
		for (var i = 0; i < amountOfEnemies.Length; i++) 
			for (var j = 0; j < amountOfEnemies [i]; j++) 
				this.Spawn (i);
	}

	public void Spawn( int enemyIndex )
	{
		EnemyController enemy = enemiePool [enemyIndex].Get ();
		enemy.gameObject.SetActive (true);
		enemy.Spawn (spawnersEnemy [Random.Range (0, spawnersEnemy.Length)].position);
		enemy.GetVitality().onDeath.AddListener (OnDeathEnemy);
		enemies.Add (enemy);
	}

	private void OnDeathEnemy()
	{
		if ( IsWaveComplete () ) 
		{
			for (var i = 0; i < enemies.Count; i++) 
				this.RecycleEnemy ( enemies[i] );
			enemies.Clear ();

			this.OnComplete.Invoke ();
		}
	}

	private bool IsWaveComplete()
	{
		for (int i = 0; i < enemies.Count; i++) 
			if ( !enemies [i].GetVitality ().IsDepth () ) return false;
		return true;
	}

	private void RecycleEnemy( EnemyController enemy )
	{
		enemy.GetVitality ().onDeath.RemoveListener (OnDeathEnemy);

		for (var i = 0; i < enemiePool.Length; i++) 
		{
			if (enemiePool [i].objectReference.name == enemy.name) 
			{
				enemiePool [i].Recycle (enemy);
				break;
			}
		}
	}
}
