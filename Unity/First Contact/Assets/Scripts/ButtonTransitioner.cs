using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTransitioner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
 	public Color32 normalColor = Color.white;
 	public Color32 hoverColor = Color.red;
 	public Color32 downColor = Color.green;

 	private Image image = null;

 	private void Awake()
 	{
 		image = GetComponent<Image>();
 	}

 	public void OnPointerEnter(PointerEventData eventData)
 	{
 		print("Enter");

 		image.color = hoverColor;
 	}

 	public void OnPointerExit(PointerEventData eventData)
 	{
 		print("Exit");

 		image.color = normalColor;
 	}

 	public void OnPointerDown(PointerEventData eventData)
 	{
 		print("Down");

 		image.color = downColor;
 	}

 	public void OnPointerUp(PointerEventData eventData)
 	{
 		print("Up");
 	}

 	public void OnPointerClick(PointerEventData eventData)
 	{
 		print("click");

 		image.color = hoverColor;
 	}
}
