using UnityEngine;
using System.Collections;

public class HowToPlayButton : MonoBehaviour {

	public void OnClick(){
		gameObject.SetActive(false);
		GameManager.Instance.SendMessage("MainMenu");	
	}
}
