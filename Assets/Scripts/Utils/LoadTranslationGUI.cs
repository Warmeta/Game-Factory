using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 
public class LoadTranslationGUI : MonoBehaviour {
    public string textInKeyLanguage;  // inglés
	public delegate void ChangeLanguageDelegate(int lang);
	public static ChangeLanguageDelegate ChangeLanguage;
     
    void Start(){
        LoadTranslation(ApplicationController.GameLanguage);
    }
     
    void LoadTranslation(int lang){
        GetComponent<Text>().text = LanguageManager.getTextInLanguage(textInKeyLanguage, lang);
    }
	
	/*Cuando el objeto se active, enlace la funcion de traduccion*/
	void OnEnable(){
		LoadTranslation(ApplicationController.GameLanguage);
		LoadTranslation_GUIButton.ChangeLanguage += LoadTranslation;
	}
	 
	/*Cuando el objeto se desactive, desenlace la funcion de traduccion*/
	void OnDisable(){
		LoadTranslation_GUIButton.ChangeLanguage -= LoadTranslation;
	}
}