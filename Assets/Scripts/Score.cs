using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
	public int score = 0;					// Player score.

	void Update ()
	{
		// Set the score text.
		guiText.text = "Score: " + score;

		if (score == 100) {

			//Unlock achievement on Google Game Services
			Social.ReportProgress("CgkIk_SmjfsREAIQBA", 100.0f, (bool success) => {
			});
		}
		if (score == 1000) {
			
			Social.ReportProgress("CgkIk_SmjfsREAIQBQ", 100.0f, (bool success) => {
			});
		}
		if (score == 5000) {
			
			Social.ReportProgress("CgkIk_SmjfsREAIQBg", 100.0f, (bool success) => {
			});
		}
		if (score == 10000) {
			
			Social.ReportProgress("CgkIk_SmjfsREAIQCA", 100.0f, (bool success) => {
			});
		}
	}
}
