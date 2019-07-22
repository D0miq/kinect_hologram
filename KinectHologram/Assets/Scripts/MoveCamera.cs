using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private new Camera camera;

    private void Start()
    {
        this.camera = GetComponent<Camera>();
    }

    public void Move(Vector3 position)
    {
        this.transform.position = position;
        this.camera.focalLength = (-position.z) * 1000;
        this.camera.lensShift = new Vector2(-position.x * 1000 / this.camera.sensorSize.x, -position.y * 1000 / this.camera.sensorSize.y);

    }

    public void Rotate(Quaternion rotation)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(-rotation.x, -rotation.z, -rotation.y, rotation.w), 0.1f);
    }
}
