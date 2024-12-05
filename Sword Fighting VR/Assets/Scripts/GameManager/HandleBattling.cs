using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleBattling : MonoBehaviour
{
    [SerializeField] private GameObject inDoor;
    [SerializeField] private GameObject outDoor;

    [SerializeField] private Transform wallFolder;
    private List<Transform> _walls = new List<Transform>();
    private Bounds _bounds;

    
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
            _walls.Add(wallFolder.GetChild(i));
        }
        int randomWall = Random.Range(0, _walls.Count);

        _bounds = _walls[randomWall].GetComponent<Collider>().bounds;
        Vector3 randomPos = SearchRandomDoorPosition(randomWall);

        GameObject cloneDoor = Instantiate(door, Vector3.zero, _walls[randomWall].rotation, transform);
        cloneDoor.transform.position = randomPos;
        cloneDoor.transform.parent = _walls[randomWall];
    }

    private Vector3 SearchRandomDoorPosition(int index)
    {
        Vector3 minLocal = _walls[index].InverseTransformPoint(_bounds.min);
        Vector3 maxLocal = _walls[index].InverseTransformPoint(_bounds.max);
        Vector3 localPos = new Vector3(
            Random.Range(minLocal.x, maxLocal.x),
            minLocal.y,
           transform.forward.z * 0.5f);

        Vector3 newPos = _walls[index].TransformPoint(localPos);

        return newPos;
    }


}
