using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.EventSystems;

public class VRInputModule : BaseInputModule
{
	public Camera cam;
	public SteamVR_Input_Sources TargetSource;
	public SteamVR_Action_Boolean clickAction;

	private GameObject currentObj = null;
	private PointerEventData mData = null;

	protected override void Awake()
	{
		base.Awake();

		mData = new PointerEventData(eventSystem);
	}

	public override void Process()
	{
		// reset data, set camera
		mData.Reset();
		mData.position = new Vector2(cam.pixelWidth / 2, cam.pixelHeight / 2);

		// raycast
		eventSystem.RaycastAll(mData, m_RaycastResultCache);
		mData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
		currentObj = mData.pointerCurrentRaycast.gameObject;

		// clear raycast
		m_RaycastResultCache.Clear();

		// handle hover states
		HandlePointerExitAndEnter(mData, currentObj);

		// press
		if(clickAction.GetStateDown(TargetSource))
			ProcessPress(mData);

		// release
		if(clickAction.GetStateUp(TargetSource))
			ProcessRelease(mData);

	}

	public PointerEventData GetData()
	{
		return mData;
	}

	private void ProcessPress(PointerEventData data)
	{
		// set raycast
		data.pointerPressRaycast = data.pointerCurrentRaycast;

		// check for object hit, get down, call
		GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(currentObj, data, ExecuteEvents.pointerDownHandler);

		// if not down handler, try and get click handler
		if(newPointerPress == null)
			newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObj);


		// set data
		data.pressPosition = data.position;
		data.pointerPress = newPointerPress;
		data.rawPointerPress = currentObj;
	}

	private void ProcessRelease(PointerEventData data)
	{
		// execute pointer up
		ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

		// check for click handler
		GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObj);

		// check if actual
		if(data.pointerPress == pointerUpHandler)
		{
			ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
		}

		// clear selected game object
		eventSystem.SetSelectedGameObject(null);

		// reset data
		data.pressPosition = Vector2.zero;
		data.pointerPress = null;
		data.rawPointerPress = null;
	}


}
