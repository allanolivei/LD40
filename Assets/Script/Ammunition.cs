using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Asset/GameData/Ammunition")]
public class Ammunition : ScriptableObject 
{
	public Bullet prefab;

	private Stack<Bullet> pooling = new Stack<Bullet> ();

	public Bullet GetBullet()
	{
		Bullet bullet = (pooling.Count > 0) ? pooling.Pop () : Instantiate<Bullet> (prefab);
		bullet.gameObject.SetActive (true);
		bullet.ammunition = this;
		return bullet;
	}

	public void Recycle( Bullet bullet )
	{
		bullet.gameObject.SetActive (false);
		pooling.Push (bullet);
	}

}
