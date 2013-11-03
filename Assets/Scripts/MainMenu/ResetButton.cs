using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour {

	public string Message;
	
	public void OnClick(){
		GameManager.Instance.SendMessage(Message);
	}
}
