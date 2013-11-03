using UnityEngine;
using System.Collections;

public class CameraSetTargetIfExists : MonoBehaviour {
	
	CameraFollowClever theCamera;
	public Transform defaultPosition;
	
	void Start() {
		theCamera = GetComponent<CameraFollowClever>();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find("Body")) {
			theCamera.target = GameObject.Find("Body").transform;
		} else {
			theCamera.target = defaultPosition;
		}
	}
}
