using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class menuloader : MonoBehaviour {

	// Use this for initialization
	public void loadMenu() {

		SceneManager.LoadScene ("Scenes/Start/Start");
	}
}
