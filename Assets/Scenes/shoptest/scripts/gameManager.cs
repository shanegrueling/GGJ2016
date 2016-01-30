using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {

	public GameObject[] questList;
	public GameObject inventory;
	public GameObject karmaUi;
	public float karmaToBalanceRatio = 0.0005f;

	private string _gamestate = "ingame";
	private float _timeToNextEvent = 5.0f;
	private float _balanceLevel = 0.5f;

	private RectTransform _progressBar;
	private int _screenWidth;


	// Use this for initialization
	void Start () {
	
		_progressBar = karmaUi.transform.Find ("progressbar").gameObject.GetComponent<RectTransform>();
		_screenWidth = Screen.width;

		_progressBar.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, _balanceLevel * _screenWidth);
	}
	// END Start()



	// Update is called once per frame
	void Update () {

		if(_gamestate == "ingame") {

			_timeToNextEvent -= Time.deltaTime;

			// something evil will happen in the gameworld
			if(_timeToNextEvent <= 0) {

				_timeToNextEvent = Random.Range (5, 15);

				// loosing some balance here, eh?
				_balanceLevel -= 0.3f;
			}

			// update progressbar -- later to replace with inking
			_progressBar.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, _screenWidth - _balanceLevel * _screenWidth);


			// balancelevel <= 0? Uhoh, you lost...
			if(_balanceLevel <= 0) {

				Debug.Log ("You just lost");
			}
		}
	}
	// END Update()



	/**
	 * exchange karma for balance
	 */
	public void addKarmaToBalance(int karmaAmount) {

		int playerKarma = inventory.GetComponent<inventoryManager> ().karma;
		int karmaToSpent = karmaAmount;
		if (karmaToSpent > playerKarma) {

			karmaToSpent = playerKarma;
		}

		inventory.GetComponent<inventoryManager> ().karma -= karmaToSpent;
		_balanceLevel += karmaToSpent * karmaToBalanceRatio;
	}
	// END addKarmaToBalance()
}
