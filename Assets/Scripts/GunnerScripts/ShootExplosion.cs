using UnityEngine;
using System.Collections;

public class ShootExplosion : MonoBehaviour {

	public Transform explosion;
	public float resetRagdollificationTime = 1;
	
	public void Explode(Vector3 pos) {
		Instantiate (explosion, pos, Quaternion.identity);
	}
	
	void Update() {
		// always lock cursor
		Screen.lockCursor = true;
		
		//if left mouse button clicked
		if (Input.GetMouseButtonDown(0))
		{
			//Get a ray going from the camera through the mouse cursor
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			//check if the ray hits a physic collider
			RaycastHit hit; //a local variable that will receive the hit info from the Raycast call below
			if (Physics.Raycast(ray,out hit))
			{
				Collider[] hits = Physics.OverlapSphere(hit.point, explosion.GetComponent<Detonator>().size);
				
				foreach (Collider h in hits) {
					print ("this is in range: " + h.name);
					
					//check if the raycast target has a rigid body (belongs to the ragdoll)
					if (h.rigidbody!=null)
					{
						// ragdolify
						if (h.transform.GetComponent<ScriptForRigidbodies>())
							h.transform.GetComponent<ScriptForRigidbodies>().overlord.ActivateRagdollingWithReset(h.rigidbody, ray.direction,
								(explosion.GetComponent<Detonator>().size - (h.transform.position - hit.point).magnitude) * resetRagdollificationTime
								);
						
					}
				}
				
				// explode after that
				Explode(hit.point);
				
			}
		}
	}
	
}
