using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnerController : MonoBehaviour 
{
	// tres inimigos
	// cada orda contem um número diferente de cada inimigo
	[System.Serializable]
	public struct Wave
	{
		public int[] amountByEnemy;
	}


	public Wave[] waves;
	public Transform[] spawners;
	public EnemyController[] enemyPrefabs;

	private int currentSpawner = 0;
	private UnityObjectPooling<EnemyController>[] pooling;
	private List<EnemyController> enemies;

	private void Awake()
	{
		enemies = new List<EnemyController> ();
		pooling = new UnityObjectPooling<EnemyController>[enemyPrefabs.Length];
		for (int i = 0; i < pooling.Length; i++)
			pooling [i] = new UnityObjectPooling<EnemyController> (enemyPrefabs [i]);

		this.Spawn ();
	}

	private void NextSpawn()
	{
		currentSpawner++;
		this.Spawn ();
	}

	private void Spawn()
	{
		Wave wave = waves [Mathf.Min (currentSpawner, waves.Length - 1)];

		for (var i = 0; i < wave.amountByEnemy.Length; i++) 
		{
			for (var j = 0; j < wave.amountByEnemy [i]; j++) 
			{
				EnemyController enemy = pooling [i].Get ();
				enemy.gameObject.SetActive (true);
				enemy.Spawn (spawners [Random.Range (0, spawners.Length)].position);
				enemy.GetVitality().onDeath.AddListener (OnDeathEnemy);
				enemies.Add (enemy);
			}
		}
	}

	private void OnDeathEnemy()
	{
		Debug.Log ("Check is Complete");
		if ( IsWaveComplete () ) 
		{
			Debug.Log ("Is Wave Complete");
			for (var i = 0; i < enemies.Count; i++) 
				this.RecycleEnemy ( enemies[i] );
			enemies.Clear ();

			this.NextSpawn ();
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

		for (var i = 0; i < pooling.Length; i++) 
		{
			if (pooling [i].objectReference.name == enemy.name) 
			{
				pooling [i].Recycle (enemy);
				break;
			}
		}
	}


}
