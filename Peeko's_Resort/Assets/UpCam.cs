using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpCam : MonoBehaviour {

	public GameObject block;
	private float xpos;
	private float zpos;
	private AutoMove am;
	private GameObject topBlock;

	// Use this for initialization
	void Start () {
		xpos = this.transform.position.x;
		zpos = this.transform.position.z;
		am = block.GetComponent<AutoMove> ();
	}

	// Update is called once per frame

	void Update () {

		topBlock = GameObject.FindWithTag("focusBlock");
		transform.position = new Vector3 (topBlock.transform.position.x, transform.position.y, transform.position.z);

		KinectManager km = KinectManager.Instance;
		uint Player1ID = km.GetPlayer1ID ();
//		Debug.Log (block.transform.position);
		Debug.Log (block.transform.position.x);
//		this.transform.position = new Vector3 (block.transform.position.x, this.transform.position.y , 0);

		if (Input.GetKeyDown ("space") || km.IsGestureComplete(Player1ID, KinectGestures.Gestures.Jump, false)) {
			Debug.Log ("move up");
			this.transform.position += new Vector3 (0, 1, 0);
			km.ResetPlayerGestures (Player1ID);
		}
//		km.ResetPlayerGestures (Player1ID);
//		if (Player1ID != 0) {
//			Vector3 newpos = km.GetUserPosition (Player1ID);
//			this.transform.position = new Vector3 (xpos + newpos.x, this.transform.position.y, zpos - newpos.z);
//		}
//		else {
//			Vector3 ogPos = new Vector3 (xpos, this.transform.position.y, zpos);
//			this.transform.position = ogPos;
//		}
	}
}
