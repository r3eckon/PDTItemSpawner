using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomItemRespawn : MonoBehaviour {

    [Header("Delay In Seconds")]
    public float delay = 0;
    private float counter = 0;

    CustomItemSpawner spawner;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if((counter += Time.deltaTime) >= delay)
        {
            counter = 0;

            try
            {

                spawner = GetComponent<CustomItemSpawner>();

                if(transform.childCount == 0)
                {
                    GetComponent<CustomItemSpawner>().spawned = false;
                    GetComponent<CustomItemSpawner>().triggered = false;

                    //Debug.Log("RESPAWNED!");
                }
                else
                {
                    //Debug.Log("ITEM STILL ON SPAWNER");
                }

            }
            catch
            {
                
            }
        }


	}
}
