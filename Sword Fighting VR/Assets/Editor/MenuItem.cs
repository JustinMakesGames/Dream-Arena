using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuItem : MonoBehaviour
{
    static void InstantiatePrefab(GameObject prefab, MenuCommand menuCommand)
    {
        GameObject go = Instantiate(prefab);
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
    [UnityEditor.MenuItem("GameObject/Custom/Ticker")]
    public static void AddTicker(MenuCommand menuCommand)
    {
        string path = "Ticker";
        GameObject go = Resources.Load<GameObject>(path);
        InstantiatePrefab(go, menuCommand);
    }
    
}
