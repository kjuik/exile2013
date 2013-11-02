using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DogLegsControl : MonoBehaviour {
	
	public DogLeg[] legs;
	public float amountEachLeg;
	public float timeBetweenLegs = 0.8f;
	int t = 0;
	
	public float phaseShift = 0;
	public float speed;
	
	public float idleForce = 0.2f;
	private float _idleForce = 0.2f;
	
	public float barrelRollForce = 10;
	
	public float bicepsFullContractForce = 15;
	public float tricepsFullContractForce = 15;
	public float chestFullContractForce = 15;
	public float[] timings = {1, 1, 1, 1};
	
	public float legContractSpeed = 2f;
	
	IEnumerator WalkCycle(DogLeg leg, int step) {
		
		if (timings.Length < 4)
			yield break;
		
		float timer;
		
		timer = Time.time + timings[0];
		if (step < 1)
		while (Time.time < timer) {
			ExtendFullLeg(leg);
			yield return 0;
		}
		
		timer = Time.time + timings[1];
		if (step < 2)
		while (Time.time < timer) {
			ContractMuscle(leg, Muscles.Triceps);
			ExtendMuscle(leg, Muscles.Biceps);
			yield return 0;
		}
		
		timer = Time.time + timings[2];
		if (step < 3)
		while (Time.time < timer) {
			ContractMuscle(leg, Muscles.Biceps);
			yield return 0;
		}
		
		timer = Time.time + timings[3];
		if (step < 4)
		while (Time.time < timer) {
			ExtendMuscle(leg, Muscles.Triceps);
			yield return 0;
		}
		
		StartCoroutine(WalkCycle(leg, 0));
		
	}
	
	IEnumerator WalkCycle2() {
		float walkTimer = Time.time;
			
		while (true) {
			
			// if still starting...
			if (Time.time - walkTimer < 0.5f) {
				IdleCycle(false, true, true, true);
				for (int i = 0; i < 4; i++) {
					ContractLeg(legs[i], -2);
				}
				ContractMuscle(legs[0], Muscles.Biceps);
			} else { // past initialization time
				
				// legs 0 and 1 are the same as 2 and 3 just reverse. They are two small reverse walking cycles.
				
				// if left leg's biceps is more contracted than right
				if (legs[0].GetMuscleLength(legs[0].biceps) < legs[1].GetMuscleLength(legs[1].biceps) 
					// or if the leg is situated forward compared to the other leg
					|| legs[0].GetLegAngle(legs[0].transform) < legs[1].GetLegAngle(legs[1].transform)) {
					
					// it must mean that --- the left leg was moving forward while the other was moving back !!! :)
					ExtendFullLeg(legs[0]);
					
					// only contract the other leg when the first has hit the ground.
					if (legs[0].isGrounded())
						ExtendMuscle(legs[1], Muscles.Triceps);
					
				} else // the other way around
				if (legs[0].GetMuscleLength(legs[0].biceps) > legs[1].GetMuscleLength(legs[1].biceps) 
					// or if the leg is situated forward compared to the other leg
					|| legs[0].GetLegAngle(legs[0].transform) > legs[1].GetLegAngle(legs[1].transform))
				{
					
					// it must mean that --- the left leg was extended and the other contracted - opposite case compared to above
					ExtendFullLeg(legs[1]);
					
					// only contract the other leg when the first has hit the ground.
					if (legs[0].isGrounded())
						ExtendMuscle(legs[0], Muscles.Triceps);
					
				} else {
					// in all the other unfortunate cases
					IdleCycle(true, true, false, false);
				}
				
				
				if (legs[3].GetMuscleLength(legs[3].biceps) < legs[2].GetMuscleLength(legs[2].biceps) 
					|| legs[3].GetLegAngle(legs[3].transform) < legs[2].GetLegAngle(legs[2].transform)) {
					
					ExtendFullLeg(legs[3]);
					
					if (legs[3].isGrounded())
						ExtendMuscle(legs[2], Muscles.Triceps);
					
				} else 
				if (legs[3].GetMuscleLength(legs[3].biceps) > legs[2].GetMuscleLength(legs[2].biceps) 
					|| legs[3].GetLegAngle(legs[3].transform) > legs[2].GetLegAngle(legs[2].transform))
				{
					
					ExtendFullLeg(legs[2]);
					
					if (legs[3].isGrounded())
						ExtendMuscle(legs[3], Muscles.Triceps);
					
				} else {
					IdleCycle(false, false, true, true);
				}
				
			}
		
			yield return 0;
		}
		
	}
	
	void Stop() {
		StopAllCoroutines();
		
	}
	
	void Start() {
		
	}
	
	void Update() {
		
		float LX = Input.GetAxis("Player 0 left X");
		float LY = Input.GetAxis("Player 0 left Y");
		float RX = Input.GetAxis("Player 0 right X");
		float RY = Input.GetAxis("Player 0 right Y");
		
		// idle cycle for inactive bodyparts
		float rotZDelta = GameObject.Find("Body").GetComponent<RotationInfo>().rotationZDelta;
		if (Mathf.Abs(rotZDelta) < 30) {
			
			float threshold = 0.1f;
			// don't play idlecycle on limbs that are functioning :)
			bool fl = (Mathf.Abs(LX) + Mathf.Abs(LY) > threshold ? false : true);
			bool fr = (Mathf.Abs(LX) + Mathf.Abs(LY) > threshold ? false : true);
			bool bl = (Mathf.Abs(RX) + Mathf.Abs(RY) > threshold ? false : true);
			bool br = (Mathf.Abs(RX) + Mathf.Abs(RY) > threshold ? false : true);
			
			// idle cycle for all four legs
			IdleCycle(fl, fr, bl, br);
			
		}
		
		_idleForce = idleForce - ((LX + LY + RX + RY)) / 4;
		
		ContractMuscle (legs[0], Muscles.Triceps, LY);
		//ContractMuscle (legs[0], Muscles.Triceps, Mathf.Clamp(LY, -1, 0) - Mathf.Clamp(LX, -1, 0));
		
		ContractMuscle (legs[1], Muscles.Triceps, LY);
		//ContractMuscle (legs[1], Muscles.Triceps, Mathf.Clamp(LY, -1, 0) + Mathf.Clamp(LX, 0, 1));
		
		ContractMuscle (legs[2], Muscles.Triceps, RY);
		//ContractMuscle (legs[2], Muscles.Triceps, Mathf.Clamp(LY, -1, 0) - Mathf.Clamp(LX, -1, 0));
		
		ContractMuscle (legs[3], Muscles.Triceps, RY);
		//ContractMuscle (legs[3], Muscles.Triceps, Mathf.Clamp(LY, -1, 0) + Mathf.Clamp(LX, 0, 1));
		
		
		
	}
	
	void IdleCycle(bool fl, bool fr, bool bl, bool br) {
		// for each leg
		for (int i = 0; i < 4; i++) {
			if ((i==0 && fl) || (i==1 && fr) || (i==2 && bl) || (i==3 && br)) {
				// calculate the difference of balance from initial rotation
				float currentL = legs[i].lowerLeg.rotation.eulerAngles.x;
				if (currentL > 180)
					currentL = currentL - 360;
				float targetL = legs[i].lowerLegTargetRotation.eulerAngles.x;
				if (targetL > 180) 
					targetL = targetL - 360;
				// the difference of balance
				float deltaL = currentL - targetL;
				
				// reposition the legs according to their muscles
				if (i<=1) {
					DogLeg.Contract(legs[i].biceps, -deltaL * _idleForce);
					
				} else {
					DogLeg.Contract(legs[i].biceps, deltaL * _idleForce);
					
				}
				
				// same but with triceps
				float currentU = legs[i].transform.rotation.eulerAngles.x;
				if (currentU > 180)
					currentU = currentU - 360;
				float targetU = legs[i].targetRotation.eulerAngles.x;
				if (targetU > 180) 
					targetU = targetU - 360;
				// the difference of balance
				float deltaU = currentU - targetU;
				
				if (i<=1) {
					DogLeg.Contract(legs[i].triceps, deltaU * _idleForce);
					
				} else {
					DogLeg.Contract(legs[i].triceps, -deltaU * _idleForce);
					
				}
			}
		}
	}
	
	// put leg close to body
	void ContractFullLeg(DogLeg leg) {
		
		DogLeg.Contract(leg.biceps, leg.GetMuscleLength(leg.biceps) * bicepsFullContractForce);
		
		DogLeg.Contract(leg.triceps, leg.GetMuscleLength(leg.triceps) * tricepsFullContractForce);
		
	}
	
	// get leg close to body slowly like a pussynannypuppy
	void ContractLeg(DogLeg leg, float amount) {
		
		// previously I tested here if the triceps was longer or shorter than the biceps, to have a async kind of contraction
		DogLeg.Contract(leg.biceps, leg.GetMuscleLength(leg.biceps) * amount * bicepsFullContractForce);
		
		DogLeg.Contract(leg.triceps, leg.GetMuscleLength(leg.triceps) * amount * tricepsFullContractForce);
		
	}
	
	// put leg forward in pointing position
	void ExtendFullLeg(DogLeg leg) {
		
		DogLeg.Contract(leg.biceps, (1 - leg.GetMuscleLength(leg.biceps)) * bicepsFullContractForce);
		
		DogLeg.Contract(leg.triceps, leg.GetMuscleLength(leg.triceps) * tricepsFullContractForce);
		
		
	}
	
	// close muscle wherever it is
	// if amount is negative, use extend.
	void ContractMuscle(DogLeg leg, Muscles muscle, float amount = 1f) {
		if (amount < 0) {
			ExtendMuscle(leg, muscle, -amount);
		} else {
			switch (muscle) {
			case Muscles.Biceps:
				DogLeg.Contract(leg.biceps, leg.GetMuscleLength(leg.biceps) * bicepsFullContractForce * amount);
				
				break;
			case Muscles.Triceps:
				DogLeg.Contract(leg.triceps, leg.GetMuscleLength(leg.triceps) * tricepsFullContractForce * amount);
				
				break;
			case Muscles.Chest:
				DogLeg.Contract(leg.chest, leg.GetMuscleLength(leg.chest) * chestFullContractForce * amount);
				
				break;
				
			}
		}
	}
	
	// open muscle fully
	void ExtendMuscle(DogLeg leg, Muscles muscle, float amount = 1f) {
		switch (muscle) {
		case Muscles.Biceps:
			DogLeg.Contract(leg.biceps, (1 - leg.GetMuscleLength(leg.biceps)) * bicepsFullContractForce * amount);
			break;
		case Muscles.Triceps:
			
			DogLeg.Contract(leg.triceps, -(1 - leg.GetMuscleLength(leg.triceps)) * tricepsFullContractForce * amount);
			
			break;
		case Muscles.Chest:
			DogLeg.Contract(leg.chest, (1 - leg.GetMuscleLength(leg.chest)) * chestFullContractForce * amount);
			break;
			
		}
	}
	
	void ExtendLeg(DogLeg leg, float amount) {
		DogLeg.Contract(leg.biceps, (1 - leg.GetMuscleLength(leg.biceps)) * amount * bicepsFullContractForce);
		
		DogLeg.Contract(leg.triceps, leg.GetMuscleLength(leg.triceps) * amount * tricepsFullContractForce);
		
	}
	
}
