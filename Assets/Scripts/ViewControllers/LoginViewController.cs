using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;
using UnityEngine.UI;

public class LoginViewController : GenericViewController {

	public GameObject emailinput;
	public GameObject passwordinput;
	
	void Awake () {
		Initialize();
		viewName = "LOGIN_VIEW";
		viewType = ViewTypes.CONTENT_SEQUENCE;
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

	public void Login(){
		KiiUser.LogIn(emailinput.GetComponent<InputField>().text, passwordinput.GetComponent<InputField>().text, (KiiUser user, Exception e) => {
			if (e is NetworkException)
			{
				if (((NetworkException)e).InnerException is TimeoutException) {
					// Network timeout occurred
					navigationController.GoTo("ALERT_VIEW");
					applicationController.alertView.GetComponent<AlertViewController>().SetAlert("Network timeout occurred");
					return;
				}
				else
				{
					// Network error occurred
					navigationController.GoTo("ALERT_VIEW");
					applicationController.alertView.GetComponent<AlertViewController>().SetAlert("Network error occurred");
					return;
				}
			}
			else if (e is ConflictException) {
				// Registration failed because the user already exists
				navigationController.GoTo("ALERT_VIEW");
				applicationController.alertView.GetComponent<AlertViewController>().SetAlert("Registration failed because the user already exists");
				return;
			}
			else if (e is CloudException)
			{
				// Email and password dont match
				navigationController.GoTo("ALERT_VIEW");
				applicationController.alertView.GetComponent<AlertViewController>().SetAlert("Email and password dont match");
				return;
			}
			else if (e != null)
			{
				// Unexpected exception occurred
				navigationController.GoTo("ALERT_VIEW");
				applicationController.alertView.GetComponent<AlertViewController>().SetAlert("Unexpected exception occurred");
				return;
			}
			applicationController.sessionStarted = true;
			Debug.Log ("GoTo My Games");
			navigationController.GoTo("GAMES_VIEW");
		});

	}

	public void Exit(){
		Debug.Log ("Exit application");
		Application.Quit();
	}
	
	public void Register(){
		Debug.Log ("GoTo Sign Up");
		navigationController.GoTo("REGISTER_VIEW");
	}

	void OnDisable(){
		emailinput.GetComponent<InputField> ().text = "";
		passwordinput.GetComponent<InputField> ().text = "";
	}
	
}
