using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour
{
	public SteamVR_Action_Boolean m_GrabAction = null;

	private SteamVR_Behaviour_Pose m_Pose = null;
	private FixedJoint m_Joint = null;

	private Interactable m_CurrentInteractable = null;
	private List<Interactable> m_ContactInteractables = new List<Interactable>();

    private void Awake()
    {
    	m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    	m_Joint = GetComponent<FixedJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        // down
    	if(m_GrabAction.GetStateDown(m_Pose.inputSource)){
    		print(m_Pose.inputSource + "Trigger Down");
    		Pickup();
    	}
        //up
        if(m_GrabAction.GetStateUp(m_Pose.inputSource)){
    		print(m_Pose.inputSource + "Trigger Up");
    		Drop();
    	}
    }

    private void OnTriggerEnter(Collider other)
    {
    	if(!other.gameObject.CompareTag("Interactable"))
    		return;

    	m_ContactInteractables.Add(other.gameObject.GetComponent<Interactable>());
    }

    private void OnTriggerExit(Collider other)
    {
    	if(!other.gameObject.CompareTag("Interactable"))
    		return;

    	m_ContactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
    }

    public void Pickup()
    {
    	//get nearest inetact
    	m_CurrentInteractable = GetNearestInteractable();
    	//null check
    	if(!m_CurrentInteractable)
    		return;
    	//already held, check
		if(m_CurrentInteractable.m_ActiveHand)
			m_CurrentInteractable.m_ActiveHand.Drop();
    	//position
    	// m_CurrentInteractable.transform.position = transform.position;
    	// //attatch
    	// Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
    	// m_Joint.connectedBody = targetBody;
		Rigidbody rigbod = m_CurrentInteractable.GetComponent<Rigidbody>();
		rigbod.isKinematic = true;
		rigbod.detectCollisions = false;
		m_CurrentInteractable.transform.parent = this.transform;
    	//set active hand
    	m_CurrentInteractable.m_ActiveHand = this;
    }

    public void Drop()
    {
    	//null
    	if(!m_CurrentInteractable)
    		return;
    	//give back rigidbody and dislocate from hand
    	Rigidbody rigbod = m_CurrentInteractable.GetComponent<Rigidbody>();
    	rigbod.isKinematic = false;
    	rigbod.detectCollisions = true;
    	m_CurrentInteractable.transform.parent = null;
    	//apply velocity
    	Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
    	targetBody.velocity = m_Pose.GetVelocity();
    	targetBody.angularVelocity = m_Pose.GetAngularVelocity();
    	//detatch it
    	m_Joint.connectedBody = null;
    	//clear
    	m_CurrentInteractable.m_ActiveHand = null;
    	m_CurrentInteractable = null;
    }

    private Interactable GetNearestInteractable()
    {
    	Interactable nearest = null;
    	float minDist = float.MaxValue;
    	float distance = 0.0f;

    	foreach(Interactable interactable in m_ContactInteractables)
    	{
    		distance = (interactable.transform.position - transform.position).sqrMagnitude;

    		if(distance < minDist)
    		{
    			nearest = interactable;
    			minDist = distance;
    		}
    	}
    	return nearest;
    }
}
