using UnityEngine;
using System.Collections;
using System.Net;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using System;
public class bsSocketServer : MonoBehaviour {

//	public GameObject headRec, leftHandRec, rightHandRec;
	public GameObject jumpText;
	private Text jtext;
	public int hostId;
	private int myChan;
	private int socketId; // above Start()
	private int socketPort = 8888; // Also a class member variable

	private Boolean jump;
	private uint jumpCount;

	int connectionId;

	// Use this for initialization
	void Start () {
		jumpCount = 0;
		bufferSize = 1024;
		initServer ();
	}

	public void initServer()
	{
		NetworkTransport.Init ();
		ConnectionConfig config = new ConnectionConfig ();
		myChan = config.AddChannel (QosType.Unreliable);
		HostTopology topology = new HostTopology(config,10);
		socketId = NetworkTransport.AddHost(topology, socketPort);
		Debug.Log("Socket Open. SocketId is: " + socketId);

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

	// Update is called once per frame
	void Update () {
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

			receivedBody = (MyBody) formatter.Deserialize(stream);

			handleReceivedData();

			break;
		case NetworkEventType.DisconnectEvent:
			Debug.Log ("remote client event disconnected");
			break;
		}

	}

	public void handleReceivedData()
	{
//		headRec.transform.position = receivedBody.head;
//		leftHandRec.transform.position = receivedBody.leftHand;
//		leftHandRec.transform.rotation = receivedBody.leftHand;
//		rightHandRec.transform.position = receivedBody.rightHand;
//		rightHandRec.transform.rotation = receivedBody.rightHand;
//		jump = receivedBody.jump;

		if (jump) {
			jumpCount += 1;
			jumpText.GetComponent<TextMesh>().text = "Jumps: " + jumpCount;
		}
	}
}

//[System.Serializable]
//public struct MyBody
//{
//	public MyBodyPart head, leftHand, rightHand;
//	public Boolean jump;
//
//	public MyBody(MyBodyPart p_head,MyBodyPart p_left, MyBodyPart p_right)
//	{
//		head = p_head;
//		leftHand = p_left;
//		rightHand = p_right;
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
//
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

