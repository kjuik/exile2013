using UnityEngine;
using System.Collections;
using System.Linq;

public class RagdollOverlord : MonoBehaviour {
	
	public float impactForce;
	public float getUpVelocityThreshold = 0.2f;
	Transform hips;
	
	void AddScriptToRigidbodies() {
		Rigidbody[] r = GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rig in r) {
			ScriptForRigidbodies sfr = rig.gameObject.AddComponent<ScriptForRigidbodies>();
			sfr.overlord = this;
			sfr.impactForce = impactForce;
		}
		
	}
	
	public void ActivateRagdolling(Rigidbody impactTarget, Vector3 force) {
		
		Transform pointer = impactTarget.transform;
		while (!pointer.GetComponent<RagdollHelper>() && pointer.parent != null) {
			pointer = pointer.parent;
		}
		if (!pointer.GetComponent<RagdollHelper>()) {
			return;
		} else {
			//find the RagdollHelper component and activate ragdolling
			RagdollHelper helper = pointer.GetComponent<RagdollHelper>();
			helper.ragdolled=true;
			
			impactTarget.AddForce(force, ForceMode.Impulse);
		}
	}
	
	public void ActivateRagdollingWithReset(Rigidbody impactTarget, Vector3 force, float timeUntilAnimation) {
		ActivateRagdolling(impactTarget, force);
		StartCoroutine (DeactivateRagdollingIfGrounded(timeUntilAnimation));
	}
	
	IEnumerator DeactivateRagdollingIfGrounded(float time) {
		
		while (hips.GetComponent<Rigidbody>().velocity.magnitude > getUpVelocityThreshold || time > 0)
		{
			time-=Time.deltaTime;
			yield return 0;
		}
		
		DeactivateRagdolling();
		
		
	}
	
	public void DeactivateRagdolling() {
		//Pressing space makes the character get up, assuming that the character root has
		//a RagdollHelper script
		RagdollHelper helper=GetComponent<RagdollHelper>();
		helper.ragdolled=false;
	}
	
	void Start() {
		AddScriptToRigidbodies();
		
		if (transform.name.Contains("skeleton"))
			hips = transform.FindChild("Bip01");
		else
			hips = transform.FindChild("Hips");
		
	}
	
	void Update () {
		
		// send ray from screen and check if it hit the ragdoll. if it did, ragdollify it.
		
//		//if left mouse button clicked
//		if (Input.GetMouseButtonDown(0))
//		{
//			//Get a ray going from the camera through the mouse cursor
//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//			
//			//check if the ray hits a physic collider
//			RaycastHit hit; //a local variable that will receive the hit info from the Raycast call below
//			if (Physics.Raycast(ray,out hit))
//			{
//				//check if the raycast target has a rigid body (belongs to the ragdoll)
//				if (hit.rigidbody!=null)
//				{
//					ActivateRagdolling(hit.rigidbody, ray.direction);
//				}
//			}
//		}
		
		if (Input.GetKeyDown(KeyCode.Space))
			DeactivateRagdolling();
		
	}
}
