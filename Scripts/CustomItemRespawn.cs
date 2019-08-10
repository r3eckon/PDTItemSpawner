using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomItemRespawn : MonoBehaviour {

    [Header("Delay in Seconds")]
    public float delay = 0;
    private float counter = 0;

    CustomItemSpawner spawner;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        try
        {
            spawner = GetComponent<CustomItemSpawner>();
        }
        catch { }
		
        if(transform.childCount == 0)
        {

            if ((counter += Time.deltaTime) >= delay)
            {
                counter = 0;

                try
                {

                    
                   GetComponent<CustomItemSpawner>().spawned = false;
                   GetComponent<CustomItemSpawner>().triggered = false;
                    

                }
                catch
                {

                }
            }

        }


	}
}
