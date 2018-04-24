using UnityEngine;
using System.Collections;
using System;

public class AlexSimpleGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface {

	public GUIText GestureInfo;
	private bool progressDisplayed;

	public void UserDetected(uint userId, int userIndex) {
		KinectManager manager = KinectManager.Instance;

		manager.DetectGesture (userId, KinectGestures.Gestures.Jump);

		if (GestureInfo != null) {
			GestureInfo.GetComponent<GUIText> ().text = "Jump!";
		}
	}

	public void UserLost(uint userId, int userIndex)
	{
		if(GestureInfo != null)
		{
			GestureInfo.GetComponent<GUIText>().text = string.Empty;
		}
	}

	public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, 
		float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos) {
	}

	public bool GestureCompleted (uint userId, int userIndex, KinectGestures.Gestures gesture, 
		KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos) {
		string sGestureText = gesture + " detected";
		Debug.Log (sGestureText);

		if (GestureInfo != null) {
			GestureInfo.GetComponent<GUIText> ().text = sGestureText;
		}

		return true;
	}

	public bool GestureCancelled(uint userId, int userIndex, KinectGestures.Gestures gesture, 
		KinectWrapper.NuiSkeletonPositionIndex joint) {
		return true;
	}
}
