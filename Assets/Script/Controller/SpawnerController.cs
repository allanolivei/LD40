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
		public int[] itens;
	}

	public Wave[] waves;
	public ItemSpawner itemSpawner;
	public EnemySpawner enemySpawner;

	private int currentSpawner = 0;

	private void Awake()
	{
		this.Spawn ();

		this.enemySpawner.OnComplete.AddListener (EnemyCompleteHandler);
	}

	private void NextSpawn()
	{
		currentSpawner++;
		this.Spawn ();
	}

	private void Spawn()
	{
		Wave wave = waves [Mathf.Min (currentSpawner, waves.Length - 1)];

		enemySpawner.Spawn (wave.amountByEnemy);
		itemSpawner.Spawn (wave.itens);
	}

	private void EnemyCompleteHandler()
	{
		this.NextSpawn ();
	}


}
