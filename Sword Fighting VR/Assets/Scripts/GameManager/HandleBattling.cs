using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleBattling : MonoBehaviour
{
    [SerializeField] private GameObject inDoor;
    [SerializeField] private GameObject outDoor;

    [SerializeField] private Transform wallFolder;
    private List<Transform> walls = new List<Transform>();
    private Bounds bounds;

    
    public void HandleSpawningDoor()
    {
        SpawnDoor(outDoor);
    }
    public void HandleWinning()
    {
        SpawnDoor(inDoor);
        GameManager.Instance.EndBattle();
    }
   
    private void SpawnDoor(GameObject door)
    {
        for (int i = 0; i < wallFolder.childCount; i++)
        {
            walls.Add(wallFolder.GetChild(i));
        }
        int randomWall = Random.Range(0, walls.Count);

        bounds = walls[randomWall].GetComponent<Collider>().bounds;
        Vector3 randomPos = SearchRandomDoorPosition(randomWall);

        GameObject cloneDoor = Instantiate(door, Vector3.zero, walls[randomWall].rotation, transform);
        cloneDoor.transform.position = randomPos;
        cloneDoor.transform.parent = walls[randomWall];
    }

    private Vector3 SearchRandomDoorPosition(int index)
    {
        Vector3 minLocal = walls[index].InverseTransformPoint(bounds.min);
        Vector3 maxLocal = walls[index].InverseTransformPoint(bounds.max);
        Vector3 localPos = new Vector3(
            Random.Range(minLocal.x, maxLocal.x),
            minLocal.y,
           transform.forward.z * 0.5f);

        Vector3 newPos = walls[index].TransformPoint(localPos);

        return newPos;
    }


}
