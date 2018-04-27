using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour {



	// Use this for initialization
	void Start () {
		this.tag = "focusBlock";
		rend = this.GetComponent<MeshRenderer> ();
		scoreText.GetComponent<GUIText> ().text = "Score: " + score;
	}
	
	// Update is called once per frame
	public float rightLimit = 4.0f;
	public float leftLimit = -4.0f;
	public float speed = 2.0f;
	static int direction = 1;

	private bool active = true;
	private bool collisionBool = false;
	private uint count;
	private Renderer rend;
	private uint score = 0;

	public GameObject newOne;
	public GameObject camera;
	public Material normal;
	public Material focus;
	public GUIText scoreText;

	public void setScore(uint s) {
		score = s;
	}

	void Update () {

		if (this.tag == "focusBlock") {
//			Debug.Log ("focusBlock");
			print("Score " + score);
			rend.material = focus;
		} else {
			rend.material = normal;
		}

		KinectManager km = KinectManager.Instance;
		uint Player1ID = km.GetPlayer1ID ();
		if (active) {
			if (transform.position.x > rightLimit) {
				direction = -1;
			} else if (transform.position.x < leftLimit) {
				direction = 1;
			}
			var movement = Vector3.right * direction * speed * Time.deltaTime; 
			transform.Translate (movement);

			if (Input.GetKeyDown ("space") || (Player1ID != 0 && km.IsGestureComplete (Player1ID, KinectGestures.Gestures.Jump, false))) {
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
					scoreText.GetComponent<GUIText> ().text = "";
					GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
					cube.AddComponent<AutoMove> ();
					cube.transform.localScale = gameObject.transform.localScale;
					cube.transform.position = this.transform.position + new Vector3 (0, 1, 0);
					// set properties of AutoMove script in new b
					cube.GetComponent<AutoMove> ().normal = normal;
					cube.GetComponent<AutoMove> ().focus = focus;
					cube.GetComponent<AutoMove> ().setScore (score + 1);
					cube.GetComponent<AutoMove> ().scoreText = scoreText;
					this.tag = "Untagged";
				}

				active = false;
			}
		}
	}

}
