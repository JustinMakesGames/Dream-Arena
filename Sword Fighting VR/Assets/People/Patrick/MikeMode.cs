using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class MikeMode : MonoBehaviour {
    private Material _material;

    public Shader shader;
    // Creates a private material used to the effect
    void Awake ()
    {
        _material = new Material(shader);
    }
    
    // Postprocess the image
    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit (source, destination, _material);
    }
}