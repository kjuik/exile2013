using UnityEngine;
using System.Collections;

public class CameraFollowTarget : MonoBehaviour {
	
	public Transform target;
	
	// Use this for initialization
	void Start () {
	
	}
	
	void LateUpdate () {
		
		transform.LookAt(target);
	}
}
