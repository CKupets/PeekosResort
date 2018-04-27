/*SAMSUNG AND ITS AFFILIATES, SUBSIDIARIES, OFFICERS, DIRECTORS, EMPLOYEES, AGENTS, PARTNERS, AND LICENSORS (COLLECTIVELY, “SAMSUNG”) DO NOT PROMISE OR GUARANTEE THAT THE CODE CONTAINED HEREIN (INCLUDING, FUNCTIONALITY OR FEATURES OF THE FOREGOING) (COLLECTIVELY, THE “CODE”) WILL BE RELIABLE, SECURE, OR ERROR-FREE, OR THAT ANY DEFECTS WILL BE CORRECTED.  THE CODE IS PROVIDED ON AN “AS-IS” BASIS. SAMSUNG CANNOT ENSURE THAT THE CODE WILL BE FREE OF VIRUSES, CONTAMINATION OR DESTRUCTIVE FEATURES. FURTHER, SAMSUNG DOES NOT GUARANTEE ANY RESULTS OR IDENTIFICATION OR CORRECTION OF PROBLEMS AS PART OF THE CODE AND SAMSUNG DISCLAIMS ANY LIABILITY RELATED THERETO. SAMSUNG DISCLAIMS ALL WARRANTIES, EXPRESS OR IMPLIED, INCLUDING ANY WARRANTIES OF ACCURACY, NON-INFRINGEMENT, MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. SAMSUNG DISCLAIMS ANY AND ALL LIABILITY FOR THE ACTS, OMISSIONS AND CONDUCT OF ANY THIRD PARTIES IN CONNECTION WITH OR RELATED TO YOUR USE OF THE CODE. YOU ASSUME TOTAL RESPONSIBILITY AND ALL RISKS FOR YOUR USE OF THE CODE. YOUR SOLE REMEDY AGAINST SAMSUNG FOR DISSATISFACTION WITH THE CODE IS TO STOP USING THE CODE. SAMSUNG HAS NO OTHER OBLIGATION OR RESPONSIBILITY TO YOU.*/
using UnityEngine.UI;
using UnityEngine;
using System.Collections;


// ApplicationManager handles the overall application logic and responds to events. For this demo's sake,
// the application manager checks the Raycast singleton to see if any objects were hit by the raycast fired from
// our reticle. If so, it responds.
public class ApplicationManager : MonoBehaviour{

	// Public control that forces the button type (increment/decrement/slideshow) to set the
	// texture cycler mode. If set to false, textures will cycle according to how the texture
	// cycler is configured in the editor.
	public bool ButtonsDefineCyclerMode = true;

	public string mButtonTag = "Button";
	public string IncrementButtonTag = "IncrementButton";
	public string DecrementButtonTag = "DecrementButton";
	public string SlideshowButtonTag = "SlideshowButton";
	public string PickUp = "Pick Up";
	public string Button = "Button"; 
	public GameObject[] TextureObject = new GameObject[1];
	public Text countText;
	public Text winText;
	public GameObject respawnPrefab;
	public GameObject prespawnPrefab;
	private Rigidbody rb;
	private int count;
	public AudioSource audio;

	public GameObject initialObject;
	public GameObject swapObject;

//	private bool mFirstTextureReady = false;

	void Start() {
//		if(ScreenFaderSphere.getInstance()) {
//			ScreenFaderSphere.getInstance().setAlpha( 1.0f );
//		}
		rb = GetComponent<Rigidbody>();
		count = 0;
		SetCountText ();
		winText.text = "";
		countText.text = ""; 
	}

	void Update () {
		// Check if there were any objects hit by our reticle-ray cast in the scene. If so, check whether or not
		// it has a TextureCycler component.
		SetCountText ();
//		if(Raycaster.getInstance().anythingHitByRay()) {
//
//			GameObject objHitByRay = Raycaster.getInstance().getObjectHitByRay();
//			string objHitTag = objHitByRay.tag;
//			GameObject[] newSpawns;
//
//			// Check that there was a valid object hit by the raycast. Raycaster.getInstance().getObjectHitByRay() 
//			// returns null if no objects were hit by ray cast on this frame. Objects responding to input must have
//			// one of the increment/decrement/slideshow tags set to be affected and cycled by raycast.
//			if(objHitByRay != null && isTagValidButton(objHitTag)) {
//				if (mapTagToModeIndex(objHitTag)==3) {
//					if(TextureObject != null && (objHitByRay.GetComponent<Renderer> ().material.color == GetComponent<Renderer>().material.color)) {
//						objHitByRay.SetActive (false);
//						//print (GetComponent<Renderer> ().material.color); 
//						audio.Play (); 
//						count = count + 1;
//						SetCountText ();
//					}
//				} else if (mapTagToModeIndex(objHitTag)==4) {
//					
//					// change the existing prefabs
//					newSpawns = GameObject.FindGameObjectsWithTag(PickUp);
//					if (PickUp == "Pick Up") {
//						foreach (GameObject n in newSpawns) {
//							Instantiate(respawnPrefab, n.transform.position, n.transform.rotation);
//						}
//						PickUp = "Resp"; 
//					} else {
//						foreach (GameObject n in newSpawns) {
//							Instantiate(respawnPrefab, n.transform.position, n.transform.rotation);
//						}
//						PickUp = "Pick Up"; 
//					}
//				}
//			}
//		}
		 
//		if(ScreenFaderSphere.getInstance() && !mFirstTextureReady && TextureLoader.getInstance().isFinishedLoadingFirstTexture()) 
//		{
//			mFirstTextureReady = true;
//			ScreenFaderSphere.getInstance().fadeToTransparent();
//		}
	}

	private int mapTagToModeIndex(string tagToMap) {
		if(tagToMap == IncrementButtonTag) return 0;
		if(tagToMap == DecrementButtonTag) return 1;
		if(tagToMap == SlideshowButtonTag) return 2;
		if(tagToMap == PickUp) return 3;
		if (tagToMap == Button)
			return 4; 
		// Default to IncrementTag if tagToMap isn't valid button tag.
		return 0;
	}

	private bool isTagValidButton(string tagToCheck) {
		if(tagToCheck == IncrementButtonTag || tagToCheck == PickUp || tagToCheck == DecrementButtonTag || tagToCheck == SlideshowButtonTag) {
			return true;
		}

		return false;
	}

//	public void fadeToBlack() {
//		ScreenFaderSphere.getInstance().fadeToBlack();
//	}
//
//	public void fadeToTransparent() {
//		ScreenFaderSphere.getInstance().fadeToTransparent();
//	}
//
	void SetCountText ()
	{
		//print (GameObject.Find ("Player").transform.position.y); 
		if (GameObject.Find("Player").transform.position.y <=.5)
		{
			winText.text = "You LOSE!";
			if (Input.GetKeyDown (KeyCode.Mouse0) || Input.GetKeyDown (KeyCode.JoystickButton2) || Input.GetKeyDown (KeyCode.JoystickButton0)) {
				Application.LoadLevel (0);
			}
		}
	}
}
