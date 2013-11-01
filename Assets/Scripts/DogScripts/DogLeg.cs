using UnityEngine;
using System.Collections;
using System.Linq;

public class DogLeg : MonoBehaviour {

	public Transform lowerLeg;
	// muscles are empty gameobjects parented under the rigidbodies that should be affected by the muscle, at the correct positions.
	public Transform[] biceps;
	float minAngleBiceps;
	float maxAngleBiceps;
	public Transform[] triceps;
	float minAngleTriceps;
	float maxAngleTriceps;
	
	// 0 - this object's rotation
	// 1 - lower leg's rotation
	public Quaternion targetRotation;
	public Quaternion lowerLegTargetRotation;
	
	
	void Start() {
		lowerLeg = transform.parent.FindChild("LegLower-" + transform.name.Substring(9, 2));
		
		targetRotation = Quaternion.identity;
		lowerLegTargetRotation = Quaternion.identity;
		targetRotation = transform.rotation;
		lowerLegTargetRotation = lowerLeg.rotation;
		
	}
	
	public static void Contract(Transform[] muscle, float amount) {
		muscle[0].parent.rigidbody.AddForceAtPosition((muscle[1].position-muscle[0].position).normalized * amount, muscle[0].position, ForceMode.Force);
		muscle[1].parent.rigidbody.AddForceAtPosition((muscle[0].position-muscle[1].position).normalized * amount, muscle[1].position, ForceMode.Force);
	
	}
	
	public float getAngle() {
		return transform.rotation.eulerAngles.x;
		
	}
	
	
	[Range(0, 200)]
	public float keepStillForce = 0;
	
	[Range(-100,100)]
	public float moveAroundX;

	[Range(-100,100)]
	public float moveAroundY;
		
	void Update() {
		moveAroundX = 10 * Input.GetAxis ("Horizontal");
		moveAroundY = 30 * Input.GetAxis ("Vertical");
		if (name.Contains("fl") || name.Contains("br")) {
			Contract (biceps, moveAroundX);
			//Contract (biceps, keepStillForce);
			//Contract (biceps, -keepStillForce);
		} else if (name.Contains("fr") || name.Contains("bl")) {
			Contract (biceps, -moveAroundX);
			
		}
		
		if (name.Contains("fl") || name.Contains("bl")) {
			Contract (triceps, moveAroundY);
			//Contract (biceps, keepStillForce);
			//Contract (biceps, -keepStillForce);
		} else if (name.Contains("fr") || name.Contains("br")) {
			Contract (triceps, -moveAroundY);
			
		}
	}
	
	void OnDrawGizmos() {
		
		if (biceps[0]) {
			Debug.DrawLine (biceps[0].position, biceps[1].position, Color.green);
			
		}
		if (triceps[0])
			Debug.DrawLine(triceps[0].position, triceps[1].position, Color.green);
		
	}
	
}
