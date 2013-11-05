using UnityEngine;
using System.Collections;

public class FootSounds : MonoBehaviour {

	public AudioClip[] steps;
	
	void Start() {
		
	}
	
	void OnCollisionEnter(Collision hit) {
		float hitForce = Mathf.Clamp01(hit.relativeVelocity.magnitude/8);
		Transform s = Instantiate ( ((References)GameObject.FindObjectOfType(typeof(References))).footStepSound, transform.position, Quaternion.identity) as Transform;
		AudioSource sound = s.GetComponent<AudioSource>();
		//int i = Random.Range(0, steps.Length-1);
		//sound.GetComponent<AudioSource>().clip = steps[i];
		
		sound.clip = steps[(int)(hitForce * (steps.Length-1))];
		sound.volume = hitForce*hitForce;
		sound.pitch = Mathf.Lerp(0.7f, 1, hitForce);
		sound.Play();
	}
	
}
