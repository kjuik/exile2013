using UnityEngine;
using System.Collections;

public class FootSounds : MonoBehaviour {

	public AudioClip[] steps;
	
	void Start() {
		
	}
	
	void OnCollisionEnter() {
		print ("suckit");
		int i = Random.Range(0, steps.Length-1);
		Transform sound = Instantiate ( ((References)GameObject.FindObjectOfType(typeof(References))).footStepSound, transform.position, Quaternion.identity) as Transform;
		sound.audio.clip = steps[i];
	}
	
}
