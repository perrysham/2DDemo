using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;

public class bullet : MonoBehaviour {

	float score = 0;

	// Use this for initialization
	void Start () {
	
		Destroy(gameObject, 2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D col) 
	{
		// If it hits an enemy...
		if(col.tag == "Enemy")
		{
			Destroy (gameObject);
			Debug.Log ("hit");
			score = score + 20;
			GameObject.Find("score").guiText.text = score.ToString("f0");
			print (score);

		}
	}	
}
