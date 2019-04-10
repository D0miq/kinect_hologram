using UnityEngine;
using System.Collections;
using Windows.Kinect;
using Microsoft.Kinect.Face;
using System.Linq;

public class KinectManager : MonoBehaviour
{
    private KinectSensor sensor;
    private BodyFrameReader bodyReader;
    private FaceFrameReader faceReader;
    private FaceFrameSource faceSource;
    private NetworkManager networkManager;

    void Start()
    {
        this.sensor = KinectSensor.GetDefault();
        if (this.sensor != null)
        {
            this.bodyReader = this.sensor.BodyFrameSource.OpenReader();
            this.faceSource = FaceFrameSource.Create(this.sensor, 0, FaceFrameFeatures.RotationOrientation);
            this.faceReader = this.faceSource.OpenReader();           

            if (!this.sensor.IsOpen)
            {
                this.sensor.Open();
            }
        }

        this.networkManager = FindObjectOfType<NetworkManager>();
    }

    void Update()
    {
        CameraSpacePoint headPosition = new CameraSpacePoint();
        Windows.Kinect.Vector4 headRotation = new Windows.Kinect.Vector4();

        if (this.bodyReader != null)
        {
            var frame = this.bodyReader.AcquireLatestFrame();
            if (frame != null)
            {
                Debug.Log("Body frame acquired.");
                Body[] bodies = new Body[frame.BodyCount];
                frame.GetAndRefreshBodyData(bodies);
                Body body = bodies.Where(b => b.IsTracked).FirstOrDefault();

                if (body != null)
                {
                    headPosition = body.Joints[JointType.Head].Position;

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
                Debug.Log("Face frame acquired.");
                FaceFrameResult result = frame.FaceFrameResult;
                if(result != null)
                {
                    headRotation = result.FaceRotationQuaternion;
                }
            }
        }

        this.networkManager.Send("" + headPosition.X + ';' + headPosition.Y + ';' + headPosition.Z + ';' + headRotation.X + ';' + headRotation.Y + ';' + headRotation.Z + ';' + headRotation.W);
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
