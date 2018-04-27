using UnityEngine;
using System.Collections;
using System.Net;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using System;

public class SocketClient : MonoBehaviour {

	/* This script has two purposes.
 * 1 - It should be launched on a pc that has a kinect.
 * 		a) 	- you need to assign 3 objects to the skeleton that will be tracked ( assigne the objects to the bones)
 * 			- for now I am tracking left hand, right hand and head
 *  	b) 	- the positions and rotations of the above objects are stored in a struct MyBody declared below
 * 		c) 	- MyBody structure has individual objects that can represent body parts ( another structure called MyBodyPart
 *		d)	- If we click connect and insert a correct IP, this instance will connect to another unity instance that runs
 *				on the phone.(you need to assign inputfield to the public variable below or hardcode the value
 *			- The connection is done via wifi/socket 8888 defined below ( feel free to change it )
 *		e)	- once we click connect, each X seconds a data is being send to the receiver.The data represents the locations/rotations
 *				of the tracked objects.
 *			- X is defined by the public variable sendInterval ( if the numbe is too low then every update the message
 *				will be send to the receiver and it will overload the receiver and create a big delay
 *			- To send the update 20 per second set the variable to 0.05, 60 times per second 0.033 atd
 */

	/*	Instructions
 * 	1	-	Put this script into a scene where is your kinect skeleton
 * 	2	-	Create 3 game objects and assign any three bones as a parent ( i have chosen head, left and right hand )
 *  3 	-	Assigne the 3 game objects to the below public variables head, leftHand rightHand
 * 	4	-	Create a input field that will contain the IP address and that will populate the object "join" in the method Connect
 *  5	-	Create a button that will call the method Connect in this script
 */

//	public GameObject head, leftHand, rightHand, leftElbow, rightElbow, leftKnee, rightKnee, leftFoot, rightFoot, leftShoulder, rightShoulder;
	public GameObject head, leftHand, rightHand, leftArm, rightArm, leftLeg, rightLeg;
	public GUIText networkText;
	public int hostId;
	private int myChan;
	private int socketId; // above Start()
	private int socketPort = 8888; // Also a class member variable
	private bool bsMessage = true;
	
	//public Text result; 
	public InputField inputField;
	
	public float sendInterval; //recommended value 0.033 ( 60 Frames per second)
	private float intervalCounter;
	private int countsBetweenSending=0;
	
	private Boolean jumped;

	int connectionId;
	// Use this for initialization
	void Start () {
		
		bufferSize = 1024;//4096; // 2048; //1024;
		initServer ();
		myBody = new MyBody (new MyBodyPart (Vector3.one, Quaternion.identity), new MyBodyPart (Vector3.one, Quaternion.identity), new MyBodyPart (Vector3.one, Quaternion.identity),
			new MyBodyPart (Vector3.one, Quaternion.identity), new MyBodyPart (Vector3.one, Quaternion.identity), new MyBodyPart (Vector3.one, Quaternion.identity), 
			new MyBodyPart (Vector3.one, Quaternion.identity));
//		myBody = new MyBody (new MyBodyPart (Vector3.one, Quaternion.identity), new MyBodyPart (Vector3.one, Quaternion.identity), new MyBodyPart (Vector3.one, Quaternion.identity));
		Connect ();
	}	

	/* Once the button connect is pressed on the PC
	 * a connection is created between this PC that has kinect and the other device
	 * if you hardcode the IP adresse in the variable "join" you don't need the input field
	 */
	public void Connect() {
		string join;
		join = inputField.text; //"192.168.1.11"
		//join = "192.168.1.13";	
		byte error;
		connectionId = NetworkTransport.Connect(socketId, join, socketPort, 0, out error);
		Debug.Log("Connected to server. ConnectionId: " + connectionId);
		networkText.text = "Connected to server. ConnectionId: " + connectionId;
		sendingData = true;
	}

	public void bsConnect() {
		bsbody = new bsMyBody (new MyBodyPart (Vector3.one, Quaternion.identity));
		bsMessage = true;
		Connect ();
	}

	public void kuConnect() {
		bsMessage = false;
		kskel = new kuSkeleton (new MyBodyPart (Vector3.one, Quaternion.identity), new MyBodyPart (Vector3.one, Quaternion.identity), new MyBodyPart (Vector3.one, Quaternion.identity),
			new MyBodyPart (Vector3.one, Quaternion.identity), new MyBodyPart (Vector3.one, Quaternion.identity), new MyBodyPart (Vector3.one, Quaternion.identity), 
			new MyBodyPart (Vector3.one, Quaternion.identity), new MyBodyPart (Vector3.one, Quaternion.identity), new MyBodyPart (Vector3.one, Quaternion.identity), new MyBodyPart (Vector3.one, Quaternion.identity), 
			new MyBodyPart (Vector3.one, Quaternion.identity), new MyBodyPart (Vector3.one, Quaternion.identity));
		Connect ();
	}
	
	//create the socket connection so we can send or receive data
	public void initServer()
	{
		NetworkTransport.Init ();
		ConnectionConfig config = new ConnectionConfig ();
		myChan = config.AddChannel (QosType.Unreliable);
		HostTopology topology = new HostTopology(config,10);
		socketId = NetworkTransport.AddHost(topology, socketPort);
		Debug.Log("Socket Open. SocketId is: " + socketId);
		networkText.text = "Socket Open. SocketId is: " + socketId;
		
	}
	
	private byte error;
	private byte[] buffer;
	
	private BinaryFormatter formatter = new BinaryFormatter();
	private int bufferSize=1024;
	private bool sendingData=false;

	//call this message to send the data
	public void SendSocketMessage() 
	{
		
		buffer = new byte[bufferSize];
		Stream stream = new MemoryStream (buffer);

		//here we calculate the data
		populateMyBody ();

		if (bsMessage) {
			formatter.Serialize (stream, bsbody);
			NetworkTransport.Send (hostId, connectionId, myChan, buffer, bufferSize, out error);
		} else {
			formatter.Serialize (stream, myBody);
			NetworkTransport.Send(hostId, connectionId, myChan, buffer, bufferSize, out error);
		}
		
	}

	//pc with kinect populates this varible that will be sent over the network
	public MyBody myBody;
	public bsMyBody bsbody;
	public kuSkeleton kskel;

	/*	If you want to expand the content of myBody you need to also update the method below
	 *  to populate the structure that will be sent
	 * */
	public void populateMyBody()
	{
		Debug.Log ("populateMyBody: " + jumped);
//		KinectManager km = KinectManager.Instance;
//		uint Player1ID = km.GetPlayer1ID ();
//		Boolean jumpbool = km.IsGestureComplete (Player1ID, KinectGestures.Gestures.Jump, true);
//		Debug.Log("Player1ID: " + Player1ID);
//		Debug.Log("Jump? " + km.IsGestureComplete (Player1ID, KinectGestures.Gestures.Jump, false));
//		myBody.jump = km.IsGestureComplete (Player1ID, KinectGestures.Gestures.Jump, true);
//		myBody.jump = true;
//		myBody.jump = jumped;
//		myBody.head.populate (head.transform.position, head.transform.rotation);
//		myBody.leftHand.populate (leftHand.transform.position, leftHand.transform.rotation);
//		myBody.rightHand.populate (rightHand.transform.position, rightHand.transform.rotation);
//		myBody.leftArm.populate (leftArm.transform.position, leftArm.transform.rotation);
//		myBody.rightArm.populate (rightArm.transform.position, rightArm.transform.rotation);
//		myBody.leftLeg.populate (leftLeg.transform.position, leftLeg.transform.rotation);
//		myBody.rightLeg.populate (rightLeg.transform.position, rightLeg.transform.rotation);
//		km.ResetPlayerGestures (Player1ID);

		if (bsMessage) {
			bsbody.jump = jumped;
			bsbody.head.populate (head.transform.position, head.transform.rotation);
		} else {
			// populate body for keep up

//			kskel.head.populate (head.transform.position, head.transform.rotation);
//			kskel.rightElbow.populate (rightElbow.transform.position, rightElbow.transform.rotation);
//			kskel.leftElbow.populate (leftElbow.transform.position, leftElbow.transform.rotation);
//			kskel.rightHand.populate (rightHand.transform.position, rightHand.transform.rotation);
//			kskel.leftHand.populate (leftHand.transform.position, leftHand.transform.rotation);
//			kskel.rightKnee.populate (rightKnee.transform.position, rightKnee.transform.rotation);
//			kskel.leftKnee.populate (leftKnee.transform.position, leftKnee.transform.rotation);
//			kskel.rightFoot.populate (rightFoot.transform.position, rightFoot.transform.rotation);
//			kskel.leftFoot.populate (leftFoot.transform.position, leftFoot.transform.rotation);
//			kskel.rightShoulder.populate (rightShoulder.transform.position, rightShoulder.transform.rotation);
//			kskel.leftShoulder.populate (leftShoulder.transform.position, leftShoulder.transform.rotation);
//			kskel.playerPos.populate (new Vector3 (0, 0, 0), Quaternion.identity);
			myBody.head.populate (head.transform.position, head.transform.rotation);
			myBody.leftHand.populate (leftHand.transform.position, leftHand.transform.rotation);
			myBody.rightHand.populate (rightHand.transform.position, rightHand.transform.rotation);
			myBody.leftArm.populate (leftArm.transform.position, leftArm.transform.rotation);
			myBody.rightArm.populate (rightArm.transform.position, rightArm.transform.rotation);
			myBody.leftLeg.populate (leftLeg.transform.position, leftLeg.transform.rotation);
			myBody.rightLeg.populate (rightLeg.transform.position, rightLeg.transform.rotation);
		}
	}

	private int recHostId;
	private int recConnectionId;
	private int recChannelId;
	private int dataSize;
	private NetworkEventType recNetworkEvent;
	
	void Update() {
		KinectManager km = KinectManager.Instance;
		uint Player1ID = km.GetPlayer1ID ();
		if (km.IsGestureComplete (Player1ID, KinectGestures.Gestures.Jump, true)) { // || km.IsGestureComplete (Player1ID, KinectGestures.Gestures.RaiseLeftHand, true)) {
			jumped = true; //= km.IsGestureComplete (Player1ID, KinectGestures.Gestures.Jump, false);
		}
		Debug.Log ("update jumped: " + jumped);
		if (sendingData) {
			intervalCounter = intervalCounter + Time.deltaTime;
			/* attempt for smoothing */
			if(intervalCounter < sendInterval)
			{
				if (bsMessage) {
					bsMyBody temp = bsbody;
					populateMyBody ();
					temp.add (bsbody);
					bsbody = temp;
				} else {
//					kuSkeleton temp = kskel;
//					populateMyBody ();
//					temp.add (kskel);
//					kskel = temp;
					MyBody temp = myBody;
					populateMyBody ();
					temp.add (myBody);
					myBody = temp;
				}
				countsBetweenSending++;
//				km.ResetPlayerGestures (Player1ID);
			}
			
			/*attempt for smoothing
			*/

			//data is being sent only when certain time has passed because if we send too much data(every frame)
			//it will overload the receiver and create a delay
			if(intervalCounter > sendInterval)
			{
				print (countsBetweenSending);
				if (bsMessage) {
					bsbody.divide (countsBetweenSending);
					bsbody.jump = jumped;
				} else {
					myBody.divide (countsBetweenSending);
				}
				countsBetweenSending = 0;
				if (connectionId > 0) {
					SendSocketMessage ();
					if (jumped)
						jumped = false;
//					km.ResetPlayerGestures (Player1ID);
				}
				intervalCounter = 0;
			}
//			km.ResetPlayerGestures (Player1ID);
		}
		handleSocketConnection ();
	}

	//this method deals with the socket connection
	//connection, deconnecion, recieving of data
	public void handleSocketConnection()
	{
		
		byte[] recBuffer = new byte[bufferSize];
		byte error;
		recNetworkEvent = NetworkTransport.Receive(out recHostId, out recConnectionId, out recChannelId, recBuffer, bufferSize, out dataSize, out error);
		switch (recNetworkEvent) {
		case NetworkEventType.Nothing:
			break;
		case NetworkEventType.ConnectEvent:
			Debug.Log ("incoming connection event received");
			networkText.text = "incoming connection event received";
			break;
		case NetworkEventType.DataEvent:
			Stream stream = new MemoryStream (recBuffer);
			BinaryFormatter formatter = new BinaryFormatter ();
			Debug.Log ("we should not receive any data");
			networkText.text = "we should not receive any data";
			break;
		case NetworkEventType.DisconnectEvent:
			Debug.Log ("remote client event disconnected");
			networkText.text = "remote client event disconnected";
			break;
		}
		
	}
}
