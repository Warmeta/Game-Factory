using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ApplicationController : MonoBehaviour {
	
	public const string gameObjectId = "AppController";	
	//public GoogleAnalyticsV3 googleAnalytics;
	public NavigationController 			navigationController;// 		{ get; public set; }
	public GameObject alertView;
	public bool sessionStarted = false;
	public static int GameLanguage = 0;		//{ get; public set; }
	//public UserController                	userController		        { get; private set; }
	//public LocationController            	locationController 			{ get; private set; }
	//public FacebookManager               	facebookManager 			{ get; private set; }
	//public SignInWithFacebookController  	signInWithFacebookController{ get; private set; }
	//public TextureDownloader             	textureDownloader 			{ get; private set; }
	//public PlantillaWebServiceClient     	webServiceClient            { get; private set; }
	//public RequestInterface              	requestService             	{ get; private set; }
	//public PersistentLocationController  	persistentLocationController{ get; private set; }
	//public ShopController                	shopController 	            { get; private set; }
	//public AchievementController         	achievementController       { get; private set; }
	//public RequestQueueController        	requestQueueController      { get; private set; }
	//public ResponseParser					responseParser				{ get; private set; }
	
	private static ApplicationController instance = null;

	public static ApplicationController GetInstance()
	{	
		return	instance;
	}
	
	void Awake ()
	{
		instance = this;
		GetInstance();
		
		//googleAnalytics.StartSession();

		//navigationController      		= this.gameObject.AddComponent<NavigationController>();
		//facebookManager           		= this.gameObject.AddComponent<FacebookManager>();
		//locationController        		= this.gameObject.AddComponent<Singular.LocationController>();
		//webServiceClient          		= this.gameObject.AddComponent<Singular.Plantilla.PlantillaWebServiceClient>();
		//requestService            		= this.gameObject.AddComponent<Singular.KiiService>();
		//shopController            		= this.gameObject.AddComponent<ShopController>();
		//achievementController     		= this.gameObject.AddComponent<AchievementController>();
		//requestQueueController    		= this.gameObject.AddComponent<RequestQueueController>();
		//userController            		= this.gameObject.AddComponent<UserController>();
		//signInWithFacebookController 	= this.gameObject.AddComponent<SignInWithFacebookController>();
		//persistentLocationController 	= this.gameObject.AddComponent<Singular.PersistentLocationController>();
		//textureDownloader         		= new TextureDownloader();
		//responseParser					= new KiiResponseParser();
		
		////component dependencies. VERY IMPORTANT!!!!
		//requestQueueController.requestService = requestService;
		//userController.requestService = requestService;
		//userController.requestQueuController = requestQueueController;
		
		//shopController.requestService = requestService;
		//shopController.requestQueueController = requestQueueController;
		//shopController.userController = userController;
		//shopController.responseParser = responseParser;
		
		//persistentLocationController.appControllerGameObjectId = gameObjectId;
		
		//achievementController.requestService = requestService;
		//achievementController.requestQueueController = requestQueueController;
		//achievementController.userController = userController;
		//achievementController.responseParser = responseParser;
		
		//signInWithFacebookController.userController = userController;
		//signInWithFacebookController.facebookManager = facebookManager;
		//signInWithFacebookController.requestService = requestService;
		//signInWithFacebookController.requestQueueController = requestQueueController;
		//signInWithFacebookController.responseParser = responseParser;

		//avoid the object destruction
		DontDestroyOnLoad(this.gameObject);
		
		LanguageManager.LoadLanguagesFile("Languages");
		
		// IMPORTANT!!! DO NOT TOUCH!!! Configuration var to serialize object in Ios Projects.
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
	}
	
	
	void Start()
	{
		//User currentUser = userController.GetCurrentUser();
		
		//if (currentUser != null){
		//	userController.SignIn(currentUser, this);
		//	
		//} else {
		//	Debug.Log("AppController GetCurrentUser user is null.");
		//}
		
		////Debug.Log("Application.temporaryCachePath: " + Application.temporaryCachePath);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
