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

/*using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections.Generic;
using System;

public class NetworkManager : RealTimeMultiplayerListener {
	const string RaceTrackName = "World";
	string[] PlayerNames = new string[] { "Player1", "Player2", "Player3", "Player4" };
	const int QuickGameOpponents = 1;
	const int GameVariant = 0;
	static NetworkManager sInstance = null;
	const int MinOpponents = 1;
	const int MaxOpponents = 3;
	private Score score;				// Reference to the Score script.
	
	// points required to finish race. Must be < 255 because it has to fit in a byte
	const int PointsToFinish = 100;
	
	public enum GameState { SettingUp, Playing, Finished, SetupFailed, Aborted };
	private GameState mGameState = GameState.SettingUp;
	
	// how many points each of our fellow racers has
	//score = GameObject.Find("Score").GetComponent<Score>();
	private Dictionary<string,int> mRacerScore = new Dictionary<string,int>();
	//private mRacerScore Time.time;
	
	// whether or not we received the final score for each participant id
	private HashSet<string> mGotFinalScore = new HashSet<string>();
	
	// my participant ID
	private string mMyParticipantId = "";
	
	// my rank (1st, 2nd, 3rd, 4th, or 0 to mean 'no rank yet')
	// This is updated every time we get a finish notification from a peer
	private int mFinishRank = 0;
	
	// room setup progress
	private float mRoomSetupProgress = 0.0f;
	
	// speed of the "fake progress" (to keep the player happy)
	// during room setup
	const float FakeProgressSpeed = 1.0f;
	const float MaxFakeProgress = 30.0f;
	float mRoomSetupStartTime = 0.0f;
	
	private NetworkManager() {
		mRoomSetupStartTime = Time.time;
	}
	
	public static void CreateQuickGame() {
		sInstance = new NetworkManager();
		PlayGamesPlatform.Instance.RealTime.CreateQuickGame(QuickGameOpponents, QuickGameOpponents,
		                                                    GameVariant, sInstance);
	}
	
	public static void CreateWithInvitationScreen() {
		sInstance = new NetworkManager();
		PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(MinOpponents, MaxOpponents,
		                                                               GameVariant, sInstance);
	}
	
	public static void AcceptFromInbox() {
		sInstance = new NetworkManager();
		PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(sInstance);
	}
	
	public static void AcceptInvitation(string invitationId) {
		sInstance = new NetworkManager();
		PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitationId, sInstance);
	}
	
	public GameState State {
		get {
			return mGameState;
		}
	}
	
	public static NetworkManager Instance {
		get {
			return sInstance;
		}
	}
	
	public int FinishRank {
		get {
			return mFinishRank;
		}
	}
	
	public float RoomSetupProgress {
		get {
			float fakeProgress = (Time.time - mRoomSetupStartTime) * FakeProgressSpeed;
			if (fakeProgress > MaxFakeProgress) {
				fakeProgress = MaxFakeProgress;
			}
			float progress = mRoomSetupProgress + fakeProgress;
			return progress < 99.0f ? progress : 99.0f;
		}
	}
	
	private void SetupTrack() {
		BehaviorUtils.MakeVisible(GameObject.Find(RaceTrackName), true);
		Debug.Log ("About to get self");
		Participant self = GetSelf();
		Debug.Log ("Self is " + self);
		Debug.Log ("About to get a list of connected participants");
		List<Participant> players = GetRacers();
		Debug.Log ("Players is  " + players + " with count of " + players.Count);
		int i;
		for (i = 0; i < PlayerNames.Length; i++) {
			Debug.Log("Looking at i value of " + i);
			GameObject pug = GameObject.Find(PlayerNames[i]);
			Debug.Log ("Looking for car name " + PlayerNames[i]);
			Participant player = i < players.Count ? players[i] : null;
			Debug.Log ("Player is " + player);
			
			bool isSelf = player != null && player.ParticipantId.Equals(self.ParticipantId);
			if (player != null) {
				Debug.Log("Racer is not null!");
				BehaviorUtils.MakeVisible(pug, true);
				ControlCharacter controller = pug.GetComponent<ControlCharacter>();
				controller.SetParticipantId(player.ParticipantId);
				controller.SetBlinking(isSelf);
			} else {
				Debug.Log("Hiding racer");
				BehaviorUtils.MakeVisible(pug, false);
			}
		}
	}
	
	private void TearDownTrack() {
		BehaviorUtils.MakeVisible(GameObject.Find(RaceTrackName), false);
		foreach (string name in PlayerNames) {
			GameObject pug = GameObject.Find(name);
			pug.GetComponent<ControlCharacter>().Reset();
			BehaviorUtils.MakeVisible(pug, false);
		}
	}
	
	public void OnRoomConnected(bool success) {
		if (success) {
			mGameState = GameState.Playing;
			mMyParticipantId = GetSelf().ParticipantId;
			SetupTrack();
		} else {
			mGameState = GameState.SetupFailed;
		}
	}
	
	public void OnLeftRoom() {
		if (mGameState != GameState.Finished) {
			mGameState = GameState.Aborted;
		}
	}
	
	public void OnPeersConnected(string[] peers) {
	}
	
	public void OnPeersDisconnected(string[] peers) {
		foreach (string peer in peers) {
			// if this peer has left and hasn't finished the race,
			// consider them to have abandoned the race (0 score!)
			mGotFinalScore.Add(peer);
			mRacerScore[peer] = 0;
			RemoveCarFor(peer);
		}
		
		// if, as a result, we are the only player in the race, it's over
		List<Participant> players = GetRacers();
		if (mGameState == GameState.Playing && (players == null || players.Count < 2)) {
			mGameState = GameState.Aborted;
		}
	}
	
	private void RemoveCarFor(string participantId) {
		foreach (string name in PlayerNames) {
			GameObject obj = GameObject.Find(name);
			ControlCharacter cc = obj.GetComponent<ControlCharacter>();
			if (participantId.Equals(cc.ParticipantId)) {
				BehaviorUtils.MakeVisible(obj, false);
			}
		}
	}
	
	public void OnRoomSetupProgress(float percent) {
		mRoomSetupProgress = percent;
	}
	
	public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data) {
		/*int score = (int)data[1];
		
		if (data[0] == (byte)'I') {
			// interim score update
			mRacerScore[senderId] = score;
		} else if (data[0] == (byte)'F') {
			// finish notification
			if (!mGotFinalScore.Contains(senderId)) {
				// record final score
				mRacerScore[senderId] = score;
				mGotFinalScore.Add(senderId);
				UpdateMyRank();
				
				// finish race too, if we haven't yet
				if (mGameState == GameState.Playing) {
					FinishRace();
				}
			} else {
				Debug.LogWarning("Received duplicate finish notification for " + senderId);
			}
		}*/

		/*Vector3 pos = new Vector3 ();
		foreach (string name in PlayerNames) {
			pos = GameObject.Find (name).transform.position;
			
			//var obj = GameObject.Find (name).transform.position;
			Debug.Log (pos);
		}
	}
	
	public void CleanUp() {
		PlayGamesPlatform.Instance.RealTime.LeaveRoom();
		TearDownTrack();
		mGameState = GameState.Aborted;
		sInstance = null;
	}
	
	public Vector3 GetRacerProgress(string participantId) {
	//public Vector3 GetRacerProgress() {
		return GetRacerPosition ();
	}
	
	public Vector3 GetRacerPosition() {
		/*if (mRacerScore.ContainsKey(participantId)) {
			return mRacerScore[participantId];
		} else {
			return 0;
		}*/

		/*return new Vector3 ();
	}
	
	private Participant GetSelf() {
		return PlayGamesPlatform.Instance.RealTime.GetSelf();
	}
	
	private List<Participant> GetRacers() {
		return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
	}
	
	private Participant GetParticipant(string participantId) {
		return PlayGamesPlatform.Instance.RealTime.GetParticipant(participantId);
	}
	
	public void UpdateSelf(float deltaT, int pointsToAdd) {
		//int pos = GetRacerPosition(mMyParticipantId);
		Vector3 pos = new Vector3 ();
		foreach (string name in PlayerNames) {
		pos = GameObject.Find (name).transform.position;

			//var obj = GameObject.Find (name).transform.position;
			Debug.Log (pos);
		}
		
		/*if (pos >= PointsToFinish) {
			// already finished
			return;
		}*/
		
		//pos += pointsToAdd;
		//pos = pos < 0 ? 0 : pos >= PointsToFinish ? PointsToFinish : pos;
		//mRacerScore[mMyParticipantId] = pos;
		
		//if (pos >= PointsToFinish) {
			// we finished the race!
			//FinishRace();
		//} else if (pointsToAdd > 0) {
			// broadcast position update to peers
			/*BroadCastPosition(pos);
		//}
	}
	
	byte[] mPosPacket = new byte[3];
	private void BroadCastPosition(Vector3 pos) {

		mPosPacket[0] = (byte)pos.x; // interim update
		mPosPacket[1] = (byte)pos.y;
		mPosPacket[2] = (byte)pos.z;

		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(false, mPosPacket);
	}
	
	byte[] mFinalPacket = new byte[2];
	private void FinishRace() {
		mGotFinalScore.Add(mMyParticipantId);
		mGameState = GameState.Finished;
		UpdateMyRank();
		
		// send final score packet to peers
		mFinalPacket[0] = (byte)'F'; // final update
		mFinalPacket[1] = (byte)mRacerScore[mMyParticipantId];
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, mFinalPacket);
	}
	
	private void UpdateMyRank() {
		int numRacers = GetRacers().Count;
		if (mGotFinalScore.Count < numRacers) {
			mFinishRank = 0; // undefined for now
		}
		int myScore = mRacerScore[mMyParticipantId];
		int rank = 1;
		foreach (string participantId in mRacerScore.Keys) {
			if (mRacerScore[participantId] > myScore) {
				++rank;
			}
		}
		mFinishRank = rank;
	}
}
/*
using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections.Generic;
using System;

public class NetworkManager : RealTimeMultiplayerListener {
	const string RaceTrackName = "World";
	string[] PlayerNames = new string[] { "Player1", "Player2", "Player3", "Player4" };
	const int QuickGameOpponents = 1;
	const int GameVariant = 0;
	static NetworkManager sInstance = null;
	const int MinOpponents = 1;
	const int MaxOpponents = 3;
	private Score score;				// Reference to the Score script.
	
	// points required to finish race. Must be < 255 because it has to fit in a byte
	const int PointsToFinish = 100;
	
	public enum GameState { SettingUp, Playing, Finished, SetupFailed, Aborted };
	private GameState mGameState = GameState.SettingUp;
	
	// how many points each of our fellow racers has
	//score = GameObject.Find("Score").GetComponent<Score>();
	private Dictionary<string,int> mRacerScore = new Dictionary<string,int>();
	//private mRacerScore Time.time;
	
	// whether or not we received the final score for each participant id
	private HashSet<string> mGotFinalScore = new HashSet<string>();
	
	// my participant ID
	private string mMyParticipantId = "";
	
	// my rank (1st, 2nd, 3rd, 4th, or 0 to mean 'no rank yet')
	// This is updated every time we get a finish notification from a peer
	private int mFinishRank = 0;
	
	// room setup progress
	private float mRoomSetupProgress = 0.0f;
	
	// speed of the "fake progress" (to keep the player happy)
	// during room setup
	const float FakeProgressSpeed = 1.0f;
	const float MaxFakeProgress = 30.0f;
	float mRoomSetupStartTime = 0.0f;
	
	private NetworkManager() {
		mRoomSetupStartTime = Time.time;
	}
	
	public static void CreateQuickGame() {
		sInstance = new NetworkManager();
		PlayGamesPlatform.Instance.RealTime.CreateQuickGame(QuickGameOpponents, QuickGameOpponents,
		                                                    GameVariant, sInstance);
	}
	
	public static void CreateWithInvitationScreen() {
		sInstance = new NetworkManager();
		PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(MinOpponents, MaxOpponents,
		                                                               GameVariant, sInstance);
	}
	
	public static void AcceptFromInbox() {
		sInstance = new NetworkManager();
		PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(sInstance);
	}
	
	public static void AcceptInvitation(string invitationId) {
		sInstance = new NetworkManager();
		PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitationId, sInstance);
	}
	
	public GameState State {
		get {
			return mGameState;
		}
	}
	
	public static NetworkManager Instance {
		get {
			return sInstance;
		}
	}
	
	public int FinishRank {
		get {
			return mFinishRank;
		}
	}
	
	public float RoomSetupProgress {
		get {
			float fakeProgress = (Time.time - mRoomSetupStartTime) * FakeProgressSpeed;
			if (fakeProgress > MaxFakeProgress) {
				fakeProgress = MaxFakeProgress;
			}
			float progress = mRoomSetupProgress + fakeProgress;
			return progress < 99.0f ? progress : 99.0f;
		}
	}
	
	private void SetupTrack() {
		BehaviorUtils.MakeVisible(GameObject.Find(RaceTrackName), true);
		Debug.Log ("About to get self");
		Participant self = GetSelf();
		Debug.Log ("Self is " + self);
		Debug.Log ("About to get a list of connected participants");
		List<Participant> players = GetRacers();
		Debug.Log ("Players is  " + players + " with count of " + players.Count);
		int i;
		for (i = 0; i < PlayerNames.Length; i++) {
			Debug.Log("Looking at i value of " + i);
			GameObject pug = GameObject.Find(PlayerNames[i]);
			Debug.Log ("Looking for car name " + PlayerNames[i]);
			Participant player = i < players.Count ? players[i] : null;
			Debug.Log ("Player is " + player);
			
			bool isSelf = player != null && player.ParticipantId.Equals(self.ParticipantId);
			if (player != null) {
				Debug.Log("Racer is not null!");
				BehaviorUtils.MakeVisible(pug, true);
				ControlCharacter controller = pug.GetComponent<ControlCharacter>();
				controller.SetParticipantId(player.ParticipantId);
				controller.SetBlinking(isSelf);
			} else {
				Debug.Log("Hiding racer");
				BehaviorUtils.MakeVisible(pug, false);
			}
		}
	}
	
	private void TearDownTrack() {
		BehaviorUtils.MakeVisible(GameObject.Find(RaceTrackName), false);
		foreach (string name in PlayerNames) {
			GameObject pug = GameObject.Find(name);
			pug.GetComponent<ControlCharacter>().Reset();
			BehaviorUtils.MakeVisible(pug, false);
		}
	}
	
	public void OnRoomConnected(bool success) {
		if (success) {
			mGameState = GameState.Playing;
			mMyParticipantId = GetSelf().ParticipantId;
			SetupTrack();
		} else {
			mGameState = GameState.SetupFailed;
		}
	}
	
	public void OnLeftRoom() {
		if (mGameState != GameState.Finished) {
			mGameState = GameState.Aborted;
		}
	}
	
	public void OnPeersConnected(string[] peers) {
	}
	
	public void OnPeersDisconnected(string[] peers) {
		foreach (string peer in peers) {
			// if this peer has left and hasn't finished the race,
			// consider them to have abandoned the race (0 score!)
			mGotFinalScore.Add(peer);
			mRacerScore[peer] = 0;
			RemoveCarFor(peer);
		}
		
		// if, as a result, we are the only player in the race, it's over
		List<Participant> players = GetRacers();
		if (mGameState == GameState.Playing && (players == null || players.Count < 2)) {
			mGameState = GameState.Aborted;
		}
	}
	
	private void RemoveCarFor(string participantId) {
		foreach (string name in PlayerNames) {
			GameObject obj = GameObject.Find(name);
			ControlCharacter cc = obj.GetComponent<ControlCharacter>();
			if (participantId.Equals(cc.ParticipantId)) {
				BehaviorUtils.MakeVisible(obj, false);
			}
		}
	}
	
	public void OnRoomSetupProgress(float percent) {
		mRoomSetupProgress = percent;
	}
	
	public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data) {
		int score = (int)data[1];
		
		if (data[0] == (byte)'I') {
			// interim score update
			mRacerScore[senderId] = score;
		} else if (data[0] == (byte)'F') {
			// finish notification
			if (!mGotFinalScore.Contains(senderId)) {
				// record final score
				mRacerScore[senderId] = score;
				mGotFinalScore.Add(senderId);
				UpdateMyRank();
				
				// finish race too, if we haven't yet
				if (mGameState == GameState.Playing) {
					FinishRace();
				}
			} else {
				Debug.LogWarning("Received duplicate finish notification for " + senderId);
			}
		}
	}
	
	public void CleanUp() {
		PlayGamesPlatform.Instance.RealTime.LeaveRoom();
		TearDownTrack();
		mGameState = GameState.Aborted;
		sInstance = null;
	}
	
	public float GetRacerProgress(string participantId) {
		return GetRacerPosition(participantId) / (float)PointsToFinish;
	}
	
	public int GetRacerPosition(string participantId) {
		if (mRacerScore.ContainsKey(participantId)) {
			return mRacerScore[participantId];
		} else {
			return 0;
		}
	}
	
	private Participant GetSelf() {
		return PlayGamesPlatform.Instance.RealTime.GetSelf();
	}
	
	private List<Participant> GetRacers() {
		return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
	}
	
	private Participant GetParticipant(string participantId) {
		return PlayGamesPlatform.Instance.RealTime.GetParticipant(participantId);
	}
	
	public void UpdateSelf(float deltaT, int pointsToAdd) {
		int pos = GetRacerPosition(mMyParticipantId);

		foreach (string name in PlayerNames) {
			var obj = GameObject.Find (name).transform.position;
			Debug.Log (obj);
		}
		
		if (pos >= PointsToFinish) {
			// already finished
			return;
		}

		pos += pointsToAdd;
		pos = pos < 0 ? 0 : pos >= PointsToFinish ? PointsToFinish : pos;
		mRacerScore[mMyParticipantId] = pos;
		
		if (pos >= PointsToFinish) {
			// we finished the race!
			FinishRace();
		} else if (pointsToAdd > 0) {
			// broadcast position update to peers
			BroadCastPosition(pos);
		}
	}
	
	byte[] mPosPacket = new byte[2];
	private void BroadCastPosition(int pos) {
		mPosPacket[0] = (byte)'I'; // interim update
		mPosPacket[1] = (byte)pos;
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(false, mPosPacket);
	}
	
	byte[] mFinalPacket = new byte[2];
	private void FinishRace() {
		mGotFinalScore.Add(mMyParticipantId);
		mGameState = GameState.Finished;
		UpdateMyRank();
		
		// send final score packet to peers
		mFinalPacket[0] = (byte)'F'; // final update
		mFinalPacket[1] = (byte)mRacerScore[mMyParticipantId];
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, mFinalPacket);
	}
	
	private void UpdateMyRank() {
		int numRacers = GetRacers().Count;
		if (mGotFinalScore.Count < numRacers) {
			mFinishRank = 0; // undefined for now
		}
		int myScore = mRacerScore[mMyParticipantId];
		int rank = 1;
		foreach (string participantId in mRacerScore.Keys) {
			if (mRacerScore[participantId] > myScore) {
				++rank;
			}
		}
		mFinishRank = rank;
	}
}*/