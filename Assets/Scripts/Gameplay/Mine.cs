using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {
	
	public GameObject ExplosionPrefab;
	
	void OnCollisionEnter(Collision c){
		
		if (c.collider.transform.parent.GetComponent<DogLegsControl>() != null)
		{
			GameObject.Destroy(this.gameObject);
			Instantiate(ExplosionPrefab,transform.position,transform.rotation);	
		}
	}
}
