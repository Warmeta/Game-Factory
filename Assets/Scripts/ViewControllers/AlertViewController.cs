using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class AlertViewController : GenericViewController {
	
	public Text alert;
	
	void Awake () {
		Initialize();
		viewName = "ALERT_VIEW";
		viewType = ViewTypes.WHOLE_WINDOW;
	}

	void Update(){
		//transform.Translate(0, 0, Time.deltaTime * 2);
	}

	public override void Show(bool value = true, string previousScreenPath = "") {
		base.Show(value);
		if (value) {
			//Show Tween
		} else {
			//Hide Tween
			gameObject.SetActive(false);
		}
	}

	public void Back(){
		Debug.Log ("Back");
		navigationController.Back();
	}

	public void Accept(){
		Debug.Log ("Alert Accept.");
		navigationController.Back();
	}

	public void SetAlert(String s){
		alert.GetComponent<LoadTranslationGUI>().textInKeyLanguage = s;
	}
	
}
