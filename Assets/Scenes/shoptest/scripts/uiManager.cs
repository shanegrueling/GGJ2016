using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using itemsystem;

public class uiManager : MonoBehaviour {

	public Animator inventoryUiAnimator;
	public Animator shopUiAnimator;
	public GameObject conversationUi;
	public GameObject speechBubblePrefab;

	private string[] _currentConversation;
	private int _currentConversationLine = 0;
	private GameObject _currentConversationPartner;
	private bool _inConversation = false;


	/**
	 * show / hide inventory
	 */
	public void toggleInventory() {

		if (inventoryUiAnimator.GetBool ("isHidden")) {

			inventoryUiAnimator.SetBool ("isHidden", false);
		} else {

			inventoryUiAnimator.SetBool ("isHidden", true);
		}
	}
	// END toggleInventory()



	/**
	 * show / hide shop
	 */
	public void toggleShop() {

		if (shopUiAnimator.GetBool ("isHidden")) {

			inventoryUiAnimator.SetBool ("isHidden", false);
			shopUiAnimator.SetBool ("isHidden", false);
		} else {

			inventoryUiAnimator.SetBool ("isHidden", true);
			shopUiAnimator.SetBool ("isHidden", true);
		}
	}
	// END toggleShop()



	/**
	 * open speech bubble to start a conversation
	 */
	public void startConversation(string[] conversationLines, GameObject convPartner) {

		if(inventoryUiAnimator.GetBool("isHidden") && shopUiAnimator.GetBool("isHidden") && !_inConversation) {

			Debug.Log ("conversation started");

			_inConversation = true;
			_currentConversationPartner = convPartner;
			_currentConversation = conversationLines;

			GameObject speechBubble = Instantiate (speechBubblePrefab);
			speechBubble.transform.SetParent (conversationUi.transform, false);
			speechBubble.transform.position = convPartner.transform.position;
			speechBubble.transform.Find ("text").GetComponent<Text> ().text = conversationLines [_currentConversationLine];

			Button button = speechBubble.GetComponent<Button> ();
			button.onClick.AddListener (continueConversation);
		}
	}
	// END startConversation()



	/**
	 * jump to the next line of the conversation
	 */
	public void continueConversation() {

		_currentConversationLine++;

		// is the current conversation continuable?
		if(_currentConversationLine >= _currentConversation.Length) {

			_inConversation = false;
			_currentConversationLine = 0;
			_currentConversationPartner.GetComponent<npcBehaviour> ().conversationEnded ();
			_currentConversationPartner = null;

			Destroy(GameObject.FindGameObjectsWithTag("speechbubble")[0]);
		} else {

			Debug.Log ("conversation continued");

			GameObject bubble = GameObject.FindGameObjectsWithTag ("speechbubble")[0];
			bubble.transform.Find("text").gameObject.GetComponent<Text>().text = _currentConversation [_currentConversationLine];
		}
	}
	// END continueConversation()
}
