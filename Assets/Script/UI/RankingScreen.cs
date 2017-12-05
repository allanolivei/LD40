using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayerIOClient;

public class RankingScreen : MonoBehaviour 
{
	struct Rank
	{
		public string name;
		public int score;
	}


	public GameObject rankingModalGO;
	public GameObject inputFieldGO;
	public GameObject waitingGO;

	public InputField input;

	public RankingLine[] lines;

	private Rank userRank;

	public void PlayGame()
	{
		SceneManager.LoadScene (1);
	}

	public void SubmitScore()
	{
		this.AuthenticateAndLoad ();
	}

	private void Start()
	{
		inputFieldGO.SetActive (true);
	}

	private void AuthenticateAndLoad()
	{
		inputFieldGO.SetActive (false);
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
				this.PlayGame();
			}
		);
	}

	private void LoadRanking( Client client )
	{
		Rank[] ranks = new Rank[lines.Length];

		string n = input.text;

		userRank = new Rank()
		{
			name = string.IsNullOrEmpty(n) ? "Undefined" : n.Substring(0, Mathf.Min(10,n.Length)),
			score = PlayerController.POINTS
		};

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
				ranks[ranks.Length - 1] = userRank;
				System.Array.Sort(ranks, ( Rank a, Rank b ) => b.score - a.score);

				//mostra rank
				this.ShowRanking(ranks);

				//salvar lista
				for (var i = 0 ; i < data.Length ; i++)
				{
					if( !string.IsNullOrEmpty(ranks[i].name) )
					{
						Debug.Log("SAVE ELEMENT");
						data[i].Set("name", ranks[i].name);
						data[i].Set("score", ranks[i].score);
						data[i].Save();
					}
					else
					{
						Debug.Log("DONT SAVE ELEMENT");
					}

				}




				//int i = 0;
				//for (; i < data.Length && data[i].Contains("name") ; i++);

				//if( i > 5 )
				//{
				//    data[i].Set("name", r.name);
				//    data[i].Set("distancia", r.distancia);
				//    data[i].Save();
				//}

				//for (var i = 0 ; i < data.Length ; i++)
				//{
				//    if (!data[i].Contains("name"))
				//    {
				//        data[i].Set("name", "Allan");
				//        data[i].Set("distance", 10);
				//        data[i].Save();
				//        break;
				//    }
				//    else
				//    {
				//    }
				//}

			},
			delegate ( PlayerIOError result )
			{
				Debug.Log("Error connecting: " + result.ToString());
			}
		);
	}

	private void ShowRanking( Rank[] ranks )
	{
		inputFieldGO.SetActive (false);
		waitingGO.SetActive (false);
		rankingModalGO.SetActive (true);

		for (int i = 0; i < lines.Length; i++) 
		{
			Rank r = ranks [i];
			bool isPlayer = r.name == userRank.name && r.score == userRank.score;
			lines [i].SetValue (i + 1, r.name, r.score, isPlayer, i==lines.Length-1 && isPlayer);
		}
	}
}
