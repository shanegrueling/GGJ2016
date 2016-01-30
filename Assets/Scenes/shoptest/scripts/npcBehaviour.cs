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

		// is the NPC within reach of the player?
		float distance = Vector3.Distance (_playerObj.transform.position, transform.position);
		if(distance > 1.2) {

			return;
		}

		// does the player already has an other quest?
		if(_playerObj.GetComponent<player>().currentNpcClient != gameObject && _playerObj.GetComponent<player>().currentNpcClient != null) {

			uimanager.GetComponent<uiManager> ().showSpeechBubbleWithText ("Seems like you already got something to do", gameObject);
			return;
		}

		// is the current quest of the player (if given) from the same NPC?
		if(_playerObj.GetComponent<player>().currentNpcClient == gameObject) {

			Hashtable playerInventory = _playerObj.GetComponent<player> ().inventory.GetComponent<inventoryManager>().slots;

			// test if all required items are in the players inventory
			foreach(string needed in neededItems) {

				if(!playerInventory.Contains(needed) || (int)playerInventory[needed] <= 0) {

					uimanager.GetComponent<uiManager> ().showSpeechBubbleWithText ("I think you still need some more things for your ritual", gameObject);
					return;
				}
			}

			// delete items from player inventory
			foreach (string needed in neededItems) {

				_playerObj.GetComponent<player> ().inventory.GetComponent<inventoryManager> ().removeItem (needed);
			}

			questSolved = true;
			_playerObj.GetComponent<player>().currentTask = null;
			_playerObj.GetComponent<player>().currentNpcClient = null;

			uimanager.GetComponent<uiManager> ().showSpeechBubbleWithText ("Yes you've got everything you need. Thanks pal!", gameObject);

			return;
		}

		// Is the quest already done?
		if (!questSolved) {

			// does the NPC has anything to say?
			if (conversationLines.Length > 0) {
		
				uimanager.GetComponent<uiManager> ().startConversation (conversationLines, gameObject);
			}

			return;
		}

	
		uimanager.GetComponent<uiManager> ().showSpeechBubbleWithText ("Mhh?", gameObject);
		return;
	}
	// END OnMouseUp()



	/**
	 *	Do something when the conversation ends (usually give a quest to the player)
	 */
	public void conversationEnded() {

		// Is the quest already done?
		if (!questSolved) {

			_playerObj.GetComponent<player> ().currentNpcClient = gameObject;
			_playerObj.GetComponent<player> ().currentTask = neededItems;
		}
	}
	// END conversationEnded()
}
