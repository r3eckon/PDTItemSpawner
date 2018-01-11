using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomItemSpawner : MonoBehaviour {

    public CustomItem spawnedItemSettings;

    public static CustomItem[] items, orderedItems;

    public static List<string> uniqueItemsGenerated = new List<string>();

    public static List<CustomItem> currentPossibleItems = new List<CustomItem>();

    public CustomItemSpawnType spawnType;

    [HideInInspector]
    public bool triggered = false;
    [HideInInspector]
    public bool spawned = false;

    public static System.Random rand;
    float n;

    GameObject gatherer;

    public static int num=0;//Amount of item spawners handled

    GameObject obj;



    /*  Edit the below method to create custom item behaviors
     *  This ties in with the "PlayerInventory.cs" script which holds the amount of coins and keys the player has.
        You can also use custom inventory script and assets, the method below is where you would tie both scripts.
        Here is a template to replace with your item's name and behavior. Add it in between the commented area.

            case "ITEM_NAME":
                GameObject.Find("Player").GetComponent<PlayerInventory>(). //Add the relevant component to increment here
                //Add more stuff to do upon picking this object up
                break;

    */
    public void ItemPickedUp(CustomItemGatherer gatherer)
    {
        switch (spawnedItemSettings.type.ToUpper())
        {
            //Add your own item behavior below this line
             

            //Do not edit anything below this line to keep ordinary pick up behavior

            default:
                if (!gatherer.inventory.ContainsKey(spawnedItemSettings.type.ToUpper()))
                {
                    gatherer.inventory.Add(spawnedItemSettings.type.ToUpper(), 1);
                }
                else
                {
                    int items = 0;
                    gatherer.inventory.TryGetValue(spawnedItemSettings.type.ToUpper(),out items);
                    gatherer.inventory.Remove(spawnedItemSettings.type.ToUpper());
                    gatherer.inventory.Add(spawnedItemSettings.type.ToUpper(), ++items);
                }
                
                break;
        }
    }


    //Edit the following method to add more general spawn types
    //which need to be handled and return true if the item type
    //is equal to the types included by your general spawn type
    bool spawnTypeCheck(CustomItemSpawnType item)
    {
        if (item == spawnType) return true;

        else if(item == CustomItemSpawnType.AnySurface)
        {
            return (spawnType == CustomItemSpawnType.EnclosedSurface || spawnType == CustomItemSpawnType.OpenSurface);
        }
        else if(item == CustomItemSpawnType.Any)
        {
            return true;
        }
        else
        {
            return false;
        }

    }


    //Below methods are important for the generation behavior to be consistent
    //Do not edit those unless you know what you are doing
    void Start () {

        rand = new System.Random((int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond));

        items = CustomItemManager.initializeListPrefab(CustomItemManager.loadItemList(CustomItemManager.getItemListPath()));

        if(orderedItems==null)
            orderedItems = CustomItemManager.reorderListByChance(items);

	}
	
	void Update () {

        if (!spawned)
        {
            currentPossibleItems = new List<CustomItem>();

            n = (float)rand.NextDouble();

            for (int i = 0; i < orderedItems.Length; i++)
            {

                //Cannot spawn on this spawner
                if (!spawnTypeCheck(orderedItems[i].spawntype))
                    continue;

                //Unique item already placed
                if (uniqueItemsGenerated.Contains(orderedItems[i].type))
                    continue;

                //Add to possible spawnable items
                if (n < orderedItems[i].generationChance)
                {

                    currentPossibleItems.Add(orderedItems[i]);

                    
                }


            }

            if(currentPossibleItems.Count > 0)
            {
                CustomItem generated = currentPossibleItems[rand.Next(currentPossibleItems.Count)];

                for (int v = 0; v < gameObject.transform.childCount; v++)
                    GameObject.Destroy(gameObject.transform.GetChild(v).gameObject);

                spawned = true;

                obj = GameObject.Instantiate(generated.prefab);
                obj.transform.localScale = generated.prefab.transform.lossyScale;
                obj.transform.position = gameObject.transform.position;
                obj.transform.rotation = gameObject.transform.rotation;
                obj.transform.parent = gameObject.transform;

                

                if (generated.unique)
                {
                    uniqueItemsGenerated.Add(generated.type);
                }

                spawnedItemSettings = generated;

                return;
            }
            else
            {
                spawned = true;
               
                //GameObject.Destroy(gameObject);
                
                return;
            }

        }

        if (triggered)
        {
            if (spawnedItemSettings.pickupable)
            {

                if (transform.childCount == 0)
                    return;

                gameObject.transform.GetChild(0).transform.position = Vector3.Lerp(gameObject.transform.GetChild(0).transform.position, gatherer.transform.position, 0.04f);
                
                if (Vector3.Distance(gameObject.transform.GetChild(0).transform.position, gatherer.transform.position) < 0.4f)
                {
                    ItemPickedUp(gatherer.GetComponent<CustomItemGatherer>());
                    triggered = false;

                    GameObject.Destroy(gameObject.transform.GetChild(0).gameObject);

                }
            }
            
        } 
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CustomItemGatherer>() !=null)
        {
            triggered = true;
            gatherer = other.gameObject;

        }
    }


}
