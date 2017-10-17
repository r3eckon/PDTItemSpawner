using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PDTItemManagerEditor : EditorWindow {

    CustomItemManager man;

    GUIStyle large, medium, center,largecenter;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/PDT Item Spawner Settings")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        PDTItemManagerEditor window = (PDTItemManagerEditor)EditorWindow.GetWindow(typeof(PDTItemManagerEditor));
        window.minSize = new Vector2(530,200);
        window.maxSize = new Vector2(530, 1000);
        window.Show();
    }

    private void OnEnable()
    {

        large = new GUIStyle();
        large.fontSize = 16;
        medium = new GUIStyle();
        medium.fontSize = 13;
        center = new GUIStyle();
        center.alignment = TextAnchor.MiddleCenter;
        largecenter = new GUIStyle();
        largecenter.fontSize = 16;
        largecenter.alignment = TextAnchor.MiddleCenter;

        if (CustomItemManager.items == null)
        {
            try
            {
                CustomItemManager.items = CustomItemManager.initializeListPrefab(CustomItemManager.loadItemList(CustomItemManager.getItemListPath()));
            }
            catch
            {
                CustomItem[] temp = new CustomItem[1];
                CustomItem newi = new CustomItem();
                newi.generationChance = 0.5f;
                newi.type = "New Item";

                CustomItemManager.saveItemList(CustomItemManager.getItemListPath(), temp);
                CustomItemManager.items = temp;
            }

        }
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("PDT Random Item Spawner 1.0",largecenter);

        EditorGUILayout.Space();
        EditorGUILayout.Space();



        //
        //if(GUILayout.Button("Force Load"))
        //{
        //    gen.customItems = CustomItemManager.initializeListPrefab(CustomItemManager.loadItemList(CustomItemManager.getItemListPath()));
        //}

        if (GUILayout.Button("Add Custom Item Slot"))
        {
            try
            {
                //First try directly editing a correctly loaded list

                CustomItem[] temp = new CustomItem[CustomItemManager.items.Length + 1];
                CustomItem newi = new CustomItem();
                newi.generationChance = 0.5f;
                newi.type = "New Item";

                for (int i = 0; i < temp.Length; i++)
                {
                    if (i < CustomItemManager.items.Length)
                        temp[i] = CustomItemManager.items[i];
                    else
                        temp[i] = newi;
                }

                CustomItemManager.items = temp;

            }
            catch
            {

                //Then try loading list to see if it exists

                try
                {

                    CustomItemManager.items = CustomItemManager.initializeListPrefab(CustomItemManager.loadItemList(CustomItemManager.getItemListPath()));

                    CustomItem[] temp = new CustomItem[CustomItemManager.items.Length + 1];
                    CustomItem newi = new CustomItem();
                    newi.generationChance = 0.5f;
                    newi.type = "New Item";

                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (i < CustomItemManager.items.Length)
                            temp[i] = CustomItemManager.items[i];
                        else
                            temp[i] = newi;
                    }

                    CustomItemManager.items = temp;

                }
                catch
                {

                    //Finally create and save an entirely new list if all fails

                    CustomItem[] temp = new CustomItem[1];
                    CustomItem newi = new CustomItem();
                    newi.generationChance = 0.5f;
                    newi.type = "New Item";

                    CustomItemManager.saveItemList(CustomItemManager.getItemListPath(), temp);
                    CustomItemManager.items = temp;

                }
            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 1;
        EditorGUIUtility.fieldWidth = 0;
        GUILayout.Space(30);
        EditorGUILayout.LabelField("Type");
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Prefab");
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Spawn Chance", GUILayout.Width(115));
        //GUILayout.Space(50);
        EditorGUILayout.LabelField("Pickupable?", GUILayout.Width(70));
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Spawn Type", GUILayout.Width(90));
        //GUILayout.Space(20);
        EditorGUILayout.LabelField("Unique?");
        GUILayout.Space(20);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (CustomItemManager.items != null)
        {

            for (int i = 0; i < CustomItemManager.items.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUIUtility.labelWidth = 50;

                EditorGUIUtility.fieldWidth = 40;


                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    List<CustomItem> t = new List<CustomItem>(CustomItemManager.items);
                    t.RemoveAt(i);
                    CustomItemManager.items = t.ToArray();
                    continue;
                }
                CustomItemManager.items[i].type = EditorGUILayout.TextField(CustomItemManager.items[i].type, GUILayout.Width(55));
                GUILayout.Space(10);
                CustomItemManager.items[i].prefab = (GameObject)EditorGUILayout.ObjectField("", CustomItemManager.items[i].prefab, typeof(GameObject), GUILayout.Width(70));
                EditorGUIUtility.labelWidth = 45;
                CustomItemManager.items[i].generationChance = EditorGUILayout.Slider("", CustomItemManager.items[i].generationChance, 0, 1, GUILayout.Width(140));
                GUILayout.Space(20);
                EditorGUIUtility.labelWidth = 10;
                CustomItemManager.items[i].pickupable = EditorGUILayout.Toggle("", CustomItemManager.items[i].pickupable, GUILayout.Width(25));
                GUILayout.Space(10);
                CustomItemManager.items[i].spawntype = (CustomItemSpawnType)EditorGUILayout.EnumPopup(CustomItemManager.items[i].spawntype, GUILayout.Width(100));
                GUILayout.Space(20);
                CustomItemManager.items[i].unique = EditorGUILayout.Toggle("", CustomItemManager.items[i].unique, GUILayout.Width(25));

                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            try
            {
                CustomItemManager.items = CustomItemManager.initializeListPrefab(CustomItemManager.loadItemList(CustomItemManager.getItemListPath()));
            }
            catch
            {

            }


        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Save / Apply Custom Item List"))
        {
            CustomItemManager.saveItemList(CustomItemManager.getItemListPath(), CustomItemManager.items);
        }

    }

}
