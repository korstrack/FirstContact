using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienInteraction : MonoBehaviour
{
	public GameObject Alien;
	private float grabTime = 4.0f;

    private IEnumerator OnTriggerEnter(Collider other)
    {
    	if(other.gameObject.CompareTag("Interactable"))
    	{
    		other.transform.parent = Alien.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform;
    		Alien.GetComponent<Animator>().SetTrigger("grab");
    		other.GetComponent<AudioSource>().Play();
    		yield return new WaitForSeconds(grabTime);
    		Alien.GetComponent<Animator>().SetTrigger("ReverseGrab");
    	}
    }
}
