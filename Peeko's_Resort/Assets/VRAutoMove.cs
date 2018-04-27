using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAutoMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.tag = "focusBlock";
		rend = this.GetComponent<MeshRenderer> ();
		scoreText.GetComponent<GUIText> ().text = "Score: " + score;
		gameover = false;
	}

	// Update is called once per frame
	public float rightLimit = 4.0f;
	public float leftLimit = -4.0f;
//	public float speed = 1.0f; //2.0f;
	static int direction = 1;

	private float speed = 1.0f;
	private bool active = true;
	private bool collisionBool = false;
	private uint count;
	private Renderer rend;
	private uint score = 0;
	private bool gameover;

	public GameObject newOne;
	public GameObject camera;
	public Material normal;
	public Material focus;
	public GUIText scoreText;
	public GameObject server;
	public GameObject lText;

	public void setScore(uint s) {
		score = s;
	}
	
	// Update is called once per frame
	void Update () {

		if (gameover) {
			if (Input.GetKeyDown (KeyCode.Mouse0) || Input.GetKeyDown (KeyCode.JoystickButton2) || Input.GetKeyDown (KeyCode.JoystickButton0)) {
				Application.LoadLevel (2);
			}
		}


		if (this.tag == "focusBlock") {
			//			Debug.Log ("focusBlock");
			print("Score " + score);
			rend.material = focus;
		} else {
			rend.material = normal;
		}

		if (active) {
			if (transform.position.x > rightLimit) {
				direction = -1;
			} else if (transform.position.x < leftLimit) {
				direction = 1;
			}
			var movement = Vector3.right * direction * speed * Time.deltaTime; 
			transform.Translate (movement);
			//if (server.GetComponent<SocketServer>().jumpCheck()) {
			if (server.GetComponent<SocketServer>().blockJumpCheck() || Input.GetKeyDown (KeyCode.Mouse0) || Input.GetKeyDown (KeyCode.JoystickButton2)|| Input.GetKeyDown (KeyCode.JoystickButton0)) {
//			if(Input.GetKeyDown (KeyCode.Mouse0) || Input.GetKeyDown (KeyCode.JoystickButton2)|| Input.GetKeyDown (KeyCode.JoystickButton0)) {
				if (Physics.CheckBox (this.transform.position - new Vector3 (0, 1f, 0),
					    new Vector3 ((gameObject.transform.localScale.x) / 2f, .1f, .5f), Quaternion.identity)) {

					Collider[] these = Physics.OverlapBox (this.transform.position - new Vector3 (0, 1f, 0),
						                   new Vector3 ((gameObject.transform.localScale.x) / 2f, .1f, .5f), Quaternion.identity);

					float rightEdgeOver = (gameObject.transform.position.x + (gameObject.transform.localScale.x) / 2f) -
					                      (these [0].transform.position.x + (these [0].transform.localScale.x) / 2f);
					float leftEdgeOver = (gameObject.transform.position.x - (gameObject.transform.localScale.x) / 2f) -
					                     (these [0].transform.position.x - (these [0].transform.localScale.x) / 2f);
					//Debug.Log (rightEdgeOver);
					//float diff = these [0].transform.position.x - gameObject.transform.position.x;

					if (rightEdgeOver > 0) {
						gameObject.transform.localScale -= new Vector3 (Mathf.Abs (rightEdgeOver), 0, 0);
						gameObject.transform.position -= new Vector3 (Mathf.Abs (rightEdgeOver) / 2f, 0, 0);
					} else {
						gameObject.transform.localScale -= new Vector3 (Mathf.Abs (leftEdgeOver), 0, 0);
						gameObject.transform.position += new Vector3 (Mathf.Abs (leftEdgeOver) / 2f, 0, 0);
					}
//					scoreText.GetComponent<GUIText> ().text = "";
					GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
					cube.AddComponent<VRAutoMove> ();
					cube.transform.localScale = gameObject.transform.localScale;
					cube.transform.position = this.transform.position + new Vector3 (0, 1, 0);
					// set properties of AutoMove script in new b
					cube.GetComponent<VRAutoMove> ().normal = normal;
					cube.GetComponent<VRAutoMove> ().focus = focus;
					cube.GetComponent<VRAutoMove> ().setScore (score + 1);
					cube.GetComponent<VRAutoMove> ().scoreText = scoreText;
					cube.GetComponent<VRAutoMove> ().server = server;
					cube.GetComponent<VRAutoMove> ().lText = lText;
					if ((score + 1) % 5 == 0) {
						cube.GetComponent<VRAutoMove> ().speed = speed + 0.5f;
					} else {
						cube.GetComponent<VRAutoMove> ().speed = speed;
					}
					this.tag = "Untagged";

				} else {
					lText.GetComponent<TextMesh> ().text = "GAME OVER\nScore: " + score + "\n" + "Click the side of the\nGearVR to play again!";
					gameover = true;
				}

				active = false;
			}
		}
	}
}
