using UnityEngine;
using System.Collections;

public class RagdollController : MonoBehaviour {
	
	float x, y;
	RagdollControlOverlord overlord;
	
	// Use this for initialization
	void Start () {
		overlord = GetComponent<RagdollControlOverlord>();
	}
	
	// Update is called once per frame
	void Update () {
	
		x = Input.GetAxis("Horizontal");
		y = Input.GetAxis("Vertical");
		
	}
	
	void FixedUpdate() {
		
		overlord.ContractMuscle("LeftLeg", x * 1000);
		
		
	}
}
