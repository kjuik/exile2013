using UnityEngine;
using System.Collections;
using System.Linq;

public enum Muscles {
	Biceps,
	Triceps,
	Chest
}

public class DogLeg : MonoBehaviour {

	public Transform lowerLeg;
	// muscles are empty gameobjects parented under the rigidbodies that should be affected by the muscle, at the correct positions.
	public Transform[] biceps;
	public Transform[] triceps;
	public Transform[] chest;
	float minBicepsLength = 0.4f;
	float maxBicepsLength = 0.5f;
	float minTricepsLength = 0.5f;
	float maxTricepsLength = 0.6f;
	float minChestLength = 0.4f;
	float maxChestLength = 0.5f;
	
	
	// 0 - this object's rotation
	// 1 - lower leg's rotation
	public Quaternion targetRotation;
	public Quaternion lowerLegTargetRotation;
	
	
	void Start() {
		lowerLeg = GameObject.Find("LegLower-" + transform.name.Substring(9, 2)).transform;
		
		targetRotation = Quaternion.identity;
		lowerLegTargetRotation = Quaternion.identity;
		targetRotation = transform.rotation;
		lowerLegTargetRotation = lowerLeg.rotation;
		
		float minBicepsLength = (biceps[0].position - biceps[1].position).magnitude-0.01f;
		float maxBicepsLength = (biceps[0].position - biceps[1].position).magnitude+0.01f;
		float minTricepsLength = (triceps[0].position - triceps[1].position).magnitude-0.01f;
		float maxTricepsLength = (triceps[0].position - triceps[1].position).magnitude+0.01f;
		float minChestLength = (chest[0].position - chest[1].position).magnitude-0.01f;
		float maxChestLength = (chest[0].position - chest[1].position).magnitude+0.01f;
		
		
	}
	
	public static void Contract(Transform[] muscle, float amount) {
		Rigidbody r;
		r = muscle[0].parent.rigidbody;
		r.AddForceAtPosition((muscle[1].position-muscle[0].position).normalized * amount * r.mass, muscle[0].position, ForceMode.Force);
		r = muscle[1].parent.rigidbody;
		r.AddForceAtPosition((muscle[0].position-muscle[1].position).normalized * amount * r.mass, muscle[1].position, ForceMode.Force);
	
	}
	
	public float GetLegAngle(Transform legPart = null) {
		if (legPart == null)
			legPart = transform;
		return legPart.rotation.eulerAngles.x;
		
	}
	
	
	public float GetMuscleLength(Transform[] muscle) {
		if (muscle == triceps) {
			if (minTricepsLength > (muscle[0].position - muscle[1].position).magnitude) {
				minTricepsLength = (muscle[0].position - muscle[1].position).magnitude;
			}
			if (maxTricepsLength < (muscle[0].position - muscle[1].position).magnitude) {
				maxTricepsLength = (muscle[0].position - muscle[1].position).magnitude;
			}
			return ((muscle[0].position - muscle[1].position).magnitude - minTricepsLength)/(maxTricepsLength-minTricepsLength);
		} else if (muscle == biceps) {
			if (minBicepsLength > (muscle[0].position - muscle[1].position).magnitude) {
				minBicepsLength = (muscle[0].position - muscle[1].position).magnitude;
			}
			if (maxBicepsLength < (muscle[0].position - muscle[1].position).magnitude) {
				maxBicepsLength = (muscle[0].position - muscle[1].position).magnitude;
			}
			return ((muscle[0].position - muscle[1].position).magnitude - minBicepsLength)/(maxBicepsLength-minBicepsLength);
		} else if (muscle == chest) {
			if (minChestLength > (muscle[0].position - muscle[1].position).magnitude) {
				minChestLength = (muscle[0].position - muscle[1].position).magnitude;
			}
			if (maxChestLength < (muscle[0].position - muscle[1].position).magnitude) {
				maxChestLength = (muscle[0].position - muscle[1].position).magnitude;
			}
			return ((muscle[0].position - muscle[1].position).magnitude - minChestLength)/(maxChestLength-minChestLength);
		}
		return 0;
	}
	
	public bool isGrounded() {
		return Physics.Raycast(lowerLeg.transform.TransformDirection(Vector3.down/2), Vector3.down, 0.1f);
	}
	
	[Range(0, 200)]
	public float keepStillForce = 0;
	
	[Range(-100,100)]
	public float moveAroundX;

	[Range(-100,100)]
	public float moveAroundY;
		
	void Update() {
		
	}
	
	void OnDrawGizmos() {
		
		if (biceps[0]) {
			Debug.DrawLine (biceps[0].position, biceps[1].position, Color.green);
			
		}
		if (triceps[0]) {
			Debug.DrawLine(triceps[0].position, triceps[1].position, new Color(0, 220, 0, 255));
		
		}
		if (chest[0]) {
			Debug.DrawLine(chest[0].position, chest[1].position, new Color(30, 255, 0, 255));
		
		}
	}
	
}
