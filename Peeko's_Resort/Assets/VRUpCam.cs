using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRUpCam : MonoBehaviour {

	public GameObject block;
	public GameObject server;
	public GameObject lText;
	private float xpos;
	private float zpos;
//	private AutoMove am;
	private GameObject topBlock; 

	// Use this for initialization
	void Start () {
		xpos = this.transform.position.x;
		zpos = this.transform.position.z;
//		am = block.GetComponent<AutoMove> ();

	}
	
	// Update is called once per frame
	void Update () {
		topBlock = GameObject.FindWithTag("focusBlock");
		transform.position = new Vector3 (topBlock.transform.position.x, transform.position.y, transform.position.z);

		//		Debug.Log (block.transform.position);
		Debug.Log (block.transform.position.x);
		//		this.transform.position = new Vector3 (block.transform.position.x, this.transform.position.y , 0);

//		if (Input.GetKeyDown ("space") || km.IsGestureComplete(Player1ID, KinectGestures.Gestures.Jump, false)) {
		//if (server.GetComponent<SocketServer>().jumpCheck()) {
		if (server.GetComponent<SocketServer>().camJumpCheck() || Input.GetKeyDown (KeyCode.Mouse0) || Input.GetKeyDown (KeyCode.JoystickButton2)|| Input.GetKeyDown (KeyCode.JoystickButton0)) {
		//if(Input.GetKeyDown (KeyCode.Mouse0) || Input.GetKeyDown (KeyCode.JoystickButton2)|| Input.GetKeyDown (KeyCode.JoystickButton0)) {
			Debug.Log ("move up");
			this.transform.position += new Vector3 (0, 1, 0);
			lText.transform.position += new Vector3 (0, 1, 0);
		}
	}
}
