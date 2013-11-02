using UnityEngine;
using System.Collections;
using System.Linq;

public class ChickenControl : MonoBehaviour {
	
	public DogLeg[] legs;
	public float amountEachLeg;
	public float timeBetweenLegs = 0.8f;
	int t = 0;
	
	public float phaseShift = 0;
	public float speed;
	
	public float idleForce = 2;
	
	public float barrelRollForce = 10;
	
	void StartWalking() {
		
		StartCoroutine (Step (legs[0], 180 * Mathf.Deg2Rad));
		StartCoroutine (Step (legs[1], 0));
		
	}
	
	void Stop() {
		StopAllCoroutines();
		
	}
	
	void Start() {
		StartCoroutine (Idle());
		
	}
	
	
	
	IEnumerator Idle() {
		while (true) {
			//Vector3 delta = transform.FindChild("Body").rigidbody.centerOfMass - transform.FindChild("Body").position;
			
			// code to keep legs straight, close to their initial rotation
			// only works if the body is upright and not tilted in any way.
			float rotZDelta = transform.FindChild("Body").GetComponent<RotationInfo>().rotationZDelta;
			if (Mathf.Abs(rotZDelta) < 30) {
				
				for (int i = 0; i < 2; i++) {
					float currentL = legs[i].lowerLeg.rotation.eulerAngles.x;
					if (currentL > 180)
						currentL = currentL - 360;
					float targetL = legs[i].lowerLegTargetRotation.eulerAngles.x;
					if (targetL > 180) 
						targetL = targetL - 360;
					// the difference of balance
					float deltaL = currentL - targetL;
					
					if (i<=1) {
						DogLeg.Contract(legs[i].biceps, -deltaL * idleForce);
						
					} else {
						DogLeg.Contract(legs[i].biceps, deltaL * idleForce);
						
					}
					
					
					float currentU = legs[i].transform.rotation.eulerAngles.x;
					if (currentU > 180)
						currentU = currentU - 360;
					float targetU = legs[i].targetRotation.eulerAngles.x;
					if (targetU > 180) 
						targetU = targetU - 360;
					// the difference of balance
					float deltaU = currentU - targetU;
					
					if (i<=1) {
						DogLeg.Contract(legs[i].triceps, deltaU * idleForce);
						
					} else {
						DogLeg.Contract(legs[i].triceps, -deltaU * idleForce);
						
					}
					
				}
			}// end if body not tilted
			else if (Mathf.Abs(rotZDelta) < 150) {
				// barrel roll around doggy
				
				// instead of this how about using chest muscles?
				transform.FindChild("Body").rigidbody.AddTorque(new Vector3(0, 0, -rotZDelta * barrelRollForce));
				
			} else {
				// don't do anything
				
			}
			
			yield return 0;
			// contract muscles, extend, etc so that everything looks dandy and nice
		}
		
	}
	
	IEnumerator Step(DogLeg leg, float phase) {
		while (true) {
			DogLeg.Contract(leg.biceps, Mathf.Sin (phase) * amountEachLeg);
			DogLeg.Contract(leg.triceps, Mathf.Sin (phase + phaseShift*Mathf.Deg2Rad) * amountEachLeg);
			phase += speed * Mathf.Deg2Rad;
			yield return new WaitForFixedUpdate();
		}
	}
	
	
	
}
