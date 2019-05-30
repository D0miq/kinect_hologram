using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float ZOffset;

    private new Camera camera;
    private Vector3 oldCameraPosition;
    private float oldFocalLength;
    private Vector2 oldLensShift;

    private void Start()
    {
        this.camera = GetComponent<Camera>();
    }

    public void Move(Vector3 position)
    {
        this.oldCameraPosition = this.transform.position;
        this.oldFocalLength = this.camera.focalLength;
        this.oldLensShift = this.camera.lensShift;

        this.transform.position = new Vector3(-position.x, position.y, -position.z - ZOffset);
        this.camera.focalLength = (position.z + ZOffset) * 1000;
        this.camera.lensShift = new Vector2(position.x * 1000 / this.camera.sensorSize.x, -position.y * 1000 / this.camera.sensorSize.y);

    }

    public void Rotate(Quaternion rotation)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(-rotation.x, -rotation.z, -rotation.y, rotation.w), 0.1f);
    }
}
