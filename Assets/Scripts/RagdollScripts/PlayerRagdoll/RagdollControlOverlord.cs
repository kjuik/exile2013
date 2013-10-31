using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public static class Extensions {
	
	// adds clockwise torque
	public static void AddTorqueAtPosition(this Rigidbody r, Vector3 torque, float factor, float factor2) {
		
		// example:
		// if torque is forward, and pos is left, then we need to add 2 forces:
		// one up and to the right
		// and one down and to the left.
		
		// the first force is the cross product
		// the other one is the negative cross product
		
		// these 2 lines rotate the vector towards the center of mass in local space instead of global :|
		// not actually sure what the fuck
		Vector3 temp = r.centerOfMass;
		temp = r.transform.TransformDirection(r.centerOfMass);
		
		r.AddForceAtPosition(Vector3.Cross(torque, temp).normalized * torque.magnitude, r.transform.position - temp * factor2);
		r.AddForceAtPosition(-Vector3.Cross(torque, temp).normalized * torque.magnitude, r.transform.position + temp * factor);
		
	}
	
}

public class RagdollControlOverlord : MonoBehaviour {
	
	public Transform Hips;
	public Transform Head;
	public Transform Chest;
	public Transform LeftUpperArm;
	public Transform LeftLowerArm;
	public Transform RightUpperArm;
	public Transform RightLowerArm;
	public Transform LeftUpperLeg;
	public Transform LeftLowerLeg;
	public Transform RightUpperLeg;
	public Transform RightLowerLeg;
	
	public float factor = 2;
	public float factor2 = 2;
	
	void AddScriptToRigidbodies() {
		CharacterJoint[] cjs = GetComponentsInChildren<CharacterJoint>();
		foreach (CharacterJoint cj in cjs) {
			PlayerScriptForJoints psfr = cj.gameObject.AddComponent<PlayerScriptForJoints>();
			psfr.overlord = this;
			
		}
	}
	
	public void ContractMuscle(string muscle, float amount) {
		switch (muscle) {
		case "LeftLeg":
			
			//LeftUpperLeg.rigidbody.AddTorque(LeftUpperLeg.GetComponent<CharacterJoint>().swingAxis * -amount);
			LeftLowerLeg.rigidbody.AddTorqueAtPosition(LeftLowerLeg.transform.forward * amount, factor, factor2);
			break;
			
		}
	}
	
	void Start() {
		AddScriptToRigidbodies();
		
	}
	
	void OnDrawGizmos() {
		
		//Vector3.Cross(torque, temp).normalized * torque.magnitude, r.transform.position - temp
		Rigidbody r = LeftLowerLeg.rigidbody;
		Vector3 torque = LeftLowerLeg.transform.forward;
		Vector3 temp = r.centerOfMass;
		temp = r.transform.TransformDirection(r.centerOfMass);
		
		Debug.DrawRay(r.transform.position - temp * factor2, Vector3.Cross(torque, temp).normalized * torque.magnitude, Color.green);
		Debug.DrawRay(r.transform.position + temp * factor, -Vector3.Cross(torque, temp).normalized * torque.magnitude, Color.red);
		
		
	}
}
