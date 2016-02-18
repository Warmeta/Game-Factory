using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
 
public class mytest : MonoBehaviour, IPointerEnterHandler {
    public void OnPointerEnter(PointerEventData data) {
        Debug.Log("Enter"+data);
    }
}