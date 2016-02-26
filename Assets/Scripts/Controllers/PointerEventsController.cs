using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerEventsController : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler {
	
	/*public int mouseOnCount = 0;
	private AudioClip audioclip;
	private AudioSource audiosource;*/
	public GameObject hiddenObj;

	public void Start(){
		/*audioclip = Resources.Load <AudioClip> ("button_hover_sound");

		audiosource = gameObject.GetComponent<AudioSource> ();*/
		hiddenObj.SetActive(false);
	}
	
	public void OnPointerEnter(PointerEventData eventData){
		Debug.Log ("Enter");
		hiddenObj.SetActive (true);
	}
	public void OnPointerExit(PointerEventData eventData){
		Debug.Log ("Exit");
		hiddenObj.SetActive (false);
	}
	
}
