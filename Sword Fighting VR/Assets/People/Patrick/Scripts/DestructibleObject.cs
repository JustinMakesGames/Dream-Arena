using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    private bool _edgeSet = false;
    private Vector3 _edgeVertex;
    private Vector2 _edgeUV;
    private Plane _edgePlane;
    

    public int cutCascades = 1;
    public float explodeForce = 0;


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            DestroyMesh();
        }
    }

    private void DestroyMesh()
    {
        var originalMesh = GetComponent<MeshFilter>().mesh;
        originalMesh.RecalculateBounds();
        var parts = new List<PartMesh>();
        var subParts = new List<PartMesh>();

        var mainPart = new PartMesh
        {
            uv = originalMesh.uv,
            vertices = originalMesh.vertices,
            normals = originalMesh.normals,
            triangles = new int[originalMesh.subMeshCount][],
            bounds = originalMesh.bounds
        };
        for (int i = 0; i < originalMesh.subMeshCount; i++)
        {
            mainPart.triangles[i] = originalMesh.GetTriangles(i);
        }
            

        parts.Add(mainPart);

        for (var c = 0; c < cutCascades; c++)
        {
            for (var i = 0; i < parts.Count; i++)
            {
                var bounds = parts[i].bounds;
                bounds.Expand(0.5f);

                var plane = new Plane(UnityEngine.Random.onUnitSphere, new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
                                                                                   UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
                                                                                   UnityEngine.Random.Range(bounds.min.z, bounds.max.z)));


                subParts.Add(GenerateMesh(parts[i], plane, true));
                subParts.Add(GenerateMesh(parts[i], plane, false));
            }
            parts = new List<PartMesh>(subParts);
            subParts.Clear();
        }

        for (var i = 0; i < parts.Count; i++)
        {
            parts[i].MakeGameObject(this);
            parts[i].gameObject.GetComponent<Rigidbody>().AddForceAtPosition(parts[i].bounds.center * explodeForce, transform.position);
        }

        Destroy(gameObject);
    }

    private PartMesh GenerateMesh(PartMesh original, Plane plane, bool left)
    {
        var partMesh = new PartMesh() { };
        var ray1 = new Ray();
        var ray2 = new Ray();


        for (var i = 0; i < original.triangles.Length; i++)
        {
            var triangles = original.triangles[i];
            _edgeSet = false;

            for (var j = 0; j < triangles.Length; j = j + 3)
            {
                var sideA = plane.GetSide(original.vertices[triangles[j]]) == left;
                var sideB = plane.GetSide(original.vertices[triangles[j + 1]]) == left;
                var sideC = plane.GetSide(original.vertices[triangles[j + 2]]) == left;

                var sideCount = (sideA ? 1 : 0) +
                                (sideB ? 1 : 0) +
                                (sideC ? 1 : 0);
                if (sideCount == 0)
                {
                    continue;
                }
                if (sideCount == 3)
                {
                    partMesh.AddTriangle(i,
                                         original.vertices[triangles[j]], original.vertices[triangles[j + 1]], original.vertices[triangles[j + 2]],
                                         original.normals[triangles[j]], original.normals[triangles[j + 1]], original.normals[triangles[j + 2]],
                                         original.uv[triangles[j]], original.uv[triangles[j + 1]], original.uv[triangles[j + 2]]);
                    continue;
                }

                //cut points
                var singleIndex = sideB == sideC ? 0 : sideA == sideC ? 1 : 2;

                ray1.origin = original.vertices[triangles[j + singleIndex]];
                var dir1 = original.vertices[triangles[j + ((singleIndex + 1) % 3)]] - original.vertices[triangles[j + singleIndex]];
                ray1.direction = dir1;
                plane.Raycast(ray1, out var enter1);
                var lerp1 = enter1 / dir1.magnitude;

                ray2.origin = original.vertices[triangles[j + singleIndex]];
                var dir2 = original.vertices[triangles[j + ((singleIndex + 2) % 3)]] - original.vertices[triangles[j + singleIndex]];
                ray2.direction = dir2;
                plane.Raycast(ray2, out var enter2);
                var lerp2 = enter2 / dir2.magnitude;

                //first vertex = anchor
                AddEdge(i,
                        partMesh,
                        left ? plane.normal * -1f : plane.normal,
                        ray1.origin + ray1.direction.normalized * enter1,
                        ray2.origin + ray2.direction.normalized * enter2,
                        Vector2.Lerp(original.uv[triangles[j + singleIndex]], original.uv[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                        Vector2.Lerp(original.uv[triangles[j + singleIndex]], original.uv[triangles[j + ((singleIndex + 2) % 3)]], lerp2));

                if (sideCount == 1)
                {
                    partMesh.AddTriangle(i,
                                        original.vertices[triangles[j + singleIndex]],
                                        //Vector3.Lerp(originalMesh.vertices[triangles[j + singleIndex]], originalMesh.vertices[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        //Vector3.Lerp(originalMesh.vertices[triangles[j + singleIndex]], originalMesh.vertices[triangles[j + ((singleIndex + 2) % 3)]], lerp2),
                                        ray1.origin + ray1.direction.normalized * enter1,
                                        ray2.origin + ray2.direction.normalized * enter2,
                                        original.normals[triangles[j + singleIndex]],
                                        Vector3.Lerp(original.normals[triangles[j + singleIndex]], original.normals[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        Vector3.Lerp(original.normals[triangles[j + singleIndex]], original.normals[triangles[j + ((singleIndex + 2) % 3)]], lerp2),
                                        original.uv[triangles[j + singleIndex]],
                                        Vector2.Lerp(original.uv[triangles[j + singleIndex]], original.uv[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        Vector2.Lerp(original.uv[triangles[j + singleIndex]], original.uv[triangles[j + ((singleIndex + 2) % 3)]], lerp2));
                    
                    continue;
                }

                if (sideCount == 2)
                {
                    partMesh.AddTriangle(i,
                                        ray1.origin + ray1.direction.normalized * enter1,
                                        original.vertices[triangles[j + ((singleIndex + 1) % 3)]],
                                        original.vertices[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector3.Lerp(original.normals[triangles[j + singleIndex]], original.normals[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.normals[triangles[j + ((singleIndex + 1) % 3)]],
                                        original.normals[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector2.Lerp(original.uv[triangles[j + singleIndex]], original.uv[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.uv[triangles[j + ((singleIndex + 1) % 3)]],
                                        original.uv[triangles[j + ((singleIndex + 2) % 3)]]);
                    partMesh.AddTriangle(i,
                                        ray1.origin + ray1.direction.normalized * enter1,
                                        original.vertices[triangles[j + ((singleIndex + 2) % 3)]],
                                        ray2.origin + ray2.direction.normalized * enter2,
                                        Vector3.Lerp(original.normals[triangles[j + singleIndex]], original.normals[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.normals[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector3.Lerp(original.normals[triangles[j + singleIndex]], original.normals[triangles[j + ((singleIndex + 2) % 3)]], lerp2),
                                        Vector2.Lerp(original.uv[triangles[j + singleIndex]], original.uv[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.uv[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector2.Lerp(original.uv[triangles[j + singleIndex]], original.uv[triangles[j + ((singleIndex + 2) % 3)]], lerp2));
                }


            }
        }

        partMesh.FillArrays();

        return partMesh;
    }

    private void AddEdge(int subMesh, PartMesh partMesh, Vector3 normal, Vector3 vertex1, Vector3 vertex2, Vector2 uv1, Vector2 uv2)
    {
        if (!_edgeSet)
        {
            _edgeSet = true;
            _edgeVertex = vertex1;
            _edgeUV = uv1;
        }
        else
        {
            _edgePlane.Set3Points(_edgeVertex, vertex1, vertex2);

            partMesh.AddTriangle(subMesh,
                                _edgeVertex,
                                _edgePlane.GetSide(_edgeVertex + normal) ? vertex1 : vertex2,
                                _edgePlane.GetSide(_edgeVertex + normal) ? vertex2 : vertex1,
                                normal,
                                normal,
                                normal,
                                _edgeUV,
                                uv1,
                                uv2);
        }
    }

    private class PartMesh
    {
        private List<Vector3> _vertices = new();
        private List<Vector3> _normals = new();
        private List<List<int>> _triangles = new();
        private List<Vector2> _uvs = new();
        public Vector3[] vertices;
        public Vector3[] normals;
        public int[][] triangles;
        public Vector2[] uv;
        public GameObject gameObject;
        public Bounds bounds;
        

        public void AddTriangle(int submesh, Vector3 vert1, Vector3 vert2, Vector3 vert3, Vector3 normal1, Vector3 normal2, Vector3 normal3, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            if (_triangles.Count - 1 < submesh)
            {
                _triangles.Add(new List<int>());
            }
            _triangles[submesh].Add(_vertices.Count);
            _vertices.Add(vert1);
            _triangles[submesh].Add(_vertices.Count);
            _vertices.Add(vert2);
            _triangles[submesh].Add(_vertices.Count);
            _vertices.Add(vert3);
            _normals.Add(normal1);
            _normals.Add(normal2);
            _normals.Add(normal3);
            _uvs.Add(uv1);
            _uvs.Add(uv2);
            _uvs.Add(uv3);

            bounds.min = Vector3.Min(bounds.min, vert1);
            bounds.min = Vector3.Min(bounds.min, vert2);
            bounds.min = Vector3.Min(bounds.min, vert3);
            bounds.max = Vector3.Min(bounds.max, vert1);
            bounds.max = Vector3.Min(bounds.max, vert2);
            bounds.max = Vector3.Min(bounds.max, vert3);
        }

        public void FillArrays()
        {
            vertices = _vertices.ToArray();
            normals = _normals.ToArray();
            uv = _uvs.ToArray();
            triangles = new int[_triangles.Count][];
            for (var i = 0; i < _triangles.Count; i++)
            {
                triangles[i] = _triangles[i].ToArray();
            }
        }

        public void MakeGameObject(DestructibleObject original)
        {
            gameObject = new GameObject(original.name);
            gameObject.transform.position = original.transform.position;
            gameObject.transform.rotation = original.transform.rotation;
            gameObject.transform.localScale = original.transform.localScale;

            var mesh = new Mesh();
            mesh.name = original.GetComponent<MeshFilter>().mesh.name;

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;
            for(var i = 0; i < triangles.Length; i++)
                mesh.SetTriangles(triangles[i], i, true);
            bounds = mesh.bounds;
            
            var renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.materials = original.GetComponent<MeshRenderer>().materials;

            var filter = gameObject.AddComponent<MeshFilter>();
            filter.mesh = mesh;

            var collider = gameObject.AddComponent<MeshCollider>();
            collider.convex = true;

            var rigidbody = gameObject.AddComponent<Rigidbody>();

        }

    }
}
