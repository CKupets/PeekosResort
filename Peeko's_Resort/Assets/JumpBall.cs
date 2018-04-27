using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBall : MonoBehaviour {

	private BlockListener blockListener;
	private GestureListener gestureListener;
	private uint count;


	// Use this for initialization
	void Start () {
		blockListener = Camera.main.GetComponent<BlockListener> ();
		gestureListener = Camera.main.GetComponent<GestureListener> ();
		Debug.Log ("JumpBall - Start");
		count = 0;
	}
	
	// Update is called once per frame
	void Update () {
		KinectManager kinectManager = KinectManager.Instance;
//		Debug.Log (count);
		uint Player1ID = kinectManager.GetPlayer1ID();
		if (Player1ID != 0) {
//			Debug.Log ("Player 1 detected");
			if (kinectManager.IsGestureComplete (Player1ID, KinectGestures.Gestures.Jump, true)) {
				count = count + 1;
				Debug.Log ("JUMP FOUND!!!");
			}
		}

//		if (gestureListener) {
//			Debug.Log ("GL exists");
//			Debug.Log (gestureListener.IsSwipeLeft ());
//			if (gestureListener.IsSwipeLeft ()) {
//				count = count + 1;
//			} else if (gestureListener.IsSwipeRight ()) {
//				count = count + 1;
//			}
//		}
//
//		if (!kinectManager) {
//			Debug.Log ("No kinect manager");
//		}

//		if (blockListener) {
//			if (blockListener.isJump ()) {
//				
//				count += 1;
////				Debug.Log ("JumpBall - blockListener exists");
//
//			}
//		}


	}

}
