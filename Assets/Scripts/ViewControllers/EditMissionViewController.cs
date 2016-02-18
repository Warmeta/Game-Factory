using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

public class EditMissionViewController : GenericViewController {
	
	void Awake () {
		Initialize();
		viewName = "EDITMISSION_VIEW";
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
		/*Uri objUri = new Uri(uri);
		KiiBucket bucketGames = Kii.Bucket("Games");
		KiiBucket bucketMission = Kii.Bucket("Mission");
		KiiBucket bucketMissionList = Kii.Bucket("MissionList");
		KiiObject objGame = KiiObject.CreateByUri(objUri);
		KiiObject obj = Kii.Bucket("Mission").NewKiiObject();
		//KiiClause clause = KiiClause.Equals("gameName", gameName.GetComponent<InputField>().text);
		//KiiQuery query = new KiiQuery(clause);
		//string userId = KiiUser.CurrentUser.ID;
		
		obj ["missionName"] = "";
		obj ["background"] = "";
		obj ["audio"] = "";
		obj ["chrono"] = "";
		obj ["missionType"] = "";
		obj ["missionOrder"] = "";
		
		
		obj.SaveAllFields(true, (KiiObject updatedObj, Exception e) => {
			if (e != null)
			{
				// handle error
			}
		});*/
	}

	void OnDisable(){
	}

	public void SaveMission(){

	}

	public void NameMission(){
		// Guardar nombre de mision
	}

	public void BackgroundMission(){
		// Guardar fondo mision
	}
	
	public void AudioMission(){
		// Guardar musica mision
	}
	
	public void CronoMission(){
		// Establecer crono de mision
	}
	
	public void TypeMission(){
		// Establecer tipo de mision
	}
	
	public void OrderMission(){
		// Establecer orden de mision
	}

	public void Back(){
		Debug.Log ("Back");
		navigationController.Back();
	}
}
