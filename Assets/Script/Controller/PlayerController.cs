using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController), typeof(VitalityComponent))]
public class PlayerController : MonoBehaviour
{

	public static PlayerController current;

    public float moveSpeed = 2.0f;
	public float smoothRotation = 16.0f;
    

    [SerializeField]
    private Weapon defaultWeapon;

	[System.NonSerialized, HideInInspector]
	public CharacterController controller;
	[System.NonSerialized, HideInInspector]
	public Transform trans;
	[System.NonSerialized, HideInInspector]
	public VitalityComponent vitality;

    private Weapon currentWeapon;
	private Camera cam;

	public void Aim( Vector3 direction )
	{
		float angle = Mathf.Atan2 (direction.x , -direction.z) * Mathf.Rad2Deg + 180;
		trans.rotation = Quaternion.Lerp(trans.rotation, Quaternion.AngleAxis (-angle, Vector3.up), smoothRotation * Time.deltaTime);
	}

    private void Awake()
    {
		current = this;

        if (defaultWeapon == null)
            throw new System.Exception("Nenhuma arma inicial configurada.");

		this.RecoveryCache ();
		this.vitality.onDeath.AddListener (OnDeathHandler);
    }

	private void Update()
	{
		if( Input.GetButton("Fire1") ) currentWeapon.Fire();
	}

    private void FixedUpdate()
    {
		this.MovePosition ();
	}

	private void OnTriggerStay( Collider other )
	{
		if (other.tag == "AlienBlood") 
			this.vitality.TakeDamage (4 * Time.deltaTime, this.trans.position);
	}

	private void RecoveryCache()
	{
		controller = GetComponent<CharacterController>();
		trans = GetComponent<Transform> ();
		vitality = GetComponent<VitalityComponent> ();
		cam = Camera.main;
		currentWeapon = defaultWeapon;
	}

	private void MovePosition()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		controller.Move( new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime );

		// *FIX y
		Vector3 pos = this.trans.position; pos.y = 0;
		this.trans.position = pos;
	}

	/*
	private void MoveRotation()
	{
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);

		Vector2 direction = (new Vector2 (ray.origin.x - this.trans.position.x, ray.origin.y - this.trans.position.y)).normalized;
		float angle = Mathf.Atan2 (direction.x , -direction.y) * Mathf.Rad2Deg + 180;
		body.rotation = angle;
	}
	*/

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
		



}
