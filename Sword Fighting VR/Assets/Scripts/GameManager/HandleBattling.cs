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
        SpawnDoor(outDoor, false);
    }
    public void HandleWinning()
    {
        SpawnDoor(inDoor, true);
        GameManager.Instance.EndBattle();
    }
   
    private void SpawnDoor(GameObject door, bool isIndoor)
    {
        for (int i = 0; i < wallFolder.childCount; i++)
        {
            _walls.Add(wallFolder.GetChild(i));
        }
        int randomWall = Random.Range(0, _walls.Count);

        _bounds = _walls[randomWall].GetComponent<Collider>().bounds;
        Vector3 randomPos = SearchRandomDoorPosition(randomWall, isIndoor);

        GameObject cloneDoor = Instantiate(door, Vector3.zero, _walls[randomWall].rotation);
        cloneDoor.transform.position = randomPos;
        cloneDoor.transform.parent = _walls[randomWall];
    }

    private Vector3 SearchRandomDoorPosition(int index, bool isIndoor)
    {
        Vector3 minLocal = _walls[index].InverseTransformPoint(_bounds.min);
        Vector3 maxLocal = _walls[index].InverseTransformPoint(_bounds.max);
        Vector3 centerLocal = _walls[index].InverseTransformPoint(_bounds.center);
        Vector3 localPos = new Vector3(centerLocal.x, minLocal.y, maxLocal.z);

        if (!isIndoor) localPos.y += 1.5f;
        

        Vector3 newPos = _walls[index].TransformPoint(localPos);

        return newPos;
    }


}
