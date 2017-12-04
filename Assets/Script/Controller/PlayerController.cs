using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController), typeof(VitalityComponent))]
public class PlayerController : MonoBehaviour
{

	public static PlayerController current;

	public float bloodDamage = 6.0f;
    public float moveSpeed = 2.0f;
	public float smoothRotation = 16.0f;
    

    [SerializeField]
    private Weapon[] weapons;

	[System.NonSerialized, HideInInspector]
	public CharacterController controller;
	[System.NonSerialized, HideInInspector]
	public Transform trans;
	[System.NonSerialized, HideInInspector]
	public VitalityComponent vitality;

    private Weapon currentWeapon;
	private Camera cam;
	private float currentSpeed;

	/*********************** PUBLIC UTILITIES **********************/
	public void Aim( Vector3 direction )
	{
		float angle = Mathf.Atan2 (direction.x , -direction.z) * Mathf.Rad2Deg + 180;
		trans.rotation = Quaternion.Lerp(trans.rotation, Quaternion.AngleAxis (-angle, Vector3.up), smoothRotation * Time.deltaTime);
	}

	public void SelectWeapon(int weaponIndex)
	{
		if( currentWeapon != null )
			currentWeapon.gameObject.SetActive (false);

		currentWeapon = weapons[weaponIndex];

		if( currentWeapon != null )
			currentWeapon.gameObject.SetActive (true);
	}
	/***************************************************************/


	/************************ UNITY MESSAGE ************************/
    private void Awake()
    {
		current = this;

		this.currentSpeed = moveSpeed;

		this.RecoveryCache ();
		this.vitality.onDeath.AddListener (OnDeathHandler);

		this.SelectWeapon (0);
    }

	private void Update()
	{
		if( Input.GetButton("Fire1") ) currentWeapon.Fire();
	}

    private void FixedUpdate()
    {
		this.MovePosition ();
	}

	private void OnTriggerExit( Collider other )
	{
		if (other.tag == "AlienBlood")
			currentSpeed = moveSpeed;
	}

	private void OnTriggerStay( Collider other )
	{
		string tag = other.tag;

		switch (tag) 
		{
			case "AlienBlood":
				this.currentSpeed = moveSpeed * 0.5f;
				this.vitality.TakeDamage (bloodDamage * Time.deltaTime, this.trans.position);
			break;
			case "Item":
				Item item = other.GetComponent<Item> ();
				Item.ITEM_TYPE itemType = item.Pickup ();
				if( itemType.Equals(Item.ITEM_TYPE.WEAPON) )
					this.SelectWeapon(item.data);
			break;
		}
	}
	/***************************************************************/



	/********************* PRIVATE UTILITIES ***********************/
	private void RecoveryCache()
	{
		controller = GetComponent<CharacterController>();
		trans = GetComponent<Transform> ();
		vitality = GetComponent<VitalityComponent> ();
		cam = Camera.main;
	}

	private void MovePosition()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		controller.Move( new Vector3(horizontal, 0, vertical) * currentSpeed * Time.deltaTime );

		// *FIX y
		Vector3 pos = this.trans.position; pos.y = 0;
		this.trans.position = pos;
	}

	private void OnDeathHandler()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	private Vector2 GetAimPoint()
	{
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);
		return new Vector2 (ray.origin.x, ray.origin.y);
	}

	private Vector2 GetAimDirection()
	{
		return (this.GetAimPoint() - (Vector2)this.trans.position).normalized;
	}
	/*****************************************************/
		



}
