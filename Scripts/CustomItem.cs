using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CustomItem
{

    public GameObject prefab;
    public float generationChance;
    public bool unique;
    public bool pickupable;
    public string type;
    public string prefabName;
    public bool initialized;
    public CustomItemSpawnType spawntype;

}
