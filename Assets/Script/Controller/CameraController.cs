using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	public PlayerController player;
	public float smoothAimMovement = 10.0f;

	//private CinemachineVirtualCamera cam;
	private Camera cam;
	private Transform trans;
	private Transform playerTrans;
	private Vector3 lastAimMovement;

	private void RecoveryCache()
	{
		cam = GetComponent<Camera>();
		trans = GetComponent<Transform> ();
		playerTrans = player.transform;
	}

	private void CalculateOrthoSize()
	{
		//LensSettings settings = cam.m_Lens;
		//settings.OrthographicSize = Screen.height/2.0f/100.0f;
		//cam.m_Lens = settings;

		// 100px - 1unit
		cam.orthographicSize = Screen.height/2.0f/100.0f;

	}

	private Vector3 GetAimPoint()
	{
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);
		return new Vector3 (ray.origin.x, 0, ray.origin.z);
	}

	private Vector3 GetAimDirection()
	{
		Vector3 aim = (this.GetAimPoint () - playerTrans.position);
		aim.y = 0;
		return aim.normalized;
	}

	private void Awake()
	{
		//cam = GetComponent<CinemachineVirtualCamera> ();
		this.RecoveryCache();
		this.CalculateOrthoSize ();
	}

	private void Update()
	{
		#if UNITY_EDITOR
		this.CalculateOrthoSize();
		#endif

	}

	private void LateUpdate()
	{
		Vector2 halfScreen = new Vector2 (Screen.width, Screen.height) * 0.5f;

		Vector3 aimMovement = new Vector3( 
			(Input.mousePosition.x - halfScreen.x)/halfScreen.x, 0, 
			(Input.mousePosition.y - halfScreen.y)/halfScreen.y 
		) * 1.4f;
		lastAimMovement = Vector3.Lerp(lastAimMovement, aimMovement, smoothAimMovement * Time.deltaTime);;
		Vector3 position = playerTrans.position + new Vector3 (0,5.0f,0);
		this.trans.position = position + lastAimMovement;

		this.player.Aim ( this.GetAimDirection() );
	}

	#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (!Application.isPlaying) return;

		if (cam == null) cam = Camera.main;

		Gizmos.color = Color.red;
		Gizmos.DrawLine ( playerTrans.position, this.GetAimPoint() );
	}
	#endif

}
