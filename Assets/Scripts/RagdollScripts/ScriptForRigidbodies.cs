using UnityEngine;
using System.Collections;

public class ScriptForRigidbodies : MonoBehaviour {
	
	public RagdollOverlord overlord;
	public float impactForce;
	
	void OnCollisionEnter(Collision hit) {
		if (hit.relativeVelocity.magnitude > 0.4f) {
			overlord.ActivateRagdolling(hit.contacts[0].thisCollider.attachedRigidbody, hit.relativeVelocity * impactForce);
			
		}
	}
	
}
