using UnityEngine;
using System.Collections;

public class RotationInfo : MonoBehaviour {
	
	public Quaternion initRotation;
	public float rotationZDelta;
	
	void Start() {
		initRotation = transform.rotation;
		
	}
	
	void Update() {
		
		float initR = initRotation.eulerAngles.z;
		if (initR > 180)
			initR = initR - 360;
		float curR = transform.rotation.eulerAngles.z;
		if (curR > 180) 
			curR = curR - 360;
		// the difference of balance
		rotationZDelta = curR - initR;
		
	}
	
	
}
