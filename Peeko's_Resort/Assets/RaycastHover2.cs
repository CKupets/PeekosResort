using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHover2 : SingletonMonoBehaviour<RaycastHover2> {

	private GameObject mLastObjectHit = null;

	// Update is called once per frame
	void Update () {
		RaycastHit hit = new RaycastHit();
		Vector3 forward = Reticle.getInstance().transform.TransformDirection (Vector3.forward);

		// Create the ray that will be casted straight into the screen. It's origin is the coordinates
		// of the reticle after a click is detected. The destination is our forward vector.
		Ray castedRay = new Ray (Reticle.getInstance().transform.position, forward);

		// Check if the raycast hit anything, if so, acquire the game object that it hit using the 'out' struct.
		if(Physics.Raycast(castedRay, out hit)) {
			// Acquire the game object information from the object that is hit. This allows for anybody
			// to query getObjectHitByRay() each frame for access to that object. This will be further used to
			// cycle textures.
			if(hit.collider.gameObject != null) {
				mLastObjectHit = hit.collider.gameObject;
			}
		}
		// Reset the last object hit reference each frame that nothing was hit.
		else {
			mLastObjectHit = null;
		}
	}

	public bool anythingHitByRay() {
		return mLastObjectHit == null ? false : true;
	}

	// Returns the object hit by the raycast, NULL otherwise if nothing was hit this frame by click.
	public GameObject getObjectHitByRay() {
		return mLastObjectHit;
	}
}