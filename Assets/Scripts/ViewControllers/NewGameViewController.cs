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
				instantiatedPrefab.GetComponent<Button> ().onClick.AddListener (() => GetButtonIndex(instantiatedPrefab));
				instantiatedPrefab.GetComponent<Button> ().onClick.AddListener (() => OpenScreenshot ());
				buttonList.Add(instantiatedPrefab);
			}
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

	public void Save(){
		bool validate = true;
		KiiBucket appBucket = Kii.Bucket("Games");
		KiiClause clause = KiiClause.Equals("gameName", gameName.GetComponent<InputField>().text);
		string bucketName = gameName.GetComponent<InputField>().text+"-Screenshots-";
		KiiBucket screenshotBucket = Kii.Bucket(bucketName);
		KiiQuery query = new KiiQuery(clause);
		string userId = KiiUser.CurrentUser.ID;
		KiiObject obj = Kii.Bucket("Games").NewKiiObject();

		obj["gameName"] = gameName.GetComponent<InputField>().text;
		obj["gameDescription"] = gameDescription.GetComponent<InputField>().text;
		obj["gameCategories"] = gameCategories.GetComponent<InputField>().text;
		obj["gameKeywords"] = gameKeywords.GetComponent<InputField>().text;
		obj["gameEula"] = gameEula.GetComponent<InputField>().text;
		obj ["gamePlatform"] = gamePlatform.GetComponent<Text> ().text;
		obj ["gameLanguage"] = gameLanguage.GetComponent<Text> ().text;
		obj["userId"] = userId;

		appBucket.Count(query, (KiiBucket b, KiiQuery q, int count, Exception e) => {
			if (e != null){
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
					};
					Debug.Log ("SavingImages.");
					SaveImage(0, obj);
				});
			}
		});
		for (int i = 1; i < imagePaths.Length; i++){
			SaveImages(bucketName, i);
		}
		Debug.Log ("ImagesSaved: GoTo Edit");
		//navigationController.GoTo ("EDIT_VIEW");
	}

	public void OpenIcon () {
		try {
			string filePath = EditorUtility.OpenFilePanel (
				"Select your image:",
				"",
				"jpg");
			if (filePath != "") {
				gameIcon.GetComponentInChildren<RawImage> ().texture = LoadImage (filePath);
				imagePaths [0] = filePath;
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
			screensList.GetComponentsInChildren<Button> ()[indexIs].GetComponentInChildren<RawImage> ().texture = LoadImage (filePath);
			imagePaths [indexIs+1] = filePath;
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

	public void SaveImages(string bucketName, int i){
		if (imagePaths[i] != null){
			KiiObject obj = Kii.Bucket (bucketName).NewKiiObject ();
			// Set key-value pairs
			obj ["screenshotName"] = "screenshot-0"+i;
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
		Texture2D tex = null;
		tex = new Texture2D (2, 2);
		tex.LoadImage (ReadImage(filePath)); //..this will auto-resize the texture dimensions.
		TextureScale.Bilinear (tex, 200, 200);
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

	public 

	void GetButtonIndex(GameObject ob){
		indexIs = FindButtonIndex(ob);
		Debug.Log ("My id is "+indexIs);
		Debug.Log ("Size item list is "+buttonList.Count);
	}

	public void Cancel(){
		Debug.Log ("Cancel");
		navigationController.Back();
	}
	
	
	
}
