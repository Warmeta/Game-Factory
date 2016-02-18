using UnityEngine;
using System.Collections;

public enum ViewTypes{CONTENT_PANEL,WHOLE_WINDOW,SCENE,CONTENT_SEQUENCE}

public abstract class GenericViewController : MonoBehaviour {
	
	public ViewTypes	viewType {get;set;}
	public string		viewName {get;set;}

	protected ApplicationController applicationController = null;
	protected NavigationController  navigationController = null;
	protected bool isShown = true;


	void Awake () {
	}

	public virtual void Initialize() {

		viewType = ViewTypes.CONTENT_PANEL;
		if (applicationController == null) {
			applicationController = ApplicationController.GetInstance();
		}
		
		if (navigationController == null) {
			navigationController = applicationController.navigationController;
		}
	}
	
	void Start () {
	
	}

	public virtual void Show(bool value = true, string previousScreenPath = "") {
		isShown = value;
	}

	public virtual bool IsShown(){
		return isShown;
	}

	public virtual void Refresh() {
		
	}

	public virtual void PopCallback() {

	}
}
