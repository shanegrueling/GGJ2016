﻿using UnityEngine;
using System.Collections;

public class shopBehaviour : MonoBehaviour {

	public GameObject uiManager;
	public GameObject player;

	void OnMouseUp() {

		var playerObj = GameObject.FindGameObjectWithTag ("Player");
		float distance = Vector3.Distance (playerObj.transform.position, transform.position);

		Debug.Log (distance);

		if (distance <= 5f) {

			if (uiManager.GetComponent<uiManager> ().GetComponent<uiManager> ().shopUiAnimator.GetBool ("isHidden") &&
				!player.GetComponent<MoveToClick>().inConversationOrMenu) {

				uiManager.GetComponent<uiManager> ().toggleShop ();
			}
		}
	}
}