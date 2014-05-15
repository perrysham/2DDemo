//#####################################################################################
//#####################################################################################
//#####################################################################################
// COUNTDOWN TIMER CODE FOR THE UNITY 3D ENGINE IN C SHARP
// THIS HAS BEEN WRITTEN BY KYLE D'AOUST FOR FREE AND EDUCATIONAL USE, ROYALTY FREE.
// FEEL FREE TO USE THIS CODE IN ANY OF YOUR PROJECTS
//#####################################################################################
//#####################################################################################
//#####################################################################################

using UnityEngine;
using System.Collections;

public class CountdownTimer_CSHARP : MonoBehaviour 
{
	// For our timer we will use minutes and seconds
	public float Seconds = 29;
	public float Minutes = 2;
	private Score score;				// Reference to the Score script.
	private GameObject loader;

	void Awake ()
	{
		// Setting up the reference.
		score = GameObject.Find("Score").GetComponent<Score>();
		loader = GameObject.Find ("LoadingTimer");
		//loader.GetComponentInChildren<LoadingTimer> ().enabled = false;
		loader.SetActive (false);
	}

	void Start(){

	}
	
	void Update()
	{
		// This is if statement checks how many seconds there are to decide what to do.
		// If there are more than 0 seconds it will continue to countdown.
		// If not then it will reset the amount of seconds to 59 and handle the minutes;
		// Handling the minutes is very similar to handling the seconds.
		//print (Seconds);
		if(Seconds <= 0)
		{
			Seconds = 59;
			if(Minutes >= 1)
			{
				Minutes--;
			}
			else
			{
				Minutes = 0;
				Seconds = 0;
				// This makes the guiText show the time as X:XX. ToString.("f0") formats it so there is no decimal place.
				GameObject.Find("TimerText").guiText.text = Minutes.ToString("f0") + ":0" + (Seconds).ToString("f0");
				Social.ReportScore(score.score, "CgkIk_SmjfsREAIQAw", (bool success) => {
				});
				gameObject.SetActive(false);
				print ("this is ogre");
				loader.SetActive(true);
			}
		}
		else
		{
			Seconds -= Time.deltaTime;
			Social.ReportProgress("CgkIk_SmjfsREAIQBw", 100.0f, (bool success) => {
			});
		}
		
		// These lines will make sure the time is shown as X:XX and not X:XX.XXXXXX
		if(Mathf.Round(Seconds) <= 9)
		{
			GameObject.Find("TimerText").guiText.text = Minutes.ToString("f0") + ":0" + Seconds.ToString("f0");
		}
		else
		{
			GameObject.Find("TimerText").guiText.text = Minutes.ToString("f0") + ":" + Seconds.ToString("f0");
		}
	}
}