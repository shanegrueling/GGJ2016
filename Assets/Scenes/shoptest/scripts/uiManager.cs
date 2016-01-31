using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using itemsystem;

public class uiManager : MonoBehaviour {

	public Animator inventoryUiAnimator;
	public Animator shopUiAnimator;
	public GameObject conversationUi;
	public GameObject speechBubblePrefab;
	public GameObject player;
	public GameObject closeShopBtn;

	public GameObject attackAnimationUi;

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
			player.GetComponent<MoveToClick> ().inConversationOrMenu = true;
		} else {

			inventoryUiAnimator.SetBool ("isHidden", true);
			player.GetComponent<MoveToClick> ().inConversationOrMenu = false;
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
			player.GetComponent<MoveToClick> ().inConversationOrMenu = true;
			StartCoroutine (showShopClose ());
		} else {

			inventoryUiAnimator.SetBool ("isHidden", true);
			shopUiAnimator.SetBool ("isHidden", true);
			player.GetComponent<MoveToClick> ().inConversationOrMenu = false;
			closeShopBtn.SetActive (false);
		}
	}
	// END toggleShop()



	/**
	 * open speech bubble to start a conversation
	 */
	public void startConversation(string[] neededItems, GameObject convPartner) {

		if(inventoryUiAnimator.GetBool("isHidden") && shopUiAnimator.GetBool("isHidden") && !_inConversation) {

			player.GetComponent<MoveToClick> ().inConversationOrMenu = true;

			_inConversation = true;
			_currentConversationPartner = convPartner;

			GameObject speechBubble = Instantiate (speechBubblePrefab);
			speechBubble.transform.SetParent (conversationUi.transform, false);
			speechBubble.transform.position = convPartner.transform.position;
//			speechBubble.transform.Find ("text").GetComponent<Text> ().text = conversationLines [_currentConversationLine];

			speechBubble.transform.Find ("text").gameObject.SetActive (false);

			foreach(string item in neededItems) {

				speechBubble.transform.Find (item).gameObject.SetActive (true);
			}


			Button button = speechBubble.GetComponent<Button> ();
			button.onClick.AddListener (continueConversation);
		}
	}
	// END startConversation()



	/**
	 * open speech bubble to show a given message
	 */
	public void showSpeechBubbleWithText(string text, GameObject convPartner) {

		if (inventoryUiAnimator.GetBool ("isHidden") && shopUiAnimator.GetBool ("isHidden") && !_inConversation) {

			player.GetComponent<MoveToClick> ().inConversationOrMenu = true;

			_inConversation = true;
			_currentConversationPartner = convPartner;
			_currentConversation = null;

			GameObject speechBubble = Instantiate (speechBubblePrefab);
			speechBubble.transform.SetParent (conversationUi.transform, false);
			speechBubble.transform.position = convPartner.transform.position;
			speechBubble.transform.Find ("text").GetComponent<Text> ().text = text;

			Button button = speechBubble.GetComponent<Button> ();
			button.onClick.AddListener (continueConversation);
		}
	}




	/**
	 * jump to the next line of the conversation
	 */
	public void continueConversation() {

		_currentConversationLine++;

		// is the current conversation continuable?
		if(_currentConversation == null || _currentConversationLine >= _currentConversation.Length) {

			player.GetComponent<MoveToClick> ().inConversationOrMenu = false;

			_inConversation = false;
			_currentConversationLine = 0;
			_currentConversationPartner.GetComponent<npcBehaviour> ().conversationEnded ();
			_currentConversationPartner = null;

			Destroy(GameObject.FindGameObjectsWithTag("speechbubble")[0]);
		} else {

			GameObject bubble = GameObject.FindGameObjectsWithTag ("speechbubble")[0];
			bubble.transform.Find("text").gameObject.GetComponent<Text>().text = _currentConversation [_currentConversationLine];
		}
	}
	// END continueConversation()


	private IEnumerator showShopClose()
	{
		yield return new WaitForSeconds (1.4f);
		closeShopBtn.SetActive (true);
	}
}
