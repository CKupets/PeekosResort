using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TeamManager : NetworkBehaviour {
	public static TeamManager instance;
	static int playerCount;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			DestroyImmediate(gameObject);
			return;
		}
	}

	[Server]
	public static void SetPlayerColor(GameObject newPlayer)
	{
		var player = newPlayer.GetComponent<NetworkPlayer>();
		player.colorNum = (int)Mathf.Repeat(playerCount, 2);
		playerCount++;
	}


}