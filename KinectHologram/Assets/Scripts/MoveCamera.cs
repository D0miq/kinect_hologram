using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class MoveCamera : MonoBehaviour
{
    public float ZOffset;

    public GameObject kinectManagerObject;

    private KinectManager kinectManager;

    private new Camera camera;

    private void Start()
    {
        this.kinectManager = this.kinectManagerObject.GetComponent<KinectManager>();
        this.camera = GetComponent<Camera>();
    }

    void Update()
    {
        //Windows.Kinect.Vector4 vector = this.kinectManager.HeadRotation;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-90f, 0f, 0f) * new Quaternion(-vector.X, -vector.Z, -vector.Y, vector.W), 0.1f);

        CameraSpacePoint headPosition = this.kinectManager.HeadPosition;

        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(headPosition.X, headPosition.Y, -headPosition.Z - ZOffset), 0.2f);
        this.camera.focalLength = Mathf.Lerp(this.camera.focalLength, (headPosition.Z + ZOffset) * 1000, 0.2f);
        this.camera.lensShift = Vector2.Lerp(this.camera.lensShift, new Vector2(-headPosition.X * 1000 / this.camera.sensorSize.x, -headPosition.Y * 1000 / this.camera.sensorSize.y), 0.2f);
    }
}
