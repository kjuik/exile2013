using UnityEngine;
using System.Collections;

public class Foot : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		// tested and it is possible
		transform.position = transform.parent.position + Vector3.down*2f;
	}
}
