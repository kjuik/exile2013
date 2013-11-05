using UnityEngine;
using System.Collections;

public class MineExplosion : MonoBehaviour {
	
	public float radius = 10;
	public float power = 1;
	
	// Use this for initialization
	void Start () {
		
		Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders) {
            if (hit.rigidbody)
                hit.rigidbody.AddExplosionForce(power, explosionPos, radius, 2);
            //print ("exploded: " + hit.name);
        }
		
		Invoke ("StopEmit", 1f);
		Destroy(gameObject, 5);
	}
	
	void StopEmit() {
		foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>()) {
			//ps.emissionRate = 0;
			ps.Stop();
		}
	}
}
