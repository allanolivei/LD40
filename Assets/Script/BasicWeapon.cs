using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : Weapon 
{

	public Ammunition munition;
	public Transform spawnPoint;

	protected override void Shoot ()
	{
		if (munition == null) 
		{
			Debug.Log ("Dont have munition");
			return;
		}

		Bullet bullet = munition.GetBullet ();
		bullet.SetData (spawnPoint.position, this.transform.up);
	}

}
