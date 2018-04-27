using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CreatePrefabs : NetworkBehaviour {
	public Transform prefab;
	void Start () {
		int i; 
		for (i = 0; i < 12; i++) {
			var radians = 30 * i * Mathf.Deg2Rad;
			var xpos = 6 * Mathf.Cos (radians);
			var ypos = 6 * Mathf.Sin (radians);
			Transform tmp = Instantiate (prefab, new Vector3 (xpos, 0.5f, ypos), Quaternion.identity);
			tmp.GetComponent<Renderer>().material.color = (i < 6) ? Color.blue : Color.red;
		}

		GameObject[] all;
		all = GameObject.FindGameObjectsWithTag ("Pick Up");
		GameObject single;
		int playerCount = 2; // hard-code 2 players 

		for (i = 0; i < 12; i++) {
			single = all [i];
			single.GetComponent<Renderer> ().material.color = (i < 6) ? Color.blue : Color.red;
		}
	}

	// Update is called once per frame
	void Update () {

	}

}

