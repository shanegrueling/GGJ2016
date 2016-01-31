using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class gameManager : MonoBehaviour {

	public GameObject[] questList;
	public GameObject inventory;
	public GameObject karmaUi;
	public float karmaToBalanceRatio = 0.0005f;

	private string _gamestate = "ingame";
	private float _timeToNextEvent = 5.0f;
	private float _balanceLevel = 0.5f;

	public GameObject _progressBar;
	private int _screenHeight;


	// Use this for initialization
	void Start () {
	
//		_progressBar = karmaUi.transform.Find ("progressbar").gameObject.GetComponent<RectTransform>();
		_screenHeight = Screen.height;

		_progressBar.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, _screenHeight - _balanceLevel * _screenHeight);
	}
	// END Start()



	// Update is called once per frame
	void Update () {

		if(_gamestate == "ingame") {

			_timeToNextEvent -= Time.deltaTime;

			// something evil will happen in the gameworld
			if(_timeToNextEvent <= 0) {

				_timeToNextEvent = Random.Range (15, 25);

				// loosing some balance here, eh?
				_balanceLevel -= 0.5f;
			}

			// update progressbar -- later to replace with inking
			_progressBar.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, _screenHeight - _balanceLevel * _screenHeight);


			// balancelevel <= 0? Uhoh, you lost...
			if(_balanceLevel <= 0) {

				SceneManager.LoadScene ("lost");
				Debug.Log ("You just lost");
			}

			if(_balanceLevel >= 100) {

				SceneManager.LoadScene ("winscreen");
				Debug.Log ("You just won");
			}
		}
	}
	// END Update()



	/**
	 * exchange karma for balance
	 */
	public void addBalance(float karmaAmount) {

		_balanceLevel += karmaAmount;
	}
	// END addKarmaToBalance()
}
