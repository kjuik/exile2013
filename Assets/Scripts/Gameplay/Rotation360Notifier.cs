using UnityEngine;
using System.Collections;

public class Rotation360Notifier : MonoBehaviour {
	
	private Objective observer;
	
	public float initRotation;
	public float prevRotation;
	public float mod = 0f;
	
	void Start() {
		this.initRotation = transform.rotation.eulerAngles.z;
		this.prevRotation = this.initRotation;
	}
	public void RegisterListener(Objective o){
		this.observer = o;
	}
	
	void Update() {
		
		float currRotation = transform.rotation.eulerAngles.z;
		
		if (currRotation+mod > prevRotation + 100)
			mod -= 360f;
		else if (currRotation+mod < prevRotation - 100)
			mod += 360f;
		
		//check if made the roll
		if (currRotation+mod - initRotation > 360 || currRotation+mod - initRotation < -360){
			observer.NotifyRotated();
			Destroy(this);
		}
		
		prevRotation = currRotation+mod;
	}

}
