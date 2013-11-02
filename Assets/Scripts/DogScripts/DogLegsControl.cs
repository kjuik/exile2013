using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DogLegsControl : MonoBehaviour {
	
	public DogLeg[] legs;
	
	public float idleForce = 0.2f;
	private float _idleForce = 0.2f;
	
	public float bicepsFullContractForce = 15;
	public float tricepsFullContractForce = 15;
	public float chestFullContractForce = 15;
	public float backLegBonus = 10;
	public float frontLegBonus = 10;
	public float maxChargeBonus = 10;
	float jumpCharge = 0;
	public float chargeSpeed;
	
	float idleTimer = 0;
	public float timeUntilIdle = 1;
	
	public float legContractSpeed = 2f;
	
	
	void Stop() {
		StopAllCoroutines();
		
	}
	
	void Start() {
		
	}
	
	void Discharge() {
		if (jumpCharge > 0) {
			jumpCharge -= chargeSpeed;
		} else {
			jumpCharge = 0;
		}
	}
	
	void Update() {
		
		_idleForce = idleForce;
		
		float LX = Input.GetAxis("Player 0 left X");
		float LY = Input.GetAxis("Player 0 left Y");
		float RX = Input.GetAxis("Player 0 right X");
		float RY = Input.GetAxis("Player 0 right Y");
		
		// idle cycle for inactive bodyparts
		float rotZDelta = GameObject.Find("Body").GetComponent<RotationInfo>().rotationZDelta;
		if (Mathf.Abs(rotZDelta) < 20) {
			
			float threshold = 0.6f;
			// don't play idlecycle on limbs that are functioning
			// bool fl: should you play idle on FL?
			bool fl = (Mathf.Abs(LX) + Mathf.Abs(LY) > threshold) ? false : true;
			bool fr = (Mathf.Abs(LX) + Mathf.Abs(LY) > threshold) ? false : true;
			bool bl = (Mathf.Abs(RX) + Mathf.Abs(RY) > threshold) ? false : true;
			bool br = (Mathf.Abs(RX) + Mathf.Abs(RY) > threshold) ? false : true;
			
			// if something has been pressed, deactivate idle until nothing has been pressed
			
			// idle for each leg depending on axes
			
			// idle cycle for all four legs
			IdleCycle(fl, fr, bl, br);
			
		}
		
		// =====================================input!!!
		
		// for charging jumps and rolls
		
		// if both joysticks are tilted the same way
		if (LX * RX > 0) {
			
		}
		
		if (LY * RY > 0) {
			jumpCharge += chargeSpeed * (LY + RY);
			
		}
		// which means LX * RX > 0
		// or LY * RY > 0
		// then increase the jumpCharge until the max amount.
		// else, discharge it.
		
		
		// =====================================left joystick!!!
		
		
		// tricepses
		if (Mathf.Abs(LY) < 0.5f) {
			
			// ===================================== left joystick: horizontal axis !!!
		
			// ============== leg: front left
			
			// when left controller going left, LX<0 so:
			// extend triceps = contract limb
			ExtendMuscle (legs[0], Muscles.Triceps, -LX);
			// extend triceps = extend biceps
			if (legs[0].GetTricepsLength() < 0.35f) {
				ExtendMuscle (legs[0], Muscles.Biceps, LX);
			} else {
				ExtendMuscle (legs[0], Muscles.Biceps, -LX);
			}
			
			if (LX < 0)
				ExtendMuscle(legs[0], Muscles.Chest, -LX);
			
			
			// ============== leg: front right
			
			
			
			// contract triceps = extend limb
			ContractMuscle (legs[1], Muscles.Triceps, -LX);
			// contract triceps = contract biceps
			// at the end of the contraction, we actually want to extend, just before we switch legs.
			print(legs[1].GetTricepsLength());
			if (legs[1].GetTricepsLength() < 0.35f) {
				ExtendMuscle (legs[1], Muscles.Biceps, -LX);
			} else {
				ContractMuscle (legs[1], Muscles.Biceps, -LX);
			}
			
			if (LX > 0)
				ExtendMuscle(legs[1], Muscles.Chest, LX);
			
			
			// contract biceps = idle other leg so we have some height :)
			if (LX < 0) 
				IdleLeg (0);
			else if (LX > 0)
				IdleLeg (1);
			
			
		}
		else {
			
			// =====================================  left joystick: vertical axis! !!!
			
			
			// contract triceps = extend limb
			ContractMuscle (legs[0], Muscles.Triceps, LY);
			if (LY > 0)
				ExtendMuscle (legs[0], Muscles.Biceps, LY);
			else 
				ExtendMuscle (legs[0], Muscles.Biceps, -LY);
			
			//ContractMuscle (legs[0], Muscles.Triceps, Mathf.Clamp(LY, -1, 0) - Mathf.Clamp(LX, -1, 0));
			
			// same as above
			ContractMuscle (legs[1], Muscles.Triceps, LY);
			//ContractMuscle (legs[1], Muscles.Triceps, Mathf.Clamp(LY, -1, 0) + Mathf.Clamp(LX, 0, 1));
			if (LY > 0)
				ExtendMuscle (legs[1], Muscles.Biceps, LY);
			else
				ExtendMuscle (legs[1], Muscles.Biceps, -LY);
			
			
			
		}
		
		// =====================================right joystick!!!
		
		
		if (Mathf.Abs(RY) < 0.5f) {
			
			// ===================================== right joystick: horizontal !!!
		
			
			ExtendMuscle (legs[2], Muscles.Triceps, -RX);
			if (legs[2].GetTricepsLength() < 0.35f) {
				ExtendMuscle (legs[2], Muscles.Biceps, RX);
			} else {
				ExtendMuscle (legs[2], Muscles.Biceps, -RX);
			}
			
			if (RX < 0)
				ExtendMuscle(legs[2], Muscles.Chest, -RX);
			
			
			ContractMuscle (legs[3], Muscles.Triceps, -RX);
			if (legs[3].GetTricepsLength() < 0.35f) {
				ExtendMuscle (legs[3], Muscles.Biceps, -RX);
			} else {
				ContractMuscle (legs[3], Muscles.Biceps, -RX);
			}
			
			if (RX > 0)
				ExtendMuscle(legs[3], Muscles.Chest, RX);
			
			
			// contract biceps = idle other leg so we have some height
			if (RX < 0) 
				IdleLeg (2);
			else if (RX > 0)
				IdleLeg (3);
			
			
		}
		else {
			// ===================================== right joystick: vertical!!!
		
			
			ContractMuscle (legs[2], Muscles.Triceps, RY);
			ExtendMuscle (legs[2], Muscles.Biceps, RY);
			
			ContractMuscle (legs[3], Muscles.Triceps, RY);
			ExtendMuscle (legs[3], Muscles.Biceps, RY);
			
		}
		
		// bicepses
		// every time we want to contract a triceps 
		// (expand the limb in pointing pos), we want to have the biceps contracted,
		//			and only in the last moments expand it.
		
		// every time we want to expand a triceps (bring limb back)
		// we want a heavily expanded biceps (for pushing into ground)
		
		// and that's why we put the code between the tricepses.
		
		// ============================ jump charge elimination
		// if jumpcharge is positive, we only use it to jump backwards
		// if it is negative, we only use it to jump forward.
		if (jumpCharge > 0) {
			if (LY < 0 && RY < 0) {
				// violent jump forward
			}
		} else {
			if (LY > 0 && RY > 0) {
				
			}
		}
				
	}
	
	void IdleLeg(int i, float idleForce = -100) {
		if (idleForce == -100) {
			idleForce = _idleForce;
		}
		
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
			DogLeg.Contract(legs[i].biceps, -deltaL * idleForce);
			
		} else {
			DogLeg.Contract(legs[i].biceps, deltaL * idleForce);
			
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
			DogLeg.Contract(legs[i].triceps, deltaU * idleForce);
			
		} else {
			DogLeg.Contract(legs[i].triceps, -deltaU * idleForce);
			
		}
	}
	
	void IdleCycle(bool fl, bool fr, bool bl, bool br) {
		// for each leg
		for (int i = 0; i < 4; i++) {
			if ((i==0 && fl) || (i==1 && fr) || (i==2 && bl) || (i==3 && br)) {
				IdleLeg(i);
			}
		}
	}
	
	// put leg close to body
	void ContractFullLeg(DogLeg leg) {
		ContractLeg(leg, 1);
		
	}
	
	// get leg close to body slowly like a nannypussy
	void ContractLeg(DogLeg leg, float amount) {
		float tempBonus = 0;
		if (leg.name.Contains("bl") || leg.name.Contains("br"))
			tempBonus += backLegBonus;
		else 
			tempBonus += frontLegBonus;
		
		// previously I tested here if the triceps was longer or shorter than the biceps, to have a async kind of contraction
		DogLeg.Contract(leg.biceps, leg.GetMuscleLength(leg.biceps) * amount * (bicepsFullContractForce + tempBonus));
		
		DogLeg.Contract(leg.triceps, leg.GetMuscleLength(leg.triceps) * amount * (tricepsFullContractForce + tempBonus));
		
	}
	
	// put leg forward in pointing position
	void ExtendFullLeg(DogLeg leg) {
		ExtendLeg(leg, 1);
		
	}
	
	// close muscle wherever it is
	// if amount is negative, use extend.
	void ContractMuscle(DogLeg leg, Muscles muscle, float amount = 1f) {
		float tempBonus = 0;
		if (leg.name.Contains("bl") || leg.name.Contains("br"))
			tempBonus += backLegBonus;
		else 
			tempBonus += frontLegBonus;
		
		if (amount < 0) {
			ExtendMuscle(leg, muscle, -amount);
		} else {
			switch (muscle) {
			case Muscles.Biceps:
				DogLeg.Contract(leg.biceps, leg.GetMuscleLength(leg.biceps) * (bicepsFullContractForce + tempBonus) * amount);
				
				break;
			case Muscles.Triceps:
				DogLeg.Contract(leg.triceps, leg.GetMuscleLength(leg.triceps) * (tricepsFullContractForce + tempBonus) * amount);
				
				break;
			case Muscles.Chest:
				DogLeg.Contract(leg.chest, leg.GetMuscleLength(leg.chest) * (chestFullContractForce + tempBonus) * amount);
				
				break;
				
			}
		}
	}
	
	// open muscle fully
	void ExtendMuscle(DogLeg leg, Muscles muscle, float amount = 1f) {
		float tempBonus = 0;
		if (leg.name.Contains("bl") || leg.name.Contains("br"))
			tempBonus += backLegBonus;
		else 
			tempBonus += frontLegBonus;
		
		switch (muscle) {
		case Muscles.Biceps:
			DogLeg.Contract(leg.biceps, -(1 - leg.GetMuscleLength(leg.biceps)) * (bicepsFullContractForce + tempBonus) * amount);
			break;
		case Muscles.Triceps:
			
			DogLeg.Contract(leg.triceps, -(1 - leg.GetMuscleLength(leg.triceps)) * (tricepsFullContractForce + tempBonus) * amount);
			
			break;
		case Muscles.Chest:
			DogLeg.Contract(leg.chest, -(1 - leg.GetMuscleLength(leg.chest)) * (chestFullContractForce + tempBonus) * amount);
			break;
			
		}
	}
	
	void ExtendLeg(DogLeg leg, float amount) {
		float tempBonus = 0;
		if (leg.name.Contains("bl") || leg.name.Contains("br"))
			tempBonus += backLegBonus;
		else 
			tempBonus += frontLegBonus;
		
		DogLeg.Contract(leg.biceps, (1 - leg.GetMuscleLength(leg.biceps)) * amount * (bicepsFullContractForce + tempBonus));
		
		DogLeg.Contract(leg.triceps, leg.GetMuscleLength(leg.triceps) * amount * (tricepsFullContractForce + tempBonus));
		
	}
	
}
