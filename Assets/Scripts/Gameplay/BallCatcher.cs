using UnityEngine;
using System.Collections;

public class BallCatcher : MonoBehaviour {
	
	public GameObject Ball;
	
	void OnTriggerEnter (Collider col){
	
		if (col.gameObject == Ball){
			Ball.AddComponent<FixedJoint>().connectedBody = transform.parent.rigidbody;
		}
		
	}
}
