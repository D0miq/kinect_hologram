using UnityEngine;
using System.Collections;
using Windows.Kinect;
using Microsoft.Kinect.Face;
using System.Linq;

public class KinectManager : MonoBehaviour
{
    public GameObject ErrorMessage;

    private KinectSensor sensor;
    private BodyFrameReader bodyReader;
    private FaceFrameReader faceReader;
    private FaceFrameSource faceSource;

    public Windows.Kinect.Vector4 HeadRotation
    {
        get;
        private set;
    }

    public CameraSpacePoint HeadPosition
    {
        get;
        private set;
    }

    public 

    void Start()
    {
        this.sensor = KinectSensor.GetDefault();
        if (this.sensor != null)
        {
            this.bodyReader = this.sensor.BodyFrameSource.OpenReader();
            this.faceSource = FaceFrameSource.Create(this.sensor, 0, FaceFrameFeatures.RotationOrientation);
            this.faceReader = this.faceSource.OpenReader();           

            if (!sensor.IsOpen)
            {
                sensor.Open();
            }
        } else
        {
            ErrorMessage.SetActive(true);
        }
    }

    void Update()
    {
        if (this.bodyReader != null)
        {
            var frame = this.bodyReader.AcquireLatestFrame();
            if (frame != null)
            {
                Body[] bodies = new Body[frame.BodyCount];
                frame.GetAndRefreshBodyData(bodies);
                Body body = bodies.Where(b => b.IsTracked).FirstOrDefault();

                if (body != null)
                {
                    this.HeadPosition = body.Joints[JointType.Head].Position;

                    if (!this.faceSource.IsTrackingIdValid)
                    {
                        this.faceSource.TrackingId = body.TrackingId;
                    }
                }

                frame.Dispose();
                frame = null;
            }
        }

        if (this.faceReader != null)
        {
            var frame = this.faceReader.AcquireLatestFrame();

            if (frame != null)
            {
                FaceFrameResult result = frame.FaceFrameResult;
                if(result != null)
                {
                    this.HeadRotation = result.FaceRotationQuaternion;
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        if (this.faceReader != null)
        {
            this.faceReader.Dispose();
            this.faceReader = null;
        }

        if (this.bodyReader != null)
        {
            this.bodyReader.Dispose();
            this.bodyReader = null;
        }

        if (this.sensor != null)
        {
            if (this.sensor.IsOpen)
            {
                this.sensor.Close();
            }

            this.sensor = null;
        }
    }
}
