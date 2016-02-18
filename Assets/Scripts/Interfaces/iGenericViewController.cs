using UnityEngine;
using System.Collections;

public interface iGenericViewController {

	ViewTypes viewType {get;set;}
	string viewName {get;set;}

	void Show(bool value = true, string previousScreenPath = "");
	void Refresh();
	void PopCallback();
	
}
