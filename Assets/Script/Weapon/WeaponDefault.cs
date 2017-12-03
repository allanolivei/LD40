using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDefault : Weapon 
{


	protected override void Shoot ()
	{
		Bullet bullet = ammunition.GetBullet ();
		bullet.SetData (spawnPoint.position, this.transform.forward);
	}

	private void OnEnable()
	{
		ammunition.Validate ();
	}

}
