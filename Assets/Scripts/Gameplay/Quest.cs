using UnityEngine;
using System.Collections.Generic;

public class Quest : MonoBehaviour {

	public bool Running = false;
	public string Title;
	public List<Objective> Objectives;
	
	public GameObject Dog;
	private Vector3 startPos;
	private Vector3 startRot;
	
	void Start(){
		startPos = new Vector3(
			Dog.transform.position.x,
			Dog.transform.position.y,
			Dog.transform.position.z
			);
		startRot = new Vector3(
			Dog.transform.rotation.eulerAngles.x,
			Dog.transform.rotation.eulerAngles.y,
			Dog.transform.rotation.eulerAngles.z
			);
	}
	
	public void StartQuest(){
		GameManager.Instance.Say(Title);
		GameManager.Instance.IntoMainButton.gameObject.SetActive(true);
		Invoke ("SetRunning",3);
	}
	
	public void SetRunning(){
		Running = true;	
		if (!this.name.Contains("Sandbox"))
			GameManager.Instance.ResetButton.gameObject.SetActive(true);
	}
	
	public void Finish(){
		GameObject.Destroy(this.gameObject,3);
		GameManager.Instance.QuestComplete();
	}
	
	void Update(){
		if (Running && ObjectivesComplete()){
			Running = false;
			Finish();
		}
	}
	
	bool ObjectivesComplete(){
	
		foreach(Objective o in Objectives){
			if (!o.Complete)
				return false;
		}
		
		GameManager.Instance.ResetButton.gameObject.SetActive(false);
		return true;
	}
}
