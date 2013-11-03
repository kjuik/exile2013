using UnityEngine;
using System.Collections;

public class Objective : MonoBehaviour {
	
	public enum State {
		Above,
		Below,
		Near,
		
		Collided,
		CollidedWithFloor,
		AboveFloor,
		
		Rolled
	}
	private bool collided = false;
	private bool rotated = false;
	
	public Transform QuestItem;
	public Transform QuestLocation;
	public State DesiredState;
	
	public float NearTreshold = 1f;
	
	void Start(){
		if (DesiredState == State.Collided){
			QuestItem.gameObject.AddComponent<ColliderNotifier>()
				.RegisterListener(this,QuestLocation);

		}
		else if (DesiredState == State.CollidedWithFloor){
			QuestItem.gameObject.AddComponent<ColliderNotifier>()
				.RegisterListener(this,Floor.Instance.transform);

		}else if (DesiredState == State.Rolled){
			QuestItem.gameObject.AddComponent<Rotation360Notifier>()
				.RegisterListener(this);

		}
	}
	
	public bool Complete {
		get
		{
			if (DesiredState == State.Below){
				return (QuestItem.position.y <= QuestLocation.position.y);
			}
			if (DesiredState == State.Above){
				return (QuestItem.position.y >= QuestLocation.position.y);
			}
			if (DesiredState == State.AboveFloor){
				return (QuestItem.position.y >= Floor.Instance.transform.position.y);
			}
			if (DesiredState == State.Near){
				return (QuestItem.position - QuestLocation.position).magnitude < NearTreshold;
			}
			if (DesiredState == State.Collided){
				return collided;
			}
			if (DesiredState == State.CollidedWithFloor){
				return collided;
			}
			if (DesiredState == State.Rolled){
				return rotated;
			}
			
			return false;
		}
	}
	
	public void NotifyCollided(){
		collided = true;	
	}
	public void NotifyRotated(){
		rotated = true;	
	}
}
