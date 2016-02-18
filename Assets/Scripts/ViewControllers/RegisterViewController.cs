using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;
using System.Text.RegularExpressions;

public class RegisterViewController : GenericViewController {

	public GameObject emailInput;
	public GameObject passwordInput;
	public GameObject errorEmail;
	public GameObject errorPassword;

	void Awake () {
		Initialize();
		viewName = "REGISTER_VIEW";
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

	public void Register(){
		bool validate = true;
		errorEmail.SetActive(false);
		errorPassword.SetActive(false);

		if (!ValidateEmail (emailInput.GetComponent<InputField> ().text)) {
			validate = false;
			errorEmail.GetComponent<LoadTranslationGUI>().textInKeyLanguage = "(Email is not valid)";
			errorEmail.SetActive(true);
		}
		if (passwordInput.GetComponent<InputField> ().text.Length < 4) {
			validate = false;
			errorPassword.GetComponent<LoadTranslationGUI>().textInKeyLanguage = "(Password is not valid)";
			errorPassword.SetActive(true);
		}

		if (validate) {
			KiiUser.Builder builder;
			builder = KiiUser.BuilderWithEmail (emailInput.GetComponent<InputField>().text);
			KiiUser user = builder.Build ();
			user["CompanyName"] = "";

			user.Register (passwordInput.GetComponent<InputField>().text, (KiiUser registeredUser, Exception e) => {
				if (e is NetworkException) {
					if (((NetworkException)e).InnerException is TimeoutException) {
						// Network timeout occurred
						applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Network timeout occurred");
						navigationController.GoTo ("ALERT_VIEW");
						return;
					} else {
						// Network error occurred
						applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Network error occurred");
						navigationController.GoTo ("ALERT_VIEW");
						return;
					}
				} else if (e is ConflictException) {
					// Registration failed because the user already exists
					applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Registration failed because the user already exists");
					navigationController.GoTo ("ALERT_VIEW");
					return;
				} else if (e is CloudException) {
					// Registration failed
					applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Registration failed");
					navigationController.GoTo ("ALERT_VIEW");
					return;
				} else if (e != null) {
					// Unexpected exception occurred
					applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Unexpected exception occurred");
					navigationController.GoTo ("ALERT_VIEW");
					return;
				}
				// Set some custom fields.
				Debug.Log ("GoTo My Games");
				navigationController.GoTo ("GAMES_VIEW");
			});
		}
	}

	public void Back(){
		Debug.Log ("Back");
		navigationController.Back();
	}

	public bool ValidateEmail(string email){
		var regex = new Regex(@"[a-z0-9!#$%&amp;'*+/=?^_`{|}~-]+(?:.[a-z0-9!#$%&amp;'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
		return regex.IsMatch(email);
	}
	
}
