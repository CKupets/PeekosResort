using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCubeMan : MonoBehaviour {

	public GameObject Head;
	public GameObject Shoulder_Left;
	public GameObject Elbow_Left;
	public GameObject Hand_Left;
	public GameObject Shoulder_Right;
	public GameObject Elbow_Right;
	public GameObject Wrist_Right;
	public GameObject Hand_Right;
	public GameObject Knee_Left;
	public GameObject Foot_Left;
	public GameObject Knee_Right;
	public GameObject Foot_Right;

	public LineRenderer SkeletonLine;

	private GameObject[] bones; 
	private LineRenderer[] lines;
	private GameObject[] spheres;
	private int[] parIdxs;

	private Vector3 initialPosition;
	private Quaternion initialRotation;
	private Vector3 initialPosOffset = Vector3.zero;
	private uint initialPosUserID = 0;

	// Use this for initialization
	void Start () {
//		bones = new GameObject[] {
//			Head,
//			Shoulder_Left, Elbow_Left, Hand_Left,
//			Shoulder_Right, Elbow_Right, Wrist_Right, Hand_Right,
//			Knee_Left, Foot_Left,
//			Knee_Right, Foot_Right
//		};
//
//		// parIdxs?
//
//		// array holding the skeleton lines
//		lines = new LineRenderer[bones.Length];
//
//		if (SkeletonLine)
//		{
//			for(int i = 0; i < lines.Length; i++)
//			{
//				lines[i] = Instantiate(SkeletonLine) as LineRenderer;
//				lines[i].transform.parent = transform;
//				//				spheres[i] = Instat
//			}
//		}
//
//		initialPosition = transform.position;
//		initialRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
//		for(int i = 0; i < bones.Length; i++) 
//		{
//			if(bones[i] != null)
//			{
//				int joint = i;
//
//					bones[i].gameObject.SetActive(true);
//
//					Vector3 posJoint = manager.GetJointPosition(playerID, joint);
//					posJoint.z = !MirroredMovement ? -posJoint.z : posJoint.z;
//
//					Quaternion rotJoint = manager.GetJointOrientation(playerID, joint, !MirroredMovement);
//					rotJoint = initialRotation * rotJoint;
//
//					posJoint -= posPointMan;
//
//					if(MirroredMovement)
//					{
//						posJoint.x = -posJoint.x;
//						posJoint.z = -posJoint.z;
//					}
//
//					bones[i].transform.localPosition = posJoint;
//					bones[i].transform.rotation = rotJoint;
//				}
//				else
//				{
//					bones[i].gameObject.SetActive(false);
//				}
//			}	
//		}
	}
}
