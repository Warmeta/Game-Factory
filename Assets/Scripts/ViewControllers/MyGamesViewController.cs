using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;
using UnityEngine.UI;

public class MyGamesViewController : GenericViewController {
	
	public GameObject gameBox;
	public GameObject container;
	public GameObject editViewController;
	
	void Awake () {
		Initialize();
		viewName = "GAMES_VIEW";
		viewType = ViewTypes.CONTENT_SEQUENCE;
	}

	void OnEnable(){
		if (applicationController.sessionStarted) {
			KiiBucket appBucket = Kii.Bucket ("Games");
			KiiClause clause = KiiClause.Equals ("userId", KiiUser.CurrentUser.ID);
			KiiQuery query = new KiiQuery (clause);
		
			appBucket.Query (query, (KiiQueryResult<KiiObject> result, Exception e) => {
				if (e != null) {
					applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Unexpected exception occurred");
					navigationController.GoTo ("ALERT_VIEW");
					return;
				}
				foreach (KiiObject obj in result) {
					GameObject gameBoxI;
					gameBoxI = Instantiate (gameBox);
					gameBoxI.SetActive (true);
					gameBoxI.transform.SetParent (container.transform);
					gameBoxI.transform.localScale = new Vector3 (1, 1, 1);
					gameBoxI.GetComponentsInChildren<Text> () [0].text = obj.GetString ("gameName");
					gameBoxI.GetComponentInChildren<Button>().onClick.AddListener(() => editViewController.GetComponent<EditViewController>().SetUri(obj.Uri));
					gameBoxI.GetComponentInChildren<Button>().onClick.AddListener(() => navigationController.GoTo("EDIT_VIEW"));
					//gameBox.GetComponentInChildren<RawImage>().texture = obj["icon"];
				}
			});
		}
	}

	void OnDisable(){
		if (applicationController.sessionStarted) {
			for (int i = 1; i < container.transform.childCount; i++) {
				Destroy (container.transform.GetChild (i).gameObject);
			}
		}
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

	public void NewGame(){
		Debug.Log ("GoTo New Game");
		navigationController.GoTo("NEWGAME_VIEW");
	}

	public void Edit(){
		//Transformar boton en Continuar
	}
	
	public void Continue(){
		Debug.Log ("GoTo Edit ");
		navigationController.GoTo("EDIT_VIEW");
	}
	
	public void Back(){
		Debug.Log ("Back");
		navigationController.GoTo("LOGIN_VIEW");;
	}
	
}
