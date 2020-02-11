using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawn;
    public Transform spawnLocation;

    public void createObject(){
    	Instantiate(spawn, spawnLocation.position, Quaternion.Euler(new Vector3(0,180,0)));
    }
}
