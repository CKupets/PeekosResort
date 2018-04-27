using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : MonoBehaviour {

	public float speed;
	public Text countText;
	public Text winText;

	private Rigidbody rb;
	private int count;
	public AudioSource audio;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		count = 0;
		SetCountText ();
		winText.text = "";
	}

//	public override void OnStartLocalPlayer()
//	{
//		GetComponent<Renderer>().material.color = Color.blue;
//	}

	void FixedUpdate ()
	{	
//		if (isLocalPlayer == false)
//			return; 
//		float moveHorizontal = Input.GetAxis ("Horizontal");
//		float moveVertical = Input.GetAxis ("Vertical");
//		transform.Rotate(0, moveHorizontal, 0);
//		transform.Translate(0, 0, moveVertical);
//
//		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
//
//		rb.AddForce (movement * speed);
	}

	void OnTriggerEnter(Collider c) 
	{
//		if (other.gameObject.CompareTag ( "Pick Up"))
//		{
//			other.gameObject.SetActive (false);
//			audio.Play (); 
//			count = count + 1;
//			SetCountText ();
//		}

//		float force = 10;



	}

	void OnCollisionEnter(Collision c)
	{
		// force is how forcefully we will push the player away from the enemy.
		// If the object we hit is the enemy
		if (c.gameObject.tag == "Reticle")
		{
			print ("balloon was hit");
			// Calculate Angle Between the collision point and the player
			//			Vector3 dir = c.transform.position - transform.position;
			//			// We then get the opposite (-Vector3) and normalize it
			// 			dir = -dir;
			//			// And finally we add force in the direction of dir and multiply it by force. 
			//			// This will push back the player
			//			GetComponent<Rigidbody>().AddForce(dir*force);
			//
			Vector3 forceVec = c.gameObject.GetComponent<Rigidbody>().velocity.normalized * 50f;
			forceVec.Normalize ();
			count += 1; 
			SetCountText (); 


			Vector3 force = (transform.position - c.transform.position) * 70.0f;
			// normalize force vector to get direction only and trim magnitude

			print (force); 

			force = new Vector3 (0, 10000f, 0);

			//	gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
			//gameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Acceleration);
			gameObject.GetComponent<Rigidbody>().AddForce(100* force);
		}

	}

	void SetCountText ()
	{
		countText.text = "Count: " + count.ToString ();

		if (GameObject.Find("Player").transform.position.y <=.5)
		{
			//print ("lost"); 
			winText.text = "You LOSE!";
		}
	}
}