using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemSpawner : MonoBehaviour 
{
	private static Collider[] nonAlloc = new Collider[10];

	public Transform[] spawnersItem;
	public Item[] itensPrefab;
	public UnityEvent OnComplete;

	private UnityObjectPooling<Item>[] itemPool;
	private List<Item> itens;

	private void Awake()
	{
		itens = new List<Item> ();
		itemPool = new UnityObjectPooling<Item>[itensPrefab.Length];
		for (int i = 0; i < itemPool.Length; i++)
			itemPool [i] = new UnityObjectPooling<Item> (itensPrefab [i]);
	}

	public void Spawn( int[] amountOfEnemies )
	{
		for (var i = 0; i < amountOfEnemies.Length; i++) 
			for (var j = 0; j < amountOfEnemies [i]; j++) 
				this.Spawn (i);
	}

	public void Spawn( int enemyIndex )
	{
		Vector3 position = Vector3.zero;

		if ( this.FindFreePosition (ref position) ) 
		{
			Item item = itemPool [enemyIndex].Get ();
			item.gameObject.SetActive (true);
			item.Spawn (position);
			item.OnPickup.AddListener (OnPickedHandler);
			itens.Add (item);
		}
	}

	private bool FindFreePosition( ref Vector3 position )
	{
		for (int i = 0; i < 5; i++) 
		{
			position = spawnersItem [Random.Range (0, spawnersItem.Length)].position;
            //for( int j = 0 ; j < itens.Count ; j++ )
            //    if( Vector3.SqrMagnitude(itens[j].transform.position- )
            int amount = Physics.OverlapSphereNonAlloc(position, 1.0f, nonAlloc);
            bool hasItem = false;
            for (int j = 0 ; j < amount ; j++)
                if (nonAlloc[j].tag == "Item")
                    hasItem = true;
            if (!hasItem)
                return true;
        }
			
		return false;
	}

	private void OnPickedHandler()
	{
		if ( IsWaveComplete () ) 
		{
			for (var i = 0; i < itens.Count; i++) 
				this.RecycleItem ( itens[i] );
			itens.Clear ();

			this.OnComplete.Invoke ();
		}
	}

	private bool IsWaveComplete()
	{
		for (int i = 0; i < itens.Count; i++) 
			if ( !itens [i].IsPicked() ) return false;
		return true;
	}

	private void RecycleItem( Item item )
	{
		item.OnPickup.RemoveListener (OnPickedHandler);

		for (var i = 0; i < itemPool.Length; i++) 
		{
			if (itemPool [i].objectReference.name == item.name) 
			{
				itemPool [i].Recycle (item);
				break;
			}
		}
	}

}
