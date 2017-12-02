using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

	private CinemachineVirtualCamera cam;

	private void Awake()
	{
		cam = GetComponent<CinemachineVirtualCamera> ();
		this.CalculateOrthoSize ();
	}

	private void CalculateOrthoSize()
	{
		LensSettings settings = cam.m_Lens;
		settings.OrthographicSize = Screen.height/2.0f/100.0f;
		cam.m_Lens = settings;

		// 100px - 1unit


	}

	private void Update()
	{
		#if UNITY_EDITOR
		this.CalculateOrthoSize();
		#endif
	}

}
