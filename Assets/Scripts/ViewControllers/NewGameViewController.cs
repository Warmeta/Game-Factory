using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using UnityEngine.UI;
using System;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class NewGameViewController : GenericViewController {

	public GameObject gameName;
	public GameObject gameDescription;
	public GameObject gameIcon;
	public GameObject gamePlatform;
	public GameObject gameLanguage;
	public GameObject gameKeywords;
	public GameObject gameCategories;
	public GameObject gameEula;
	public GameObject screensList;
	public GameObject screenshotBox;
	public List<GameObject> buttonList = new List<GameObject>();
	private Texture2D load_texture;
	private string[] imagePaths = new string[5];
	private int indexIs;
	private Uri objUri;

	void Awake () {
		Initialize();
		viewName = "NEWGAME_VIEW";
		viewType = ViewTypes.CONTENT_SEQUENCE;
	}

	void start(){

	}

	void OnEnable(){
		if (applicationController.sessionStarted) {
			for (int i=0; i<4; i++) {
				GameObject instantiatedPrefab;
				instantiatedPrefab = Instantiate (screenshotBox) as GameObject;
				instantiatedPrefab.SetActive (true);
				instantiatedPrefab.transform.SetParent (screensList.transform);
				instantiatedPrefab.transform.localScale = new Vector3 (1, 1, 1);
				instantiatedPrefab.GetComponent<Button> ().onClick.AddListener (() => GetButtonIndex (instantiatedPrefab));
				instantiatedPrefab.GetComponent<Button> ().onClick.AddListener (() => OpenScreenshot ());
				buttonList.Add (instantiatedPrefab);
			}
			bool validate = true;
			KiiBucket appBucket = Kii.Bucket ("Games");
			//string gameNameTrim = gameName.GetComponent<InputField> ().text.Replace (" ", String.Empty);
			/*KiiQuery query = new KiiQuery(
			KiiClause.And(
			KiiClause.Equals("gameName", gameName.GetComponent<InputField>().text)
			,KiiClause.Equals("bucketScreenshotsId", gameNameTrim)
			)
			);*/
			//string bucketName = gameNameTrim+"-Screenshots-";
			string userId = KiiUser.CurrentUser.ID;
			KiiObject obj = Kii.Bucket ("Games").NewKiiObject ();
			String id = "";
			try {
				obj.Refresh ();
				id = obj.GetString ("_id");
			} catch (Exception e) {
				Debug.Log ("Fallo refresh id game: " + e);
			}

			/*obj["gameName"] = gameName.GetComponent<InputField>().text;
		obj["gameDescription"] = gameDescription.GetComponent<InputField>().text;
		obj["gameCategories"] = gameCategories.GetComponent<InputField>().text;
		obj["gameKeywords"] = gameKeywords.GetComponent<InputField>().text;
		obj["gameEula"] = gameEula.GetComponent<InputField>().text;
		obj["gamePlatform"] = gamePlatform.GetComponent<Text> ().text;
		obj["gameLanguage"] = gameLanguage.GetComponent<Text> ().text;
		obj["userId"] = userId;
		obj["bucketScreenshotsId"] = gameNameTrim;*/
		
			//obj["GameBucket"] = userId;
		
			//if(ValidatedFields()){
			/*appBucket.Count(query, (KiiBucket b, KiiQuery q, int count, Exception e) => {
			if (e != null){
				validate = false;
				applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Unexpected exception occurred");
				navigationController.GoTo ("ALERT_VIEW");
				return;
			}
			if (count > 0){
				validate = false;
				applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Game name already exist");
				navigationController.GoTo ("ALERT_VIEW");
				return;
			}*/
			if (validate) {
				// Save the object
				obj.SaveAllFields (true, (KiiObject savedObj, Exception exc) => {
					if (exc is NetworkException) {
						if (((NetworkException)exc).InnerException is TimeoutException) {
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
					} else if (exc is ConflictException) {
						// Registration failed because the user already exists
						applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Registration failed because the game name already exists");
						navigationController.GoTo ("ALERT_VIEW");
						return;
					} else if (exc is CloudException) {
						// Registration failed
						applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Registration failed");
						navigationController.GoTo ("ALERT_VIEW");
						return;
					} else if (exc != null) {
						// Unexpected exception occurred
						applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Unexpected exception occurred");
						navigationController.GoTo ("ALERT_VIEW");
						return;
					}
					;
					objUri = obj.Uri;
					Debug.Log ("mi uri es:" + objUri);
					/*Debug.Log ("SavingImages.");
					SaveImage(0, obj);*/
				});
			}

			//});
			//Check whether the id is valid.
			/*for (int i = 1; i < imagePaths.Length; i++){
			SaveImages(bucketName, i);
		}
		Debug.Log ("ImagesSaved: GoTo Edit");*/
			//navigationController.GoTo ("EDIT_VIEW");
			//}
		}
	}

	void OnDisable(){
		if (applicationController.sessionStarted) {
			for (int i = 0; i < screensList.transform.childCount; i++) {
				Destroy (screensList.transform.GetChild (i).gameObject);
			}
			buttonList.Clear();
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

	public void Save(String field, String value){
		bool validate = true;
		KiiObject obj = KiiObject.CreateByUri(objUri);
		//string gameNameTrim = gameName.GetComponent<InputField> ().text.Replace (" ", String.Empty);
		//

		//string bucketName = gameNameTrim+"-Game-";

		obj.Refresh((KiiObject obj2, Exception ex) => {
		if (ex != null)
		{
			// handle error
			Debug.Log("Error en Refresh obj.");
			return;
		}

		obj[field] = value;
		/*obj["gameDescription"] = gameDescription.GetComponent<InputField>().text;
		obj["gameCategories"] = gameCategories.GetComponent<InputField>().text;
		obj["gameKeywords"] = gameKeywords.GetComponent<InputField>().text;
		obj["gameEula"] = gameEula.GetComponent<InputField>().text;
		obj["gamePlatform"] = gamePlatform.GetComponent<Text> ().text;
		obj["gameLanguage"] = gameLanguage.GetComponent<Text> ().text;
		obj["bucketScreenshotsId"] = gameNameTrim;*/

		if (validate) {
			// Save the object
			obj2.Save((KiiObject updatedObj, Exception exc) => {
				if (exc is NetworkException) {
					if (((NetworkException)exc).InnerException is TimeoutException) {
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
				} else if (exc is ConflictException) {
					// Registration failed because the user already exists
					applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Registration failed because the game name already exists");
					navigationController.GoTo ("ALERT_VIEW");
					return;
				} else if (exc is CloudException) {
					// Registration failed
					applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Registration failed");
					navigationController.GoTo ("ALERT_VIEW");
					return;
				} else if (exc != null) {
					// Unexpected exception occurred
					applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Unexpected exception occurred");
					navigationController.GoTo ("ALERT_VIEW");
					return;
				};
			});
		}
		//Check whether the id is valid.
		/*for (int i = 1; i < imagePaths.Length; i++){
			SaveImages(bucketName, i);
		}
		Debug.Log ("ImagesSaved");*/
		//navigationController.GoTo ("EDIT_VIEW");
		});
	}

	public void SaveName(){
		Save("gameName", gameName.GetComponent<InputField>().text);
	}

	public void SaveDescription(){
		Save("gameName", gameName.GetComponent<InputField>().text);
	}

	public void SaveCategories(){
		Save("gameDescription", gameDescription.GetComponent<InputField>().text);
	}

	public void SaveKeywords(){
		Save("gameKeywords", gameKeywords.GetComponent<InputField>().text);
	}

	public void SaveEula(){
		Save("gameEula", gameEula.GetComponent<InputField>().text);
	}

	public void SaveDropdowns(){
		Save("gameLang", gameLanguage.GetComponent<Text> ().text);
		Save("gamePlat", gamePlatform.GetComponent<Text> ().text);
	}

	public void OpenIcon () {
		try {
			string filePath = EditorUtility.OpenFilePanel (
				"Select your image:",
				"",
				"jpg");
			if (filePath != "") {
				KiiObject obj = KiiObject.CreateByUri(objUri);
				gameIcon.GetComponentInChildren<RawImage> ().texture = LoadImage (filePath);
				imagePaths [0] = filePath;
				Debug.Log ("SavingImages.");
				SaveImage(0, obj);
			}
		} catch(System.SystemException e) {
			Debug.Log (e);
		}
	}

	public void OpenScreenshot () {
		try {
			string filePath = EditorUtility.OpenFilePanel (
			"Select your image:",
			"",
			"jpg");
			if (filePath != "") {
				screensList.GetComponentsInChildren<Button> ()[indexIs].GetComponentInChildren<RawImage> ().texture = LoadImage (filePath);
				imagePaths [indexIs+1] = filePath;
			}
		} catch(System.SystemException e) {
			Debug.Log (e);
		}
	}

	public void SaveImage(int n, KiiObject obj){
		if (imagePaths[n] != null){
			//FileStream file = new FileStream(imagePaths[n], FileMode.Open); // Start uploading 
			byte[] imageToBytes = LoadImage (imagePaths [n]).EncodeToPNG();
			MemoryStream stream = new MemoryStream(imageToBytes);
			obj.UploadBody("image/icon", stream, (KiiObject uploadedObj, Exception e2) => { if (e2 != null) { // Handle error 
					return; 
				} Debug.Log ("Uploaded image"+n);
				}, (KiiObject uploadingObj, float progress) => { 
					Debug.Log("##### Uploading image"+n+"..."+progress*100);
					//applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Uploading... " + (progress * 100) + "%");
					//navigationController.GoTo ("ALERT_VIEW");
					return;
			});
		}
	}

	public void SaveImages(string idGame, int i){
		if (imagePaths[i] != null){
			KiiObject obj = Kii.Bucket (idGame).NewKiiObject ();
			// Set key-value pairs
			obj ["screenshotName"] = "screenshot-0"+i;
			obj ["idGame"] = idGame;
			obj.SaveAllFields (true, (KiiObject savedObj, Exception exc) => {
				if (exc != null){
					Debug.Log (exc);
					return;
				}
				// Prepare file to upload
				SaveImage (i, savedObj);
			});
		}
	}
		
	// return image in Texture2D from directory
	public Texture2D LoadImage(string filePath) {
		int[] widthHeightResize;
		Texture2D tex = new Texture2D (1, 1);
		tex.LoadImage (ReadImage(filePath)); //..this will auto-resize the texture dimensions.
		Debug.Log("Raw size: "+tex.width+" x "+tex.height);
		widthHeightResize = CalculateAspectRatio (tex.width, tex.height, 400);
		Debug.Log("Resized size: "+widthHeightResize[0]+" x "+widthHeightResize[1]);
		TextureScale.Bilinear (tex, widthHeightResize[0], widthHeightResize[1]);
		return tex;
	}
	
	// return image in bytes from directory.
	private byte[] ReadImage (string filePath)
	{
		byte[] fileData;
		fileData = File.ReadAllBytes (filePath);
		if (fileData == null) {
			Debug.Log ("#####Failed to read image from file.");
		} else {
			Debug.Log ("#####Load image " + fileData.Length + "bytes");
		}
		return fileData;
	}

	public int FindButtonIndex(GameObject go){
		int foundButton = -1;
		for (int i= 0; i < buttonList.Count; i++) {
			if(go == buttonList[i]){
				foundButton = i;
				break;
			}
		}
		return foundButton;
	}

	public void GetButtonIndex(GameObject ob){
		indexIs = FindButtonIndex(ob);
		Debug.Log ("My id is "+indexIs);
		Debug.Log ("Size item list is "+buttonList.Count);
	}

	public Boolean ValidatedFields(){
		bool validate = true;
		int sGameName = gameName.GetComponent<InputField> ().text.Length;
		int sGameDescription = gameDescription.GetComponent<InputField> ().text.Length;
		int sGameCategories = gameCategories.GetComponent<InputField> ().text.Length;
		int sGameKeywords = gameKeywords.GetComponent<InputField> ().text.Length;
		int sGameEula = gameEula.GetComponent<InputField> ().text.Length;
		//int sGamePlatform = gamePlatform.GetComponent<Text> ().text.Length;
		//int sGameLanguage = gameLanguage.GetComponent<Text> ().text.Length;
		if (sGameName < 4 || sGameName > 18) {
			validate = false;
			//Set error
			Debug.Log ("Game name long is not beetween 4 and 18 characters.");
		}
		if (sGameDescription < 5 || sGameDescription > 50) {
			validate = false;
			//Set error
			Debug.Log ("Game description long is not beetween 5 and 50 characters.");
		}
		if (sGameCategories < 5 || sGameCategories > 20) {
			validate = false;
			//Set error
			Debug.Log ("Game categories long is not beetween 5 and 20 characters.");
		}
		if (sGameKeywords < 5 || sGameKeywords > 20) {
			validate = false;
			//Set error
			Debug.Log ("Game keywords long is not beetween 5 and 20 characters.");
		}
		if (sGameEula < 5 || sGameEula > 50) {
			validate = false;
			//Set error
			Debug.Log ("Game eula long is not beetween 5 and 50 characters.");
		}
		return validate;
	}

	public int[] CalculateAspectRatio(int width, int height, int max){
		//calculate aspect ratio
		int[] whValues = {width,height};
		
		//calculate new dimensions based on aspect ratio
		while (whValues[0] > max || whValues[1] > max) {
			(int)whValues [0] /= 2;
			(int)whValues [1] /= 2;
		}
		return whValues;
	}
	
	public void Cancel(){
		Debug.Log ("Cancel");
		bool validate = true;
		KiiObject obj = KiiObject.CreateByUri(objUri);
	  	KiiBucket appBucket = Kii.Bucket("Games");
	 	String id = "";
	 	try {
			obj.Refresh();
			id = obj.GetString("_id");
		} catch (Exception e) {
			Debug.Log("Fallo refresh id game: "+e);
		}
		KiiQuery query = new KiiQuery(
			KiiClause.And(
				KiiClause.Equals("gameName", gameName.GetComponent<InputField>().text)
				,KiiClause.NotEquals("_id", id)
			)
		);
		appBucket.Count(query, (KiiBucket b, KiiQuery q, int count, Exception e) => {
		if (e != null){
			validate = false;
			applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Unexpected exception occurred");
			navigationController.GoTo ("ALERT_VIEW");
			return;
		}
		if (count > 0){
			validate = false;
			applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Game name already exist");
			navigationController.GoTo ("ALERT_VIEW");
			return;
		}
			if (ValidatedFields() && validate) {
				navigationController.Back ();
			} else {
				applicationController.alertView.GetComponent<AlertViewController> ().SetAlert ("Invalid inputs.");
				navigationController.GoTo ("ALERT_VIEW");
			}
		});
	}
	
	
	
}
