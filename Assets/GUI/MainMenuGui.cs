/*
 * Copyright (C) 2014 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;

public class MainMenuGui : BaseGui {

	WidgetConfig TitleCfg = new WidgetConfig(0.0f, -0.24f, 1.0f, 0.2f, 100, "Commando Pug");
	WidgetConfig PlayhCfg = new WidgetConfig(0.0f, -0.1f, 0.8f, 0.2f, 60, "Play");
	WidgetConfig AchievCfg = new WidgetConfig(0.0f, 0.015f, 0.8f, 0.2f, 60, "Achievements");
	WidgetConfig LeadCfg = new WidgetConfig(0.0f, 0.13f, 0.8f, 0.2f, 60, "Leaderboards");
	WidgetConfig SignOutCfg = new WidgetConfig(WidgetConfig.WidgetAnchor.Bottom, 0.4f, -0.055f, 0.4f, 0.15f,
	                                           TextAnchor.MiddleCenter, 45, "Sign Out");

	float standbyTimer;
	System.Action<bool> mAuthCallback;
	bool mAuthOnStart = true;
	
	public void Start() {
		//InvitationManager.Instance.Setup();
		//PlayGamesPlatform.Activate();
		//
	}
	
	public void Update() {
		// if an invitation arrived, switch to the "invitation incoming" GUI
		// or directly to the game, if the invitation came from the notification
		/*Invitation inv = InvitationManager.Instance.Invitation;
		if (inv != null) {
			if (InvitationManager.Instance.ShouldAutoAccept) {
				// jump straight into the game, since the user already indicated
				// they want to accept the invitation!
				InvitationManager.Instance.Clear();
				RaceManager.AcceptInvitation(inv.InvitationId);
				gameObject.GetComponent<RaceGui>().MakeActive();
			} else {
				// show the "incoming invitation" screen
				gameObject.GetComponent<IncomingInvitationGui>().MakeActive();
			}
		}*/


		//SetStandBy("Please wait...");

		//standbyTimer += standbyTimer;

		//Debug.Log (standbyTimer);

		//if (standbyTimer == 2) {

		//	EndStandBy();
		//}
	}
	
	protected override void DoGUI() {
		GuiLabel(TitleCfg);
		Screen.orientation = ScreenOrientation.LandscapeRight;
		
		if (GuiButton(PlayhCfg)) {
			//RaceManager.CreateQuickGame();
			//gameObject.GetComponent<RaceGui>().MakeActive();
			Application.LoadLevel(2);
		} else if (GuiButton(AchievCfg)) {
			//RaceManager.CreateWithInvitationScreen();
			//gameObject.GetComponent<RaceGui>().MakeActive();
			Social.ShowAchievementsUI();
		} //else if (GuiButton(MultCfg)) {
			//RaceManager.AcceptFromInbox();
			//gameObject.GetComponent<RaceGui>().MakeActive();
			//Social.ShowLeaderboardUI();
			//Screen.orientation = ScreenOrientation.Portrait;
			//NetworkManager.CreateQuickGame();
			//Application.LoadLevel(3);

		//} 
	else if (GuiButton(LeadCfg)) {

			Social.ShowLeaderboardUI();

		} else if (GuiButton(SignOutCfg)) {
			DoSignOut();
		}
	}
	
	void DoSignOut() {
		PlayGamesPlatform.Instance.SignOut();
		//gameObject.GetComponent<WelcomeGui>().SetAuthOnStart(false);
		//gameObject.GetComponent<WelcomeGui>().MakeActive();
		Application.LoadLevel (0);
	}
}
