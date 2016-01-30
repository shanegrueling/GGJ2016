using UnityEngine;
using System.Collections;
using Node = NavGrid.Node;

public class npcBehaviour : MonoBehaviour {

	public GameObject uimanager;

	public string[] conversationLines;	// holds all the "before quest" lines of conversation
	public string[] neededItems; 		// which items are needed in order to solve the quest
	private bool questSolved = false; 	// is the quest aready done?

	private GameObject _playerObj;


	void Start() {

		_playerObj = GameObject.FindGameObjectWithTag ("Player");
	}


	/**
	 * Start a conversation on mouseUp
	 */
	void OnMouseUp() {

//		Irgendwas funktioniert hier noch nicht. prüfen, wenn Internet wieder da ist!!!!!
//		if(!_playerObj.GetComponent<player>().currentNpcClient) {
//
//			Debug.Log("Seems like you already got something to do");
//			return;
//		}

		if(_playerObj.GetComponent<player>().currentNpcClient == gameObject) {

			Hashtable playerInventory = _playerObj.GetComponent<player> ().inventory.GetComponent<inventoryManager>().slots;

			foreach(string needed in neededItems) {

				if(!playerInventory.Contains(needed) || (int)playerInventory[needed] <= 0) {

					Debug.Log ("Something is missing in your inventory");
					return;
				}

				questSolved = true;
				_playerObj.GetComponent<player>().currentTask = null;
				_playerObj.GetComponent<player>().currentNpcClient = null;

				Debug.Log ("Yes! You have got everything you need to finish this quest.");
			}

			return;
		}

		// Is the quest already done?
		if (!questSolved) {

			float distance = Vector3.Distance (_playerObj.transform.position, transform.position);

			// Is the NPC right beneath the player?
			if (distance <= 1.1f) {

				// does the NPC has anything to say?
				if (conversationLines.Length > 0) {
			
					uimanager.GetComponent<uiManager> ().startConversation (conversationLines, gameObject);
				}
			}

			return;
		}

	
		Debug.Log ("The quest already has been solved");
		return;
	}
	// END OnMouseUp()



	/**
	 *	Do something when the conversation ends (usually give a quest to the player)
	 */
	public void conversationEnded() {

		// Is the quest already done?
		if (!questSolved) {

			Debug.Log ("Okay, here is your quest...");
			_playerObj.GetComponent<player> ().currentNpcClient = gameObject;
			_playerObj.GetComponent<player> ().currentTask = neededItems;
		}
	}
	// END conversationEnded()
}
