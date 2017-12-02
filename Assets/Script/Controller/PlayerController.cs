using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 2.0f;
	public Vitality vitality;
	public float smoothRotation = 16.0f;
    

    [SerializeField]
    private Weapon defaultWeapon;

    private Weapon currentWeapon;
	private CharacterController controller;
	private Transform trans;
	private Camera cam;

	public void Aim( Vector3 direction )
	{
		float angle = Mathf.Atan2 (direction.x , -direction.z) * Mathf.Rad2Deg + 180;
		trans.rotation = Quaternion.Lerp(trans.rotation, Quaternion.AngleAxis (-angle, Vector3.up), smoothRotation * Time.deltaTime);
	}

    private void Awake()
    {
        if (defaultWeapon == null)
            throw new System.Exception("Nenhuma arma inicial configurada.");

		this.RecoveryCache ();
    }

	private void Update()
	{
		//this.MoveRotation ();
		if( Input.GetButton("Fire1") ) currentWeapon.Fire();
	}

    private void FixedUpdate()
    {
		this.MovePosition ();
	}

	private void RecoveryCache()
	{
		controller = GetComponent<CharacterController>();
		trans = GetComponent<Transform> ();
		cam = Camera.main;
		currentWeapon = defaultWeapon;
	}

	private void MovePosition()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		//body.velocity = new Vector3(horizontal, vertical, 0) * moveSpeed * Time.deltaTime;
		controller.Move( new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime );
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
