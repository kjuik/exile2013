using UnityEngine;
using System.Collections;
using System.Linq;

public class DogLegsControl : MonoBehaviour {
	
	public int playerNumber;
	public DogLeg[] legs;
	public float amountEachLeg;
	public float timeBetweenLegs = 0.8f;
	int t = 0;
	
	public float phaseShift = 0;
	public float speed;
	
	public float idleForce = 2;
	
	void StartWalking() {
		
		StartCoroutine (Step (legs[0], 180 * Mathf.Deg2Rad));
		StartCoroutine (Step (legs[1], 0));
		StartCoroutine (Step (legs[2], 0));
		StartCoroutine (Step (legs[3], 180 * Mathf.Deg2Rad));
		
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
			
			for (int i = 0; i < 4; i++) {
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
