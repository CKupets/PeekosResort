using UnityEngine;
using System.Collections;
using System.Net;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using System;

/* This script has one purpose.
 *	1 - It should be launched on an android phone ( or any other second device)
 *		a)  - the phone creates a socket connection on a port 8888 and it listens for a datastream
 *		b)	- once it receives data it deserializes the data into MyBody structure that contains position and rotation of
 *				left hand, right hand and head.
 *		c)	- you need to assign 3 game objects to the variables  (headRec, leftHandRec, rightHandRec)
 *		d)	- if everything goes as it should the original 3 objects tracked by kinect in the script SocketClient
 *				should replicate the movement into the unity instance that is running on your phone 
 *				(maybe with GearVR or the cardboard)
 *
 * */

/*	Instructions
 * 	1	-	Put this script into a scene
 * 	2	-	Assign 3 objects to the headRec, leftHandRec and rightHandRec - these objects will replicate the movement
 * 			of the 3 objects tracked by kinect
 * 	3	-	Change the socket number if needed
 * 	4	-	ATTENTION!!! i take absolute position, you can do relative and do gameObject.transform.position+tracked position
 * */

public class KUSocketServer :MonoBehaviour{
		
	public GameObject headRec, leftHandRec, rightHandRec, leftArm, rightArm, leftLeg, rightLeg;
	public GameObject ball;
//	public GameObject jumpText;
	private Text jtext;
	public int hostId;
	private int myChan;
	private int socketId; // above Start()
	private int socketPort = 8888; // Also a class member variable

	private Boolean jump;
	private Boolean camJump;
	private Boolean blockJump;
	private uint jumpCount;
	private Boolean gameStarted;
		private bool connected;

	int connectionId;
		// Use this for initialization
	void Start () {
		jumpCount = 0;
//		jumpText.GetComponent<TextMesh> ().text = "Jumps: " + jumpCount;
//		jumpText.GetComponent<Text>().text = "Jumps: " + jumpCount;
		bufferSize = 1024;
		gameStarted = false;
		connected = false;
		initServer ();
	}	

	void gameStart() {
		bool waiting = true;

		while (waiting) {
			if (Input.GetKeyDown (KeyCode.Mouse0) || Input.GetKeyDown (KeyCode.JoystickButton2) || Input.GetKeyDown (KeyCode.JoystickButton0)) {
				waiting = false;
			}
		}
			ball.GetComponent<Rigidbody> ().useGravity = true;

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
	
	}

	public Boolean jumpCheck() {
		return jump;
	}

	public Boolean camJumpCheck() {
		if (camJump) {
			camJump = false;
			return true;
		}
		return false;
	}

	public Boolean blockJumpCheck() {
		if (blockJump) {
			blockJump = false;
			return true;
		}
		return false;
	}

	private byte error;
	private byte[] buffer;

	private BinaryFormatter formatter = new BinaryFormatter();
	private int bufferSize=1024;

	private int recHostId;
	private int recConnectionId;
	private int recChannelId;
	private int dataSize;
	private NetworkEventType recNetworkEvent;

	private MyBody receivedBody;

	void Update() {
		if (connected) {
			if (Input.GetKeyDown (KeyCode.Mouse0) || Input.GetKeyDown (KeyCode.JoystickButton2) || Input.GetKeyDown (KeyCode.JoystickButton0)) {
				ball.GetComponent<Rigidbody> ().useGravity = true;
			}
		}
		handleSocketConnection ();
	}

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
			connected = true;
				break;
		case NetworkEventType.DataEvent:
			connected = true;
				Stream stream = new MemoryStream (recBuffer);
				BinaryFormatter formatter = new BinaryFormatter ();
				
				receivedBody = (MyBody) formatter.Deserialize(stream);
				
				handleReceivedData();

				break;
		case NetworkEventType.DisconnectEvent:
			Debug.Log ("remote client event disconnected");
			connected = false;
			ball.GetComponent<Rigidbody> ().useGravity = false;
				break;
			}

	}

	//here we deal with the data we have received and we assign the coordinance
	//i ignore the rotation of the head because that is bing controlled by cardboard/gearVR
	public void handleReceivedData()
	{
		headRec.transform.position = receivedBody.head;
		leftHandRec.transform.position = receivedBody.leftHand;
		leftHandRec.transform.rotation = receivedBody.leftHand;
		rightHandRec.transform.position = receivedBody.rightHand;
		rightHandRec.transform.rotation = receivedBody.rightHand;

		leftArm.transform.position = receivedBody.leftArm;
		leftArm.transform.rotation = receivedBody.leftArm;
		rightArm.transform.position = receivedBody.rightArm;
		rightArm.transform.rotation = receivedBody.rightArm;
		leftLeg.transform.position = receivedBody.leftLeg;
		leftLeg.transform.rotation = receivedBody.leftLeg;
		rightLeg.transform.position = receivedBody.rightLeg;
		rightLeg.transform.rotation = receivedBody.rightLeg;
//		camJump = jump;
//		blockJump = jump;
//		
//		if (jump) {
//			jumpCount += 1;
////			jumpText.GetComponent<TextMesh>().text = "Jumps: " + jumpCount;
//		}
	}
}
	
[System.Serializable]
public struct kuSkeleton
{
	public MyBodyPart head, leftHand, rightHand, leftElbow, rightElbow, leftKnee, rightKnee, leftFoot, rightFoot, leftShoulder, rightShoulder, playerPos;

	public kuSkeleton(MyBodyPart khead, MyBodyPart kleftHand, MyBodyPart krightHand, 
		MyBodyPart kleftElbow, MyBodyPart krightElbow, MyBodyPart kleftKnee, MyBodyPart krightKnee, 
		MyBodyPart kleftFoot, MyBodyPart krightFoot, MyBodyPart kleftShoulder, MyBodyPart krightShoulder, MyBodyPart kplayerPos) {
	
		head = khead;
		leftHand = kleftHand;
		rightHand = krightHand;
		leftElbow = kleftElbow;
		rightElbow = krightElbow;
		leftKnee = kleftKnee;
		rightKnee = krightKnee;
		leftFoot = kleftFoot;
		rightFoot = krightFoot;
		leftShoulder = kleftShoulder;
		rightShoulder = krightShoulder;
		playerPos = kplayerPos;
	}

	public void add(kuSkeleton m)
	{
		head.add (m.head);
		leftHand.add (m.leftHand);
		rightHand.add (m.rightHand);
		leftElbow.add (m.leftElbow);
		rightElbow.add (m.rightElbow);
		leftKnee.add (m.leftKnee);
		rightKnee.add (m.rightKnee);
		leftFoot.add (m.leftFoot);
		rightFoot.add (m.rightFoot);
		leftShoulder.add (m.leftShoulder);
		rightShoulder.add (m.rightShoulder);
		playerPos.add (m.playerPos);
	}

	public void divide(int i)
	{
		head.divide (i);
		leftHand.divide (i);
		rightHand.divide (i);
		leftElbow.divide (i);
		rightElbow.divide (i);
		leftKnee.divide (i);
		rightKnee.divide (i);
		leftFoot.divide (i);
		rightFoot.divide (i);
		leftShoulder.divide (i);
		rightShoulder.divide(i);
		playerPos.divide(i);
	}
}

//[System.Serializable]
//public struct MyBody
//{
//	public MyBodyPart head, leftHand, rightHand, leftArm, rightArm, leftLeg, rightLeg;
//	public Boolean jump;
//
//	public MyBody(MyBodyPart p_head,MyBodyPart p_left, MyBodyPart p_right, MyBodyPart p_leftArm, MyBodyPart p_rightArm, MyBodyPart p_leftLeg, MyBodyPart p_rightLeg)
//	{
//		head = p_head;
//		leftHand = p_left;
//		rightHand = p_right;
//		leftArm = p_leftArm;
//		rightArm = p_rightArm;
//		leftLeg = p_leftLeg;
//		rightLeg = p_rightLeg;
//
//		jump = false;
//	}
//	public void add(MyBody m)
//	{
//		head.add (m.head);
//		leftHand.add (m.leftHand);
//		rightHand.add(m.rightHand);
//
//	}
//
//	public void divide(int i)
//	{
//		head.divide (i);
//		leftHand.divide (i);
//		rightHand.divide (i);
//	}
//}

//[System.Serializable]
//public struct MyBodyPart
//{
//	public float x,y,z;
//	public float rx, ry, rz, rw;
//
//	public void add(MyBodyPart m)
//	{	
//		x  += m.x;
//		y += m.y;
//		z += m.z;
//		rx += m.rx;
//		ry += m.ry;
//		rz += m.rz;
//		rw += m.rw;
//	}
//
//	public void divide(int i)
//	{
//		x = x / i;
//		y = y / i;
//		z = z / i;
//		rx = rx / i;
//		ry = ry / i;
//		rz = rz / i;
//		rw = rw / i;
//	}
//
//
//	public MyBodyPart(float rX, float rY, float rZ, float rrx, float rry, float rrz, float rrw)
//	{
//		x = rX;
//		y = rY;
//		z = rZ;
//		rx = rrx;
//		ry = rry;
//		rz = rrz;
//		rw = rrw;
//
//	}
//
//	public MyBodyPart(Vector3 rValue, Quaternion rQuat)
//	{
//		x = rValue.x;
//		y = rValue.y;
//		z = rValue.z;
//		rx = rQuat.x;
//		ry = rQuat.y;
//		rz = rQuat.z;
//		rw = rQuat.w;
//	}
//
//	public void populate(Vector3 rValue, Quaternion rQuat)
//	{
//		x = rValue.x;
//		y = rValue.y;
//		z = rValue.z;
//		rx = rQuat.x;
//		ry = rQuat.y;
//		rz = rQuat.z;
//		rw = rQuat.w;
//	}
//
//	public override string ToString()
//	{
//		return String.Format("[{0}, {1}, {2}:{3}, {4}, {5}]", x, y, z, rx, ry, rz);
//	}
//
//	public static implicit operator Quaternion(MyBodyPart rValue)
//	{
//		return new Quaternion(rValue.rx, rValue.ry, rValue.rz, rValue.rw);
//	}
//
//	public static implicit operator Vector3(MyBodyPart rValue)
//	{
//		return new Vector3(rValue.x, rValue.y, rValue.z);
//	}
//}

	