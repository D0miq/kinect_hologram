using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class FlipCamera : MonoBehaviour
{
    new Camera camera;
    public bool flipHorizontal;
    public bool flipVertical;

    void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void OnPreCull()
    {
        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        Vector3 scale = new Vector3(1, 1, 1);

        if (flipHorizontal)
        {
            scale.x = -1;
        } else if (flipVertical)
        {
            scale.y = -1;
        }
        
        //camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(scale);
    }

    void OnPreRender()
    {
        GL.invertCulling = flipHorizontal || flipVertical;
    }

    void OnPostRender()
    {
        GL.invertCulling = false;
    }
}