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

public class SocketServer :MonoBehaviour{
		
	public GameObject headRec, leftHandRec, rightHandRec, leftArm, rightArm, leftLeg, rightLeg;
	public GameObject jumpText;
	private Text jtext;
	public int hostId;
	private int myChan;
	private int socketId; // above Start()
	private int socketPort = 8888; // Also a class member variable

	private Boolean jump;
	private Boolean camJump;
	private Boolean blockJump;
	private uint jumpCount;
	private uint msgCount;

	int connectionId;
		// Use this for initialization
	void Start () {
		jumpCount = 0;
		msgCount = 0;
		jumpText.GetComponent<TextMesh> ().text = "Jumps: " + jumpCount;
//		jumpText.GetComponent<Text>().text = "Jumps: " + jumpCount;
		bufferSize = 1024;
		initServer ();
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

//	private MyBody receivedBody;
	private bsMyBody receivedBody;

	void Update() {
		jumpText.GetComponent<TextMesh>().text = "Jumps: " + jumpCount + " / Msgs: " + msgCount;
		if (jump) {
			jumpCount += 1;
//			jumpText.GetComponent<TextMesh>().text = "Jumps: " + jumpCount + " / Msgs: " + msgCount;
			jump = false;
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
				break;
		case NetworkEventType.DataEvent:
			Stream stream = new MemoryStream (recBuffer);
			BinaryFormatter formatter = new BinaryFormatter ();
				
			receivedBody = (bsMyBody)formatter.Deserialize (stream);
			msgCount += 1;
				handleReceivedData();

				break;
			case NetworkEventType.DisconnectEvent:
				Debug.Log ("remote client event disconnected");
				break;
			}

	}

	//here we deal with the data we have received and we assign the coordinance
	//i ignore the rotation of the head because that is bing controlled by cardboard/gearVR
	public void handleReceivedData()
	{
		headRec.transform.position = receivedBody.head;
//		leftHandRec.transform.position = receivedBody.leftHand;
//		leftHandRec.transform.rotation = receivedBody.leftHand;
//		rightHandRec.transform.position = receivedBody.rightHand;
//		rightHandRec.transform.rotation = receivedBody.rightHand;

//		leftArm.transform.position = receivedBody.leftArm;
//		leftArm.transform.rotation = receivedBody.leftArm;
//		rightArm.transform.position = receivedBody.rightArm;
//		rightArm.transform.rotation = receivedBody.rightArm;
//		leftLeg.transform.position = receivedBody.leftLeg;
//		leftLeg.transform.rotation = receivedBody.leftLeg;
//		rightLeg.transform.position = receivedBody.rightLeg;
//		rightLeg.transform.rotation = receivedBody.rightLeg;
		
//		if (receivedBody.jump) {
//			jump = true;
//			camJump = true;
//			blockJump = true;
//		}
		jump = receivedBody.jump;
		camJump = jump;
		blockJump = jump;
		
//		if (jump) {
//			jumpCount += 1;
//			jumpText.GetComponent<TextMesh>().text = "Jumps: " + jumpCount;
//		}
	}
}

[System.Serializable]
public struct bsMyBody
{
	public MyBodyPart head;
	public bool jump;

	public bsMyBody(MyBodyPart p_head)
	{
		head = p_head;
		jump = false;
	}

	public void add(bsMyBody m)
	{
		head.add (m.head);
	}

	public void divide(int i)
	{
		head.divide (i);
	}
}


[System.Serializable]
public struct MyBody
{
	public MyBodyPart head, leftHand, rightHand, leftArm, rightArm, leftLeg, rightLeg;
//	public Boolean jump;

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
	public MyBody(MyBodyPart p_head,MyBodyPart p_left, MyBodyPart p_right, MyBodyPart p_leftArm, MyBodyPart p_rightArm, MyBodyPart p_leftLeg, MyBodyPart p_rightLeg)
	{
		head = p_head;
		leftHand = p_left;
		rightHand = p_right;
		leftArm = p_leftArm;
		rightArm = p_rightArm;
		leftLeg = p_leftLeg;
		rightLeg = p_rightLeg;

	}
	public void add(MyBody m)
	{
		head.add (m.head);
		leftHand.add (m.leftHand);
		rightHand.add(m.rightHand);
		leftArm.add (m.leftArm);
		rightArm.add(m.rightArm);
		leftLeg.add (m.leftLeg);
		rightLeg.add(m.rightLeg);

	}

	public void divide(int i)
	{
		head.divide (i);
		leftHand.divide (i);
		rightHand.divide (i);
		leftArm.divide (i);
		rightArm.divide (i);
		leftLeg.divide (i);
		rightLeg.divide (i);
	}
}

[System.Serializable]
public struct MyBodyPart
{
	public float x,y,z;
	public float rx, ry, rz, rw;

	public void add(MyBodyPart m)
	{	
		x  += m.x;
		y += m.y;
		z += m.z;
		rx += m.rx;
		ry += m.ry;
		rz += m.rz;
		rw += m.rw;
	}

	public void divide(int i)
	{
		x = x / i;
		y = y / i;
		z = z / i;
		rx = rx / i;
		ry = ry / i;
		rz = rz / i;
		rw = rw / i;
	}


	public MyBodyPart(float rX, float rY, float rZ, float rrx, float rry, float rrz, float rrw)
	{
		x = rX;
		y = rY;
		z = rZ;
		rx = rrx;
		ry = rry;
		rz = rrz;
		rw = rrw;

	}

	public MyBodyPart(Vector3 rValue, Quaternion rQuat)
	{
		x = rValue.x;
		y = rValue.y;
		z = rValue.z;
		rx = rQuat.x;
		ry = rQuat.y;
		rz = rQuat.z;
		rw = rQuat.w;
	}

	public void populate(Vector3 rValue, Quaternion rQuat)
	{
		x = rValue.x;
		y = rValue.y;
		z = rValue.z;
		rx = rQuat.x;
		ry = rQuat.y;
		rz = rQuat.z;
		rw = rQuat.w;
	}

	public override string ToString()
	{
		return String.Format("[{0}, {1}, {2}:{3}, {4}, {5}]", x, y, z, rx, ry, rz);
	}

	public static implicit operator Quaternion(MyBodyPart rValue)
	{
		return new Quaternion(rValue.rx, rValue.ry, rValue.rz, rValue.rw);
	}

	public static implicit operator Vector3(MyBodyPart rValue)
	{
		return new Vector3(rValue.x, rValue.y, rValue.z);
	}
}

	