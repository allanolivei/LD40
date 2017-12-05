using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScreen : MonoBehaviour 
{
	
	public PlayerController player;

	public ProgBar lifeBar;
	public ProgBar weaponBar;
	public Text scoreView;

	private void Update()
	{
		lifeBar.SetValue (player.vitality.data.GetLife () / player.vitality.data.initialLife);

		WeaponDefault weapon = player.currentWeapon as WeaponDefault;
		if (weapon.timeToFinish > 0.1f) {
			weaponBar.gameObject.SetActive (true);
			weaponBar.SetValue (weapon.timeToFinish / (Time.time - weapon.initWeaponTime));
		} else {
			weaponBar.gameObject.SetActive (false);
		}

		scoreView.text = string.Format ("{0:D7}", PlayerController.POINTS);

	}

}
