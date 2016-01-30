using UnityEngine;
using System.Collections;

public class npcBehaviour : MonoBehaviour {

	public GameObject uimanager;
	public string[] conversationLines;



	/**
	 * Start a conversation on mouseUp
	 */
	void OnMouseUp() {

		uimanager.GetComponent<uiManager>().startConversation(conversationLines, gameObject);
	}
	// END OnMouseUp()



	/**
	 *	Do something when the conversation ends 
	 */
	public void conversationEnded() {

		Debug.Log ("conversation ended");
	}
	// END conversationEnded()
}
