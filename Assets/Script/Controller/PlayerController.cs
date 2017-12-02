using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 200.0f;
	public Vitality vitality;
    

    [SerializeField]
    private Weapon defaultWeapon;

    private Weapon currentWeapon;
    private Rigidbody2D body;
	private Transform trans;
	private Camera cam;

    private void Awake()
    {
        if (defaultWeapon == null)
            throw new System.Exception("Nenhuma arma inicial configurada.");

        body = GetComponent<Rigidbody2D>();
		trans = GetComponent<Transform> ();
		cam = Camera.main;
        currentWeapon = defaultWeapon;
    }

    private void Update()
    {
		this.MovePosition ();
		this.MoveRotation ();

		if( Input.GetButton("Fire1") ) currentWeapon.Fire();
    }

	private void MovePosition()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		body.velocity = new Vector3(horizontal, vertical, 0) * moveSpeed * Time.deltaTime;
	}

	private void MoveRotation()
	{
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);

		Vector2 direction = (new Vector2 (ray.origin.x - this.trans.position.x, ray.origin.y - this.trans.position.y)).normalized;
		float angle = Mathf.Atan2 (direction.x , -direction.y) * Mathf.Rad2Deg + 180;
		body.rotation = angle;
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
		

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (!Application.isPlaying) return;

		if (cam == null) cam = Camera.main;

		Gizmos.color = Color.red;
		Gizmos.DrawLine ( this.transform.position, this.GetAimPoint() );
	}
#endif

}
