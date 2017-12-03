using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Asset/GameData/Ammunition")]
public class Ammunition : ScriptableObject 
{
	public Bullet prefab;

	private List<Bullet> pooling = new List<Bullet> ();

	public Bullet GetBullet()
	{
		Bullet bullet = null;
		if (pooling.Count > 0) 
		{
			bullet = pooling [0];
			pooling.RemoveAt (0);
		}
		else 
			bullet = Instantiate<Bullet> (prefab);

		bullet.gameObject.SetActive (true);
		bullet.ammunition = this;
		return bullet;
	}

	public void Recycle( Bullet bullet )
	{
		bullet.gameObject.SetActive (false);
		pooling.Add (bullet);
	}

	public void Validate()
	{
		for (var i = this.pooling.Count-1; i >= 0; i--)
			if (this.pooling [i] == null)
				this.pooling.RemoveAt (i);
	}

}
