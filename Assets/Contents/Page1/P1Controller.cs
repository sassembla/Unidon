using UnityEngine;
using System.Collections;
using UniCMS;

public class P1Controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnBackTapped () {
		Debug.LogError("back!");
		SiteManager.sManager.BackToIndex();
	}
}
