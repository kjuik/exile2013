using UnityEngine;
using System.Collections;

public class DestroyAfterSecs : MonoBehaviour {
	
	public float secs;
	
	// Use this for initialization
	void Start () {
		Destroy (gameObject, secs);
	}
	
}
