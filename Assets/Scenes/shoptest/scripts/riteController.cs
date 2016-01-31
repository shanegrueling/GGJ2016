using UnityEngine;
using System.Collections;

public class riteController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
//		gameObject.GetComponent<RectTransform> ().rotation = Quaternion.Euler(0, 0, Time.deltaTime * 900);
		gameObject.GetComponent<RectTransform> ().rotation = Quaternion.Euler(0, 0, Time.fixedTime * 120);
	}
}
