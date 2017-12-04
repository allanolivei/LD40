using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDefault : Weapon 
{

	public ParticleSystem effectShoot;
	public int numberOfBullet = 1;
	public float angleBetweenBullet = 0;

	protected override void Shoot ()
	{
		//Bullet bullet = ammunition.GetBullet ();
		//bullet.SetData (spawnPoint.position, this.transform.forward);
		float offsetAngle = (angleBetweenBullet * numberOfBullet) / 2;
		float forwardAngle = this.transform.eulerAngles.y;

		for (int i = 0; i < numberOfBullet; i++) 
		{
			Bullet bullet = ammunition.GetBullet ();
			bullet.SetData (spawnPoint.position, Quaternion.AngleAxis(forwardAngle - offsetAngle + angleBetweenBullet * i, Vector3.up) * Vector3.forward);
		}

		if (effectShoot != null)
			effectShoot.Play ();
	}

	private void OnEnable()
	{
		ammunition.Validate ();
	}

}
