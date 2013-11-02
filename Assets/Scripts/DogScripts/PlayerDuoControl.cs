using UnityEngine;
using System.Collections;

public class PlayerDuoControl : MonoBehaviour {

	DogLeg[] legs;
	public float moveSpeed = 10;
	
	
	void Start() {
		legs = GetComponent<DogLegsControl>().legs;
		
	}
	
	void Update() {
		
	}
	
	void Update2 () {
		// biceps-triceps, front-back, left-right.
		float[,,] joy = new float[2,2,2];
		
		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < 2; j++) {
				for (int k = 0; k < 2; k++) {
					
					// biceps contractions
					//			 are done on both x axes
					
					//           if left controller, invert x axis, so biceps are closed toward inside of controller and open towards outside
					joy[0,j,k] = (k==0 ? -1 : 1) * Input.GetAxis("Player 1 " + (k==0? "left" : "right") + " X") * 3;
					
					// triceps
					
					// 			if left controller, control front legs, if right controller, control back legs
					joy[1,j,k] = Input.GetAxis("Player 1 " + (j==0 ? "left" : "right") + " Y");
					
					
					joy[i,j,k] *= moveSpeed;
					// axis: Player 1 left X
					// 		 Player 2 right Y
					
				}
			}
		}
		
		DogLeg.Contract(legs[0].biceps, joy[0,0,0]);
		DogLeg.Contract(legs[1].biceps, joy[0,0,1]);
		DogLeg.Contract(legs[2].biceps, joy[0,1,0]);
		DogLeg.Contract(legs[3].biceps, joy[0,1,1]);
		
		DogLeg.Contract(legs[0].triceps, joy[1,0,0]);
		DogLeg.Contract(legs[1].triceps, joy[1,0,1]);
		DogLeg.Contract(legs[2].triceps, joy[1,1,0]);
		DogLeg.Contract(legs[3].triceps, joy[1,1,1]);
		
		
	}
}
