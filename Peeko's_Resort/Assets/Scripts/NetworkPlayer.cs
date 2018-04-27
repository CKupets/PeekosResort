using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour {
	[SyncVar(hook = "OnPlayerIDChanged")] public string playerID;
	[SyncVar(hook = "OnColorChanged")] public int colorNum;
	Camera playerCam;
	Color myColor; 

	void Awake()
	{
		playerCam = GetComponentInChildren<Camera>();
		if (playerCam)
			playerCam.gameObject.SetActive(false);
	}

	[Command]
	void CmdSetPlayerID(string newID)
	{
		playerID = newID;
	}

	[Command]
	void CmdSetColor(GameObject player)
	{
		TeamManager.SetPlayerColor(gameObject);
		myColor = GetComponent<Renderer> ().material.color; 
	}

	public override void OnStartLocalPlayer ()
	{
		string myPlayerID = string.Format("Player {0}", netId.Value);
		CmdSetPlayerID(myPlayerID);
		//playerCam.gameObject.SetActive(true);
		CmdSetColor(gameObject);
	}

	public override void OnStartClient ()
	{
		OnPlayerIDChanged(playerID);
		OnColorChanged(colorNum);
	}

	void Update()
	{

	}

	void OnPlayerIDChanged(string newValue)
	{
		playerID = newValue;
	}

	public void OnColorChanged(int newColor)
	{
		colorNum = newColor;
		GetComponent<Renderer>().material.color =  colorNum==0 ? Color.red : Color.blue;
	}

}