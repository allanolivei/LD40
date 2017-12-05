using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingLine : MonoBehaviour 
{

	public Image background;
	public Text numberField;
	public Text nameField;
	public Text scoreField;

	private Color defaultColor = Color.grey;

	private void Awake()
	{
		defaultColor = background.color;
	}

	public void SetValue( int number, string name, int score, bool highlight=false, bool hideNumber=false )
	{
		if (string.IsNullOrEmpty (name)) 
		{
			this.Empty ();
			return;
		}
		numberField.text = hideNumber ? string.Empty : string.Format ("{0:D2}", number);
		nameField.text = name;
		scoreField.text = string.Format ("{0:D7}", score);
		background.color = highlight ? Color.green : defaultColor;
	}

	public void Empty()
	{
		numberField.text = 
		nameField.text = 
		scoreField.text = string.Empty;

		background.color = defaultColor;
	}
}
