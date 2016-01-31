using UnityEngine;
using System.Collections;
using System.Resources;

public class npcController : MonoBehaviour {

	public GameObject uiManager;
	public GameObject npcPrefab;
	public GameObject atkStage;
	public GameObject gameManager;
	public GameObject speaktosound;
	public Sprite[] npcSprite;



	// Use this for initialization
	void Start () {
	
		string[] items = {
			"water",
			"kagami",
			"omamori",
			"komainu",
			"whitespell",
		};

		var numNpcs = Random.Range (20, 30);

		while (numNpcs-- >= 0) {

			_generateNpc (items);
		}
	}


	private void _generateNpc(string[] items) {

		var pos = new Vector3 (Random.Range (-50, 50), Random.Range (-50, 50), 0);

		GameObject tmpNpc = (GameObject)Instantiate (npcPrefab, pos, Quaternion.identity);

		int type = Random.Range (0, 5);
		tmpNpc.GetComponent<SpriteRenderer>().sprite = npcSprite[type];
		tmpNpc.GetComponent<npcBehaviour>().uimanager = uiManager;
		tmpNpc.GetComponent<npcBehaviour>().attackStage = atkStage;
		tmpNpc.GetComponent<npcBehaviour>().gameManager = gameManager;
		tmpNpc.GetComponent<npcBehaviour>().speaktosound = speaktosound;


		// create random "needed" items
		if(type < 2) {

			// grown up

			int numItems = Random.Range(1,3);

			string[] neededItems = new string[numItems];

			while(--numItems >= 0) {

				neededItems [numItems] = items [Random.Range (0, 4)];
				tmpNpc.GetComponent<npcBehaviour>().neededItems = neededItems;
			}

		} else {

			// child

			int numItems = Random.Range(1,2);

			string[] neededItems = new string[numItems];

			while(--numItems >= 0) {

				neededItems [numItems] = items [Random.Range (0, 4)];
				tmpNpc.GetComponent<npcBehaviour>().neededItems = neededItems;
			}
		}
	}
}