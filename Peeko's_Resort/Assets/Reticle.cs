using UnityEngine;
using System.Collections;


public class Reticle : SingletonMonoBehaviour<Reticle> {

	private bool isLerping          = false;
	private float mAnimStartTime    = 0.0f;
	private float mAnimEndTime      = 0.0f;
	private float mAnimHalfTime     = 0.0f;

	private InputManager mInputManager = null;

	public Color PressedColor       = new Color (1f, 1f, 1f, 1f);
	public Color StartColor         = new Color (1f, 1f, 1f, 0.5f);

	public float ClickAnimationDuration = 0.5f;

	// Use this for initialization
	void Start () {
		mInputManager = InputManager.getInstance();
		SpriteRenderer reticleSpriteRenderer = GetComponent<SpriteRenderer>();

		if(reticleSpriteRenderer != null) {
			StartColor = reticleSpriteRenderer.color;
		}
	}

	// Update is called once per frame
	void Update () {
		// While the user has clicked we signal the flag to lerp the reticle color.
		if(mInputManager != null) {
			if (mInputManager.IsPressed == true) {
				mAnimStartTime = Time.unscaledTime;
				mAnimHalfTime = mAnimStartTime + (ClickAnimationDuration/2);
				mAnimEndTime = Time.unscaledTime + ClickAnimationDuration;

				// Reset the color on clicks before lerping.
				GetComponent<SpriteRenderer> ().color = StartColor;
				isLerping = true;
			}
		}

		// True if the animation hasn't reached the end yet.
		if(Time.unscaledTime < mAnimEndTime) {
			lerpReticleColor();
		}
		else if(Time.unscaledTime > mAnimEndTime && GetComponent<SpriteRenderer> ().color == PressedColor) {
			isLerping = false;
		}
		else if(Time.unscaledTime > mAnimEndTime && GetComponent<SpriteRenderer> ().color != PressedColor && isLerping) {
			isLerping = false;
			GetComponent<SpriteRenderer> ().color = StartColor;
		}
	}

	private void lerpReticleColor() {
		// % animation has progress since start [0,1] (depending on first or second half of animation)
		float animProgress = Time.unscaledTime - (Time.unscaledTime <= mAnimHalfTime ? mAnimStartTime : mAnimHalfTime);

		// Total duration in real-time of the animation (depending on if the animation is in first half or second).
		float halfAnimDuration = ClickAnimationDuration / 2.0f;

		// The first half of the reticle color animation will go from idleColor -> pressedColor
		if(Time.unscaledTime <= mAnimHalfTime) {
			// Lerp the color to the appropriate amount.
			GetComponent<SpriteRenderer> ().color = Color.Lerp(StartColor, PressedColor, animProgress / halfAnimDuration);
		}
		// The second half of the reticle color animation will go back from PressedColor -> idleColor
		else {
			GetComponent<SpriteRenderer> ().color = Color.Lerp(
				GetComponent<SpriteRenderer> ().color,
				StartColor,
				animProgress / halfAnimDuration);
		}
	}
}





