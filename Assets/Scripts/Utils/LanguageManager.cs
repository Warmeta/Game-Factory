using UnityEngine;
using System.Collections;
using System.IO;
 
public static class LanguageManager{
    private static Hashtable textTable;
    private static int nLanguages;

	public static void LoadLanguagesFile(string filename){
		/* 1. CARGUE EL ARCHIVO DE TEXTO */
		TextAsset textFile = (TextAsset) Resources.Load(filename, typeof(TextAsset));
		 if (textFile == null){
			Debug.LogWarning("LanguageManager: el archivo '"+ filename +"' no se encontro!.");
			return;
		}
		 
		/* 2. PARSING DEL ARCHIVO */
		textTable = new Hashtable();
		StringReader reader = new StringReader(textFile.text);
		 
		string line= reader.ReadLine(); //num. de idiomas= primera linea en el archivo de idiomas!
		nLanguages =  int.Parse(line);  //convierta este numero a un entero
		string key; 
		while (( line= reader.ReadLine()) != null){
			key = line;
			if(nLanguages==1){
				string val = reader.ReadLine();
				textTable.Add(key,val);
			}
			else{
				string[] val = new string[nLanguages];
				for(int i=0; i<nLanguages; i++){
					line= reader.ReadLine();
					val[i] = line;
				}
				textTable.Add(key,val);
				Debug.Log(key+" "+val);
			}
		}
		reader.Close();    
	}

	public static string getTextInLanguage(string key, int l){
		/* 1. Verificamos que la tabla no este vacia */
		if(textTable==null) {
			Debug.LogWarning("LanguageManager: Error, olvidaste leer los datos en la hashtable?");
			return key; /* si esta vacia retornamos la palabra key ingresada */
		}
		/* 2. Verificamos que la palabra clave este en la tabla*/
		string val = ""; /*esta sera la palabra key traducida al idioma l*/
		if (textTable.ContainsKey(key)){
			if(nLanguages==1) val = l==0 ? key:(string)textTable[key];
			else{
				string[] vals = (string[])textTable[key];
				if(l<nLanguages) val = vals[l];   
			}
		}
		else{ 
			Debug.LogWarning("La palabra '"+ key + "' no se encontro en la tabla!");
			return key;
		}
		return val;
	}
}