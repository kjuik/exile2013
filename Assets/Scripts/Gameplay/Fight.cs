using UnityEngine;
using System.Collections.Generic;

public class Fight : MonoBehaviour {

	public bool Running = false;
	public string Title;
	
	public List<Objective> P1Objectives;
	public List<Objective> P2Objectives;
	
	public enum Outcome {
		None,
		Dog1Won,
		Dog2Won,
		Draw,
	}
	
	public Outcome outcome {
		get
		{
			bool p1Won = true;
			bool p2Won = true;
			
			foreach(Objective o in P1Objectives){
				if (!o.Complete)
					p1Won = false;
			}
			foreach(Objective o in P2Objectives){
				if (!o.Complete)
					p2Won = false;
			}
			
			if (p1Won && p2Won)
				return Outcome.Draw;
			if (p1Won)
				return Outcome.Dog1Won;
			if (p2Won)
				return Outcome.Dog2Won;
			
			return Outcome.None;
		}
	}
	
	public void StartFight(){
		GameManager.Instance.Say(Title,3);
		Invoke ("setRunning",3);
	}
	
	private void setRunning(){
		Running = true;	
	}
	
	void Update(){
		if (Running && (outcome != Outcome.None)){
			Running = false;
			Win();
		}
	}
	
	void Win(){
		GameObject.Destroy(this.gameObject,3);
		GameManager.Instance.FightComplete(outcome);
	}
	
	
}
