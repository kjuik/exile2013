using UnityEngine;
using System.Collections;

public class generateThings : MonoBehaviour {
	
	public int amount;
	public float radius;
	public Transform what;
	public KeyCode key;
	
	// Use this for initialization
	void Start () {
	
	}
	
	void Generate() {
		for (int i = 0; i < amount; i++) {
			Instantiate(what, transform.position + new Vector3(Mathf.Sin(2*Mathf.PI/amount * i) * radius, 0, Mathf.Cos(2*Mathf.PI/amount * i) * radius), Quaternion.identity);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	 	if (Input.GetKeyDown(key)) {
			Generate ();
		}
	}
}
