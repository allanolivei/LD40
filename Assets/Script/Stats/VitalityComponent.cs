using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VitalityComponent : MonoBehaviour 
{
	public SpriteRenderer splash;
	public VitalityData data;
	public Color damageColor;
	public SpriteRenderer spriteRendererColor;
	public float pointsScale = 0.0f;
	[Header("Use when data is null")]
	public float initialLife = 100;
	[Header("Effects")]
	public ParticleSystem damageEffectPrefab;
	public ParticleSystem deapthEffectPrefab;

	public AudioClip damageClip;

	public UnityEvent onDeath;

	private float lastDamageTime;
	private float lastDamageEffectTime;

	public bool IsDepth()
	{
		return data.GetLife() <= 0;
	}

	public bool TakeDamage( float value, Vector3 position )
	{
		bool result = data.TakeDamage (value);

		PlayerController.POINTS += Mathf.RoundToInt (value * pointsScale);

		if( splash != null )
			Instantiate (splash, position, splash.transform.rotation);
		
		if (result) 
		{
			onDeath.Invoke ();
			if( deapthEffectPrefab )
				ParticleManager.GetInstance ().Show (deapthEffectPrefab.name, position);
		} 
		else if( Time.time-lastDamageEffectTime > 0.25f )
		{
			if (spriteRendererColor)
				StartCoroutine ( BlinkColor() );

			if( damageEffectPrefab )
				ParticleManager.GetInstance ().Show (damageEffectPrefab.name, position);

			if (damageClip != null)
				AudioSource.PlayClipAtPoint (damageClip, Vector3.zero);

			lastDamageEffectTime = Time.time;
		}

		lastDamageTime = Time.time;


		return result;
	}
		
	private void Awake()
	{
		if (this.data == null) 
		{
			this.data = ScriptableObject.CreateInstance<VitalityData> ();
			this.data.initialLife = initialLife;
		}


		if (damageEffectPrefab != null)
			ParticleManager.GetInstance ().Register (damageEffectPrefab);
		if (deapthEffectPrefab != null)
			ParticleManager.GetInstance ().Register (deapthEffectPrefab);
	}

	private void OnEnable()
	{
		this.data.Reset ();
	}

	private void OnDisable()
	{
		spriteRendererColor.color = Color.white;
	}

	private IEnumerator BlinkColor()
	{
		float duration = 0.01f;
		Color colorA = Color.white;
		Color colorB = damageColor;

		for (int i = 0; i < 4; i++) 
		{
			// anim transition
			float initTime = Time.time;
			while (true) 
			{
				float percent = Mathf.Min (1.0f, (Time.time - initTime) / duration);
				spriteRendererColor.color = Color.Lerp (colorA, colorB, percent);
				if (percent >= 1.0f) break;
				yield return null;
			}

			// swapColor
			Color tempColor = colorA;
			colorA = colorB;
			colorB = tempColor;
		}
	}

}
