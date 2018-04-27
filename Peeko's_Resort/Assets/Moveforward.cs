using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveforward : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Mouse0)) {
			transform.position = transform.position + Camera.main.transform.forward * 50 * Time.deltaTime;
		}
	}
}
