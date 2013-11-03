using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {

	// Singleton
	private static Floor _Instance;
	public static Floor Instance{
		get {return _Instance;}	
	}
	
	void Awake(){
		_Instance = this;	
	}
}
