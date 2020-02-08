using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
	public float defaultLength = 5.0f;
	public GameObject m_Dot;
	public VRInputModule InputModule;

	private LineRenderer m_LineRenderer = null;


	private void Awake()
	{
		m_LineRenderer = GetComponent<LineRenderer>();
	}

    // Update is called once per frame
    void Update()
    {
    	UpdateLine();   
    }

    private void UpdateLine()
    {
    	// use default or distance
    	PointerEventData data = InputModule.GetData();
    	float targetLen = data.pointerCurrentRaycast.distance == 0 ? defaultLength : data.pointerCurrentRaycast.distance;

    	// racast
    	RaycastHit hit = CreateRaycast(targetLen);

    	// default end
    	Vector3 endPos = transform.position + (transform.forward * targetLen);

    	// update length on hit
    	if(hit.collider != null){
    		endPos = hit.point;
    	}

    	// set position of dot
    	m_Dot.transform.position = endPos;
    	// set position of line renderer
    	m_LineRenderer.SetPosition(0, transform.position);
    	m_LineRenderer.SetPosition(1, endPos);

    }

    private RaycastHit CreateRaycast(float length){
    	RaycastHit hit;
    	Ray ray = new Ray(transform.position, transform.forward);
    	Physics.Raycast(ray, out hit, defaultLength);

    	return hit;
    }
}
