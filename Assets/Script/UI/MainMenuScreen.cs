using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour 
{

	public void PlayGame()
	{
		SceneManager.LoadScene (1);
	}

	public void ShowRanking()
	{
		SceneManager.LoadScene (3);
	}

}
