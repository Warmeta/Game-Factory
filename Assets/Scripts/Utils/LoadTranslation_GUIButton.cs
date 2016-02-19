using UnityEngine;
using System.Collections;
 
public class LoadTranslation_GUIButton : MonoBehaviour {
    public string textInKeyLanguage="Language";
	public delegate void ChangeLanguageDelegate(int lang);
	public static ChangeLanguageDelegate ChangeLanguage;
    string textBoton;
     
    public void Start(){
        textBoton = textInKeyLanguage;
        LoadTranslation(ApplicationController.GameLanguage);
    }
     
    public void LoadTranslation(int lang){
        textBoton = LanguageManager.getTextInLanguage(textInKeyLanguage, lang);
    }
     
    public void OnGUI(){
        if(GUI.Button(new Rect(5,Screen.height-40,80,35),textBoton)){
			ApplicationController.GameLanguage = ApplicationController.GameLanguage==1?0:1;
			if(ChangeLanguage!=null){ 
				ChangeLanguage(ApplicationController.GameLanguage);
			}
        }
    }
	
	/*Cuando el objeto se active, enlace la funcion de traduccion*/
	public void OnEnable(){
		LoadTranslation_GUIButton.ChangeLanguage += LoadTranslation;
	}
	 
	/*Cuando el objeto se desactive, desenlace la funcion de traduccion*/  
	public void OnDisable(){
		LoadTranslation_GUIButton.ChangeLanguage -= LoadTranslation;
	}
}