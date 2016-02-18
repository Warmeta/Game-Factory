using UnityEngine;
using System.Collections;

public class MainPanelViewController : MonoBehaviour {

	Animator animator;
	Vector3 displacedPos;
	Vector3 retractedPos;
	Vector3 currentPos;
	Vector3 startMarker;
	Vector3 endMarker;

	float startTime = 0.0f;

	public float speed = 1.0f;
	public float duration = 0.2f;

	private bool isShown = true;
	
	void Start () {

		retractedPos = transform.position;
		displacedPos = transform.position;
		displacedPos.x = 2 * Screen.width - (int)(0.175f * Screen.width);
		startMarker = retractedPos;
		endMarker = retractedPos;
	
	}

	void Update() {

		float covered = (Time.time - startTime) * speed;
		float factor = Mathf.Min (covered / duration,  1.0f);

		if (Time.time - startTime <= duration) {
			transform.position = Vector3.Lerp(startMarker, endMarker, factor);
		}
		else {
			transform.position = endMarker;
		}
	}

	public void Show (bool value = true) {

		if (isShown != value) {
			isShown = value;
			startTime = Time.time;

			//animator.SetBool("Visible", value);
			if (value) {
				Debug.Log ("Showing main panel");
				startMarker = displacedPos;
				endMarker = retractedPos;
			}
			else {
				Debug.Log ("Hiding main panel");
				startMarker = retractedPos;
				endMarker = displacedPos;
			}
		}
	}

	public bool IsShown() {
		return isShown;
	}
}
