using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using KiiCorp.Cloud.Storage;
using UnityEngine.EventSystems;

public class EditViewController : GenericViewController {

	private Uri uri;
	public GameObject editButtonPanel;
	public GameObject editMissionPanel;
	public GameObject missionBox;
	public GameObject gameTitle;
	
	void Awake () {
		Initialize();
		viewName = "EDIT_VIEW";
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

	void OnEnable(){
		if (applicationController.sessionStarted) {
			/*//bool validate = true;
		Uri objUri = new Uri(uri);
		KiiBucket bucketGames = Kii.Bucket("Games");
		KiiBucket bucketMission = Kii.Bucket("Mission");
		KiiBucket bucketMissionList = Kii.Bucket("MissionList");
		KiiObject objGame = KiiObject.CreateByUri(objUri);
		KiiObject obj = Kii.Bucket("Mission").NewKiiObject();
		//KiiClause clause = KiiClause.Equals("gameName", gameName.GetComponent<InputField>().text);
		//KiiQuery query = new KiiQuery(clause);
		//string userId = KiiUser.CurrentUser.ID;
		
		objGame ["missionName"] = "";
		objGame ["background"] = "";
		objGame ["audio"] = "";
		objGame ["chrono"] = "";
		objGame ["missionType"] = "";
		objGame ["missionOrder"] = "";
		
		
		objGame.SaveAllFields(true, (KiiObject updatedObj, Exception e) => {
			if (e != null)
			{
				// handle error
			}
		});*/
			KiiObject obj = KiiObject.CreateByUri (new Uri (uri.ToString ()));
			obj.Refresh ((KiiObject refreshedObj, Exception e) => {
				if (e != null) {
					// handle error
					return;
				}
				// Get key-value pairs.
				gameTitle.GetComponentInChildren<Text> ().text = (string)refreshedObj ["gameName"];
			});
		}

		/*for (int i=0; i < 4; i++) {
			GameObject box;
			box = Instantiate(missionBox);
			box.SetActive (true);
			box.transform.SetParent (missionBox.transform);
			box.transform.localScale = new Vector3 (1, 1, 1);
			box.GetComponentsInChildren<Text> () [0].text = "";
			gameBoxI.GetComponentInChildren<Button>().onPointerEnter.AddListener(() => editViewController.GetComponent<EditViewController>().SetUri(obj.Uri));
			gameBoxI.GetComponentInChildren<Button>().onClick.AddListener(() => navigationController.GoTo("EDIT_VIEW"));
			//gameBox.GetComponentInChildren<RawImage>().texture = obj["icon"];
		}*/
	}

	void OnDisable(){
	}

	public void ModifyGame(){
		navigationController.GoTo("NEWGAME_VIEW");
	}

	public void ModifyMission(){
		navigationController.GoTo("EDITMISSION_VIEW");
	}

	public void Back(){
		Debug.Log ("Back");
		navigationController.Back();
	}
	
	public void AddMission(){
		navigationController.GoTo("EDITMISSION_VIEW");
	}
	
	public void AlertMission(){
		// mensajes alerta
	}
	
	public void DeleteMission(){
		applicationController.alertView.GetComponent<AlertViewController>().SetAlert("Are you sure?");
		navigationController.GoTo("ALERT_VIEW");
	}
	
	public void ScrollList(){
		// mostrar scroll list de misiones
	}

	public void SetUri(Uri uri){
		this.uri = uri;

	}

	public void HoverPanel(){
	}

}
