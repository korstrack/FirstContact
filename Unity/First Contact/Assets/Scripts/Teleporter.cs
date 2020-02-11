using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Teleporter : MonoBehaviour
{
	public GameObject m_Pointer;
	public SteamVR_Action_Boolean m_TeleportAction;

	private SteamVR_Behaviour_Pose Pose = null;
	private bool m_HasPosition = false;
	private float fadeTime = 0.5f;
	private bool isTeleporting = false;

	private void Awake(){
		Pose = GetComponent<SteamVR_Behaviour_Pose>();
	}


    // Update is called once per frame
    void Update()
    {
        //pointer
        m_HasPosition = UpdatePointer();
        m_Pointer.SetActive(m_HasPosition);


        //teleport
        if(m_TeleportAction.GetStateUp(Pose.inputSource))
        	TryTeleport();
    }

    private void TryTeleport(){
    	//check for valid position and if already teleporting
    	if(!m_HasPosition || isTeleporting){
    		return;
    	}

    	// get camera rig and head position
    	Transform cameraRig = SteamVR_Render.Top().origin;
    	Vector3 headPosition = SteamVR_Render.Top().head.position;

    	//figure out translation
    	Vector3 groudPosition = new Vector3(headPosition.x, cameraRig.position.y, headPosition.z);
    	Vector3 translationVector = m_Pointer.transform.position - groudPosition;

    	//move
    	StartCoroutine(MoveRig(cameraRig, translationVector));
    }

    private IEnumerator MoveRig(Transform cameraRig, Vector3 translation){
    	//flag
    	isTeleporting = true;

    	//fade to black
    	SteamVR_Fade.Start(Color.black, fadeTime, true);

    	//apply translation
    	yield return new WaitForSeconds(fadeTime);
    	cameraRig.position += translation;

    	//fade to clear
    	SteamVR_Fade.Start(Color.clear, fadeTime, true);

    	//deflag
    	isTeleporting = false;

    }

    private bool UpdatePointer(){
    	// ray from controller
    	Ray ray = new Ray(transform.position, transform.forward);
    	RaycastHit hit;
    	// if its a hit
    	if(Physics.Raycast(ray, out hit) && hit.transform.tag == "Floor"){
    		m_Pointer.transform.position = hit.point;
    		return true;
    	}
    	// if not a hit
    	return false;
    }
}
