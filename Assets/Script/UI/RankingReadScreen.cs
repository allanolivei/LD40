using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayerIOClient;

public class RankingReadScreen : MonoBehaviour 
{
	struct Rank
	{
		public string name;
		public int score;
	}


	public GameObject rankingModalGO;
	public GameObject waitingGO;
	public RankingLine[] lines;

	public void GoBackMainMenu()
	{
		SceneManager.LoadScene (0);
	}

	private void Start()
	{
		AuthenticateAndLoad ();
	}

	private void AuthenticateAndLoad()
	{
		waitingGO.SetActive (true);
		rankingModalGO.SetActive (false);

		string userId = "Guest" + UnityEngine.Random.Range(0, 10000);

		PlayerIO.Authenticate(
			"bloodblood-i1xisc4vquzylare1lvma",            //Your game id
			"public",                               //Your connection id
			new Dictionary<string, string>
			{        
				{ "userId", userId }
			},
			null,                                   //PlayerInsight segments
			delegate ( Client client ) 
			{
				Debug.Log("auth complete");
				this.LoadRanking(client);
			},
			delegate ( PlayerIOError error ) {
				Debug.Log("Error connecting: " + error.ToString());
				this.GoBackMainMenu();
			}
		);
	}

	private void LoadRanking( Client client )
	{
		Rank[] ranks = new Rank[lines.Length];

		client.BigDB.LoadKeysOrCreate(
			"PlayerObjects",
			new[] { "0", "1", "2", "3", "4" },
			delegate ( DatabaseObject[] data )
			{
				//carrega a lista a partir dos dados salvos
				for (var i = 0 ; i < data.Length ; i++)
				{ 
					if ( data[i].Contains("name") )
						ranks[i] = new Rank() { name = data[i].GetString("name"), score = data[i].GetInt("score") };
					else
						ranks[i] = new Rank() { name = string.Empty, score = int.MinValue };
				}

				//inseri player na posicao atual
				System.Array.Sort(ranks, ( Rank a, Rank b ) => b.score - a.score);

				//mostra rank
				this.ShowRanking(ranks);



			},
			delegate ( PlayerIOError result )
			{
				Debug.Log("Error connecting: " + result.ToString());
			}
		);
	}

	private void ShowRanking( Rank[] ranks )
	{
		waitingGO.SetActive (false);
		rankingModalGO.SetActive (true);

		for (int i = 0; i < lines.Length; i++) 
		{
			Rank r = ranks [i];
			lines [i].SetValue (i + 1, r.name, r.score, false, false);
		}
	}
}
