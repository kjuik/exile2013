using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {

	public string Message;
	
	public void OnClick(){
		transform.parent.gameObject.SetActive(false);
		GameManager.Instance.SendMessage(Message);	
	}
}
