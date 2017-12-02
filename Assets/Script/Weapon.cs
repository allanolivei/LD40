using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	public float cooldown = 1.0f;

	private float lastShootTime;

	public virtual void Fire()
    {
		if (Time.time - this.lastShootTime > cooldown) 
		{
			this.lastShootTime = Time.time;
			this.Shoot();
		}
    }

	protected abstract void Shoot ();
}
