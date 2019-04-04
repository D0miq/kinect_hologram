using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class RotateCharacter : MonoBehaviour
{
    public Transform character;
    public GameObject kinectManagerObject;

    private KinectManager kinectManager;
    //float degreesPerSecond = 50.0f;

    private void Start()
    {
        this.kinectManager = this.kinectManagerObject.GetComponent<KinectManager>();
    }


    void Update()
    {
        //Windows.Kinect.Vector4 vector = this.kinectManager.HeadRotation;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(-90f, 0f, 0f) * new Quaternion(-vector.X, -vector.Z, -vector.Y, vector.W), 0.1f);
        
        CameraSpacePoint headPosition = this.kinectManager.HeadPosition;
        Vector3 headVector = new Vector3(headPosition.X * 100, -headPosition.Y * 100, headPosition.Z * 100);

        Debug.Log(headVector);

        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(headVector) * Quaternion.Euler(-90f, 0f, 0f), 0.5f);
        transform.rotation = Quaternion.LookRotation(headVector) * Quaternion.Euler(-90f, 0f, 0f);
    }
}
