using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AddItemToMenu : MonoBehaviour
{
    static void LoadObject(string path, MenuCommand menuCommand)
    {
        GameObject obj = Resources.Load(path) as GameObject;
        InstantiatePrefab(obj, menuCommand);
    }
    static void InstantiatePrefab(GameObject prefab, MenuCommand menuCommand)
    {
        GameObject go = Instantiate(prefab);
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
    [MenuItem("GameObject/Custom/Ticker")]
    public static void AddTicker(MenuCommand menuCommand)
    {
        LoadObject("Ticker", menuCommand);
    }

    [MenuItem("GameObject/Weapons/Sword")]
    public static void AddSword(MenuCommand menuCommand)
    {
        LoadObject("Sword", menuCommand);
    }
    
    
}
