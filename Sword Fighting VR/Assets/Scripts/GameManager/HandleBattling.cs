using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleBattling : MonoBehaviour
{
    [SerializeField] private GameObject door;

    [SerializeField] private Transform wallFolder;
    private List<Transform> walls = new List<Transform>();
    private Bounds bounds;

    private void Start()
    {
        HandleWinning();
    }
    public void HandleWinning()
    {
        SpawnDoor();
    }

    private void SpawnDoor()
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
    }

    private Vector3 SearchRandomDoorPosition(int index)
    {
        Vector3 minLocal = walls[index].InverseTransformPoint(bounds.min);
        Vector3 maxLocal = walls[index].InverseTransformPoint(bounds.max);
        Vector3 localPos = new Vector3(
            Random.Range(minLocal.x, maxLocal.x),
            minLocal.y,
           maxLocal.z);

        Vector3 newPos = walls[index].TransformPoint(localPos);

        return newPos;
    }
}
