﻿using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
	public Vector3 offset;			// The offset at which the Health Bar follows the player.
	
	private Transform player;		// Reference to the player.


	void Awake ()
	{
		// Setting up the reference.

	}

	void Update ()
	{
		player = GameObject.Find("pug").transform;
		// Set the position to the player's position with the offset.
		transform.position = player.position + offset;
	}
}