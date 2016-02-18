using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerEventsController : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler {
	
	/*public int mouseOnCount = 0;
	private AudioClip audioclip;
	private AudioSource audiosource;*/
	public GameObject miniButtons;

	public void Start(){
		/*audioclip = Resources.Load <AudioClip> ("button_hover_sound");
		gameObject.AddComponent<AudioSource> ();
		audiosource = gameObject.GetComponent<AudioSource> ();*/
	}
	
	public void OnPointerEnter(PointerEventData eventData){
		Debug.Log ("Enter");
		miniButtons.SetActive (true);
	}
	public void OnPointerExit(PointerEventData eventData){
		Debug.Log ("Exit");
		miniButtons.SetActive (false);
	}
	
}
