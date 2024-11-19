using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SearchService;

public class SpawnGrid : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int length;
    [SerializeField] private float cellSize;
    [SerializeField] private float gridSpacing;
    [SerializeField] private GameObject tile;
    [SerializeField] private LayerMask walkableGround;

    [SerializeField] private Transform groundTransform;

    private Bounds _groundBounds;
    private Vector3 _spawnPosition;
    public static GameObject GenerateTiles(Transform parent, Vector3 position, LayerMask walkableGround, GameObject tile)
    {
        GameObject obj = Instantiate(tile);
        Transform objTransform = obj.transform;
        objTransform.parent = parent;
        objTransform.position = position;
        objTransform.GetComponent<GridInfo>().GetWalkableInfo();
       
        return obj;
    }

    private void Awake()
    {
        _groundBounds = groundTransform.GetComponent<Collider>().bounds;

        _spawnPosition = new Vector3(_groundBounds.min.x, _groundBounds.max.y, _groundBounds.min.z);
        GridTiles gridTiles = new GridTiles(width, length, gridSpacing, walkableGround, tile, _spawnPosition);
    }
}
