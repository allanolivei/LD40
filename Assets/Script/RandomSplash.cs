using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSplash : MonoBehaviour 
{
	public Sprite[] sprites;
	public float minScale = 0.9f;
	public float maxScale = 1.1f;
	public float minTime = 20.0f;
	public float maxTime = 20.0f;

	private SpriteRenderer sr;
	private Transform tr;

	private void Awake()
	{
		sr = GetComponent<SpriteRenderer> ();
		tr = GetComponent<Transform> ();
	}

	private void OnEnable()
	{
		sr.sprite = sprites [Random.Range (0, sprites.Length)];
		tr.localScale = Vector3.one * Random.Range (minScale, maxScale);
	}

}
