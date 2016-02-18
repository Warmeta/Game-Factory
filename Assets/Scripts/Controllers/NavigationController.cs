using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavigationController : MonoBehaviour {
	
	//public GoogleAnalyticsV3 googleAnalytics;
	public GameObject mainPanel;
	public GameObject defaultView;
	public GameObject currentView;
	public GameObject[] viewList;
	private Stack<string> navigationStack = new Stack<string>();
	private Dictionary<string, GameObject> allViewsDictionary = new Dictionary<string, GameObject>();
	private Dictionary<string, GameObject> contentViewDictionary = new Dictionary<string, GameObject>();
	private string previousViewName = "";

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
	
	void Start () {
		RegisterViews();
		GoToRoot();
		PrintStack();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Back();
		}
	}

	void PrintStack() {
		Debug.Log("STACK START");
		foreach (string item in navigationStack) {
			Debug.Log ("Stack: " + item);
		}
		Debug.Log("STACK END");
	}

	void RegisterViews() {

		if (allViewsDictionary.Count == 0) {
			for (int iter = 0; iter < viewList.Length; iter++) {
				GenericViewController viewController = viewList[iter].GetComponent("GenericViewController") as GenericViewController;
				allViewsDictionary.Add(viewController.viewName, viewList[iter]);
				//viewController.googleAnalytics = googleAnalytics;
				if (viewController.viewType == ViewTypes.CONTENT_PANEL ||
				    viewController.viewType == ViewTypes.CONTENT_SEQUENCE) {
					contentViewDictionary.Add(viewController.viewName, viewList[iter]);
				}
			}
		}
	}

	public void GoToRoot() { 
		
		for (int iter = 0; iter < viewList.Length; iter++) {
			GenericViewController viewController = viewList[iter].GetComponent("GenericViewController") as GenericViewController;
			viewList[iter].SetActive(false);
		}

		string previousViewName = currentView.GetComponent<GenericViewController>().viewName;
		currentView = defaultView;
		GenericViewController currentViewController = defaultView.GetComponent<GenericViewController>();
		navigationStack.Clear();
		navigationStack.Push(currentViewController.viewName);
		currentView.SetActive(true);
		currentViewController.Show(true, previousViewName);
	}

	public void ShowMainPanel(bool value = true) {
		
		mainPanel.GetComponent<MainPanelViewController>().Show(value);
		
	}
	
	public void ShowLeftMenu(bool value = true) {
		
		mainPanel.GetComponent<MainPanelViewController>().Show(!value);
		
	}

	public void GoTo(string viewName) {
		
		GenericViewController currentViewController = currentView.GetComponent<GenericViewController>();
		GenericViewController nextViewController = null;
		GameObject nextViewGameObject = null;
		
		if ( (allViewsDictionary.ContainsKey(viewName)) && 
		    (currentViewController.viewName != viewName)) {

			previousViewName = currentViewController.viewName;

			nextViewGameObject = allViewsDictionary[viewName];
			nextViewGameObject.SetActive(true);
			nextViewController = nextViewGameObject.GetComponent<GenericViewController>();

			//Comes from
			if ( (currentViewController.viewType == ViewTypes.WHOLE_WINDOW) ||
			    (nextViewController.viewType    == ViewTypes.CONTENT_PANEL &&
			 	currentViewController.viewType == ViewTypes.CONTENT_PANEL)) {
				Pop();
				currentViewController.Show(false);
			}
			else if (nextViewController.viewType    == ViewTypes.CONTENT_SEQUENCE &&
			         (currentViewController.viewType == ViewTypes.CONTENT_PANEL ||
			 		  currentViewController.viewType == ViewTypes.CONTENT_SEQUENCE)) {
				currentViewController.Show(false);
			}
			else if (currentViewController.viewType == ViewTypes.CONTENT_SEQUENCE  &&
			         nextViewController.viewType    == ViewTypes.CONTENT_PANEL) {
				WipeOutSequence();
			}

			//Goes to
			ShowViewPanel(viewName);
			currentView = allViewsDictionary[viewName]; 
			if (!(navigationStack.Count >0 && navigationStack.Peek() == viewName)) {
				navigationStack.Push(viewName);
			}
		}
		else {
			//GoTo same view = Do nothing or refresh
			//currentView.GetComponent<GenericViewController>().Refresh();
		}
		
		PrintStack();
	}

	private void WipeOutSequence() {
		if (currentView != null) {
			GenericViewController currentViewController = currentView.GetComponent<GenericViewController>();
			while (navigationStack.Count > 1 && currentViewController.viewType == ViewTypes.CONTENT_SEQUENCE)  {
				Pop();
				currentViewController.Show(false);
				if (allViewsDictionary.ContainsKey(navigationStack.Peek())) {
					currentView = allViewsDictionary[navigationStack.Peek()];
					currentViewController = currentView.GetComponent<GenericViewController>();
				}
			}
			Pop();
			currentViewController.Show(false);
			if (navigationStack.Count > 0){
				if (allViewsDictionary.ContainsKey(navigationStack.Peek())) {
					currentView = allViewsDictionary[navigationStack.Peek()];
					currentViewController = currentView.GetComponent<GenericViewController>();
				}
			}
			else {
				currentView = null;
				currentViewController = null;
			}
		}
	}
	
	public void ShowViewPanel(string value) {

		if (allViewsDictionary.ContainsKey(value)) {
			GameObject viewGameObject = allViewsDictionary[value];
			GenericViewController viewController = viewGameObject.GetComponent<GenericViewController>();
			if (viewController.viewType == ViewTypes.CONTENT_PANEL){
				ShowContentViewPanel(value);
			}
			else if (viewController.viewType == ViewTypes.SCENE) {
				Application.LoadLevel(value);
			}
			else {
				viewGameObject.SetActive(true);
				viewController.Show(true, previousViewName);
			}
			currentView = allViewsDictionary[value];
		}
	}
	
	public void ShowContentViewPanel(string value) {
		if (allViewsDictionary.ContainsKey(value)) {
			if (mainPanel != null) { 
				ShowMainPanel(true);
			}
			GameObject viewPanel = allViewsDictionary[value];
			foreach (KeyValuePair<string, GameObject> iter in contentViewDictionary) {
				if (iter.Value.activeSelf) {
					//Debug.Log("ShowContentViewPanel iter is active");
					//iter.Value.GetComponent<GenericViewController>().Show(false);
				}
			}
			viewPanel.SetActive(true);
			viewPanel.GetComponent<GenericViewController>().Show(true, previousViewName);
		}
	}

	private void Pop() {
		if (navigationStack.Count >0) {
			currentView.GetComponent<GenericViewController>().PopCallback();
			navigationStack.Pop();
		}
	}
	
	public void Back() {

		if (navigationStack.Count > 1) {

			Pop();

			BackFromCommonPanel();

			string peekedViewName = navigationStack.Peek();
			if (allViewsDictionary.ContainsKey(peekedViewName)) {
				currentView = allViewsDictionary[peekedViewName];
			}

		}
		else {
			//Do nothing or refresh
		}

		PrintStack();
	}

	private void BackFromLeftPanel() {
		if (mainPanel != null) {
			ShowMainPanel(true);
		}
	}

	private void BackFromCommonPanel() {
		currentView.GetComponent<GenericViewController>().Show(false);
		string PeekViewName = navigationStack.Peek();
		ShowViewPanel(PeekViewName);
	}
}
