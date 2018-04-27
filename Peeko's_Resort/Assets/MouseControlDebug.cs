using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControlDebug : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	float speed = 75.0f;

	// Update is called once per frame
	void Update () {

		Vector3 direction = new Vector3 (-Input.GetAxis ("Vertical"), Input.GetAxis ("Horizontal"), 0f);

		gameObject.transform.Rotate( direction * speed * Time.deltaTime); 


//		if(Input.GetKeyDown (KeyCode.UpArrow)){
//			gameObject.transform.rotation = new Quaternion(0,1,0) + gameObject.transform.rotation;
//			}
//		if(Input.GetKeyDown (KeyCode.UpArrow)){
//			gameObject.transform.rotation = new Quaternion(0,-1,0) + gameObject.transform.rotation;
//		}
	}
}
