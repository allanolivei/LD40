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
	public float rotateSpeed = 10.0f;
	public UnityEvent OnPickup;

	private Transform trans;

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

	private void Awake()
	{
		this.trans = GetComponent<Transform>();
	}

	private void Update()
	{
		this.trans.Rotate( 0,rotateSpeed * Time.deltaTime,0 );
		this.trans.localScale = Vector3.one * (0.85f+Mathf.PingPong (Time.time * 0.3f, 0.15f));
	}
}
