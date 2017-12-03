using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour 
{

	public enum ITEM_TYPE
	{
		WEAPON
	}

	[SerializeField]
	private ITEM_TYPE itemType;

	public int data;
	public UnityEvent OnPickup;

	public bool IsPicked()
	{
		return !this.gameObject.activeInHierarchy;
	}

	public void Spawn( Vector3 position )
	{
		this.transform.position = position;
	}

	public ITEM_TYPE Pickup()
	{
		this.gameObject.SetActive (false);
		this.OnPickup.Invoke ();

		return itemType;
	}

}
