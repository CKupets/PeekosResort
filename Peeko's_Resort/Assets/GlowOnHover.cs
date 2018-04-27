using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlowOnHover : MonoBehaviour {

	public Material newMat;

	public Material oldMat;

	private GameObject[] blocks;
	private GameObject[] balloon;

	// Use this for initialization
	void Start () {
		blocks = GameObject.FindGameObjectsWithTag ("ChangeModel");   
		balloon = GameObject.FindGameObjectsWithTag ("Respawn"); 
	}

	// Update is called once per frame
	void Update () {
		//This code below will change the color of the reticle if we are mousing over something
		if (RaycastHover2.getInstance ().anythingHitByRay ()) {

			GameObject objHitByRay = RaycastHover2.getInstance ().getObjectHitByRay ();
			string objHitTag = objHitByRay.tag;

			// Check that there was a valid object hit by the raycast. Raycaster.getInstance().getObjectHitByRay()
			// returns null if no objects were hit by ray cast on this frame. Objects responding to input must have
			// one of the increment/decrement/slideshow tags set to be affected and cycled by raycast.
			if (objHitByRay != null) {
				if (objHitTag == "ChangeModel") {
					changeColor(1);
				}
				else if (objHitTag == "Respawn") {
					balloon[0].GetComponent<Renderer> ().material = newMat;
				}
				else {
					changeColor(0);
					balloon[0].GetComponent<Renderer> ().material = oldMat;
				}
			} else {
				changeColor(0);
				balloon[0].GetComponent<Renderer> ().material = oldMat;
			}
		} else{
			changeColor(0);
			balloon[0].GetComponent<Renderer> ().material = oldMat;
		}
		if (Raycaster.getInstance ().anythingHitByRay ()) {

			GameObject objHitByRay = Raycaster.getInstance ().getObjectHitByRay ();
			string objHitTag = objHitByRay.tag;

			// Check that there was a valid object hit by the raycast. Raycaster.getInstance().getObjectHitByRay()
			// returns null if no objects were hit by ray cast on this frame. Objects responding to input must have
			// one of the increment/decrement/slideshow tags set to be affected and cycled by raycast.
			if (objHitByRay != null) {
				if (objHitTag == "ChangeModel") {
					SceneManager.LoadScene ("VRBlockStack");
				}
				if (objHitTag == "Respawn") {
					SceneManager.LoadScene ("halfway");
				}
			}
		}
	}

	void changeColor(int color){
		if(color == 1){
			for (int i = 0;i<4; i++) {
				blocks[i].GetComponent<Renderer> ().material = newMat;
			}
		}
		if(color == 0){
			for (int i = 0;i<4; i++) {
				blocks[i].GetComponent<Renderer> ().material = oldMat;
			}
		}
	}
}
