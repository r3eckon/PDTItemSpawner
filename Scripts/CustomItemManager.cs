using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CustomItemManager {

    //Item list path use in game. Must be appended with prefix in editor.
    private static string DefaultListPath = "PDTFiles/Saved Data/CustomItemList";
    public static string getDefaultListPath() { return DefaultListPath; }

    //Resources folder editor prefix
    private static string ResourcesFolderPrefix = Application.dataPath + "/PDT Item Spawner/Resources/";
    public static string getResourcesFolderPrefix() { return ResourcesFolderPrefix;  }

    public static CustomItem[] reorderedList;

    public static CustomItem[] items;

    public static int seed = 1;


    public static void saveItemList(string path, CustomItem[] list)
    {
        StreamWriter w = new StreamWriter(path);

        w.WriteLine("- Custom Item List - ");
        w.WriteLine("Count =" + list.Length);

        for(int i = 0; i < list.Length;i++)
        {
            w.WriteLine("-");
            w.WriteLine("Type =" + list[i].type);
            w.WriteLine("Prefab =" + ((list[i].prefab != null) ? list[i].prefab.name : "None" ));
            w.WriteLine("Chance =" + list[i].generationChance.ToString(System.Globalization.CultureInfo.InvariantCulture));
            w.WriteLine("Unique =" + ( (list[i].unique) ? "TRUE" : "FALSE" ) );
            w.WriteLine("Pickupable =" + (list[i].pickupable ? "TRUE" : "FALSE"));
            w.WriteLine("SpawnType =" + (short)list[i].spawntype);

        }

        w.Close();

    }

    public static CustomItem[] loadItemList(string path)
    {
        CustomItem[] items;
        string currentLine;

        if (Application.isEditor)
        {
            StreamReader sr = new StreamReader(path);

            sr.ReadLine();//Read descriptor line


            currentLine = sr.ReadLine();//Read object count
            int count = int.Parse(currentLine.Replace("Count =", ""));

            items = new CustomItem[count];
            CustomItem currentItem = new CustomItem();



            for (int i = 0; i < count; i++)
            {
                sr.ReadLine();//Read descriptor line

                currentLine = sr.ReadLine();//Read object type/name
                currentItem.type = currentLine.Replace("Type =", "");

                currentLine = sr.ReadLine();//Read prefab name
                currentItem.prefabName = currentLine.Replace("Prefab =", "");

                currentLine = sr.ReadLine();//Read generation chance
                currentItem.generationChance = float.Parse(currentLine.Replace("Chance =", ""), System.Globalization.CultureInfo.InvariantCulture);

                currentLine = sr.ReadLine();//Read uniqueness line
                currentLine = currentLine.Replace("Unique =", "");
                currentItem.unique = (currentLine == "TRUE") ? true : false;

                currentLine = sr.ReadLine();
                currentLine = currentLine.Replace("Pickupable =", "");
                currentItem.pickupable = (currentLine == "TRUE") ? true : false;


                currentLine = sr.ReadLine();//Read spawn type line
                currentItem.spawntype = (CustomItemSpawnType)short.Parse(currentLine.Replace("SpawnType =", ""));

                currentItem.initialized = false;

                items[i] = currentItem;

            }

            sr.Close();
        }
        else
        {
            TextAsset ta = (TextAsset)Resources.Load(path,typeof(TextAsset));
            Debug.Log(path);
            StringReader sr = new StringReader(ta.text);

            sr.ReadLine();//Read descriptor line


            currentLine = sr.ReadLine();//Read object count
            int count = int.Parse(currentLine.Replace("Count =", ""));

            items = new CustomItem[count];
            CustomItem currentItem = new CustomItem();




            for (int i = 0; i < count; i++)
            {
                sr.ReadLine();//Read descriptor line

                currentLine = sr.ReadLine();//Read object type/name
                currentItem.type = currentLine.Replace("Type =", "");

                currentLine = sr.ReadLine();//Read prefab name
                currentItem.prefabName = currentLine.Replace("Prefab =", "");

                currentLine = sr.ReadLine();//Read generation chance
                currentItem.generationChance = float.Parse(currentLine.Replace("Chance =", ""), System.Globalization.CultureInfo.InvariantCulture);

                currentLine = sr.ReadLine();//Read uniqueness line
                currentLine = currentLine.Replace("Unique =", "");
                currentItem.unique = (currentLine == "TRUE") ? true : false;

                currentLine = sr.ReadLine();
                currentLine = currentLine.Replace("Pickupable =", "");
                currentItem.pickupable = (currentLine == "TRUE") ? true : false;


                currentLine = sr.ReadLine();//Read spawn type line
                currentItem.spawntype = (CustomItemSpawnType)short.Parse(currentLine.Replace("SpawnType =", ""));

                currentItem.initialized = false;

                items[i] = currentItem;

            }

            sr.Close();
        }
        

        return items;
    }

    public static CustomItem[] initializeListPrefab(CustomItem[] list)
    {
        CustomItem[] initialized = new CustomItem[list.Length];

        for(int i = 0; i < list.Length; i++)
        {
            initialized[i] = list[i];

            try
            {
                if(initialized[i].prefabName!= "None")
                {
                    initialized[i].prefab = GameObject.Find(initialized[i].prefabName);
                    
                }
                initialized[i].initialized = true;

            }
            catch { }

        }

        return initialized;
    }



    public static CustomItem[] reorderListByChance(CustomItem[] list)
    {

        List<CustomItem> ordered = new List<CustomItem>();

        CustomItem topChance = new CustomItem();


        do
        {
            topChance.type = "NULL";
            topChance.generationChance = 1;

            for (int i = 0; i < list.Length; i++)
            {
                if (!ordered.Contains(list[i]))
                {


                    if (topChance.generationChance >= list[i].generationChance)
                    {



                        topChance = list[i];
                    }
                }
            }

            if (topChance.type != "NULL")
                ordered.Add(topChance);



        } while (topChance.type != "NULL");



        return ordered.ToArray();

    }


}
