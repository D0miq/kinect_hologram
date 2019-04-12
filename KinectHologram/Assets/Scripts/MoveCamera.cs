using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float ZOffset;

    private new Camera camera;

    private void Start()
    {
        this.camera = GetComponent<Camera>();
    }

    public void Move(Vector3 position)
    {
        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(position.x, position.y, -position.z - ZOffset), 0.2f);
        this.camera.focalLength = Mathf.Lerp(this.camera.focalLength, (position.z + ZOffset) * 1000, 0.2f);
        this.camera.lensShift = Vector2.Lerp(this.camera.lensShift, new Vector2(-position.x * 1000 / this.camera.sensorSize.x, -position.y * 1000 / this.camera.sensorSize.y), 0.2f);
    }

    public void Rotate(Quaternion rotation)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(-rotation.x, -rotation.z, -rotation.y, rotation.w), 0.1f);
    }
}
