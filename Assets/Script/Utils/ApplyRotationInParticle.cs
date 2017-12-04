using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyRotationInParticle : MonoBehaviour 
{

	public bool onlyStart = true;
	public ParticleSystem[] ps;

	private void Start()
	{
		this.ApplyRotation ();
	}

	private void OnEnable()
	{
		this.ApplyRotation ();
	}

	private void ApplyRotation()
	{
		float angle = this.transform.eulerAngles.y;
		//ParticleSystem.MainModule module = ps.main;
		//module.startRotation = new ParticleSystem.MinMaxCurve(angle);;
		Debug.Log("ROTATION: "+angle);
		foreach( ParticleSystem p in ps )
			p.startRotation = angle;	
		//ps.main.startSize = 
	}


}
