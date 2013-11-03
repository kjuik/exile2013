using UnityEngine;
using System.Collections;

public class ColliderNotifier : MonoBehaviour {
	
	private Objective observer;
	private Transform target;
	
	public void RegisterListener(Objective o, Transform target){
		
		this.observer = o;
		this.target = target;
		
		if (this.collider == null)
			gameObject.AddComponent<MeshCollider>();
		
		if (target.collider == null)
			target.gameObject.AddComponent<MeshCollider>();
	}
	
	void OnTriggerEnter (Collider c){
		if (c.transform == target){
			observer.NotifyCollided();
			Destroy(this);
		}
	}
}
