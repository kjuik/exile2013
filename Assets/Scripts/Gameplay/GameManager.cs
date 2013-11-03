using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	// Singleton
	private static GameManager _Instance;
	public static GameManager Instance{
		get {return _Instance;}	
	}
	
	void Awake(){
		_Instance = this;	
	}
	
	// Navigation methods
	
	public UIPanel mainMenuPanel;
	public void MainMenu(){
		mainMenuPanel.gameObject.SetActive(true);
		
		ResetButton.gameObject.SetActive(false);
		IntoMainButton.gameObject.SetActive(false);
	}
	
	public void Coop(){
		SpawnQuest();
	}
	
	public void Versus(){
		SpawnFight();
	}
	
	public void Sandbox(){
		SpawnSandbox();
	}
	
	public UIButton HowToPlaySprite;
	public void HowToPlay(){
		HowToPlaySprite.gameObject.SetActive(true);
	}
	
	//Coop mode
	
	public List<Quest> Quests;
	public int questNo = 0;
	public Quest CurrentQuest;
	
	public void SpawnQuest(){
		if (Quests.Count == questNo){
			FinishQuests();
		} else {
			CurrentQuest = (Quest) Instantiate(Quests[questNo]);
			CurrentQuest.StartQuest();	
		}
	}
	
	public void QuestComplete(){
		questNo++;
		
		Say ("You did it!\nGood doggie!",3);
		Instantiate (((References)GameObject.FindObjectOfType(typeof(References))).winSound, Camera.main.transform.position, Quaternion.identity);
		Invoke("SpawnQuest",3);
	}
	
	public void FinishQuests(){
		questNo = 0;
		Say ("You finished all quests!\nCongratulations!\nNow buy DLC's!",3);
		Invoke("MainMenu",3);
	}
	
	//Versus mode
	
	public Fight FightPrefab;
	
	private void SpawnFight(){
		Fight fight = (Fight) Instantiate(FightPrefab);
		fight.StartFight();	
	}
	
	public void FightComplete(Fight.Outcome outcome){
		
		if (outcome == Fight.Outcome.Draw)
			Say ("Draw!",3);
		else if (outcome == Fight.Outcome.Dog1Won)
			Say ("Dog 1 won!",3);
		else if (outcome == Fight.Outcome.Dog2Won)
			Say ("Dog 2 won!",3);
		
		Invoke("MainMenu",3);
	}
	
	//Onscreen messages
	
	
	public Quest SandboxPrefab;
	
	private void SpawnSandbox(){
		CurrentQuest = (Quest) Instantiate(SandboxPrefab);
		CurrentQuest.StartQuest();	
	}
	
	// bla
	
	
	public UILabel msgLabel;
	
	public void Say(string msg, float msgTime = 3){
		msgLabel.text = msg;
		Invoke("ClearMsg",msgTime);
	}
	
	public void ClearMsg(){
		msgLabel.text = "";
	}
	
	//Reset button
	public UIButton ResetButton;
	public void Reset(){
		if (CurrentQuest != null){
			//((DogLegsControl) GameObject.FindObjectOfType(typeof(DogLegsControl))).StopAllCoroutines();
			GameObject.Destroy(CurrentQuest.gameObject);
			CurrentQuest = (Quest) Instantiate(Quests[questNo]);
			CurrentQuest.SetRunning();
		}
	}
	public UIButton IntoMainButton;
	public void IntoMain(){
		if (CurrentQuest != null){
			GameObject.Destroy(CurrentQuest.gameObject);
			questNo = 0;
		}
		MainMenu();
	}
}
