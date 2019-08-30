using UnityEngine;
using System.Collections;
using Windows.Kinect;
using Microsoft.Kinect.Face;
using System.Linq;
using UnityEditor;

public class KinectManager : MonoBehaviour
{
    public IClient NetworkClient;
    public float MaxZ;
    public float angle;
    public float XOffset;
    public float ZOffset;

    private Matrix4x4 transformMatrix;
    private Quaternion rotation;
    private KinectSensor sensor;
    private BodyFrameReader bodyReader;
    private FaceFrameReader faceReader;
    private FaceFrameSource faceSource;
  
    void Start()
    {
        this.rotation = Quaternion.Euler(0, angle, 0);
        this.transformMatrix = Matrix4x4.Scale(new Vector3(1, 1, -1)) * Matrix4x4.Translate(new Vector3(this.XOffset, 0, this.ZOffset)) * Matrix4x4.Rotate(Quaternion.Euler(0, angle, 0));
        //new Matrix4x4(new UnityEngine.Vector4(10*cos, 0, 10*sin, 0), new UnityEngine.Vector4(0, 10, 0, 0), new UnityEngine.Vector4(10*sin, 0, -10*cos, 0), new UnityEngine.Vector4(XOffset, 0, -ZOffset, 1));

        this.sensor = KinectSensor.GetDefault();
        if (this.sensor != null)
        {
            this.bodyReader = this.sensor.BodyFrameSource.OpenReader();

            if(this.faceSource != null)
            {
                this.faceReader = this.faceSource.OpenReader();
            } else
            {
                Debug.Log("Face reader has not been opened.");
            }        

            if (!this.sensor.IsOpen)
            {
                Debug.Log("Kinect has been started.");
                this.sensor.Open();
            }
        } else
        {
            Debug.Log("Kinect has not been found.");
        }
    }

    void Update()
    {
        CameraSpacePoint headPosition = new CameraSpacePoint();
        Windows.Kinect.Vector4 headRotation = new Windows.Kinect.Vector4();
        CameraSpacePoint handPosition = new CameraSpacePoint();
        Windows.Kinect.Vector4 handRotation = new Windows.Kinect.Vector4();

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
                    handPosition = body.Joints[JointType.HandRight].Position;
                    handRotation = body.JointOrientations[JointType.HandRight].Orientation;

                    if (this.faceSource != null && !this.faceSource.IsTrackingIdValid)
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

        if (headPosition.Z < this.MaxZ)
        {
            UnityEngine.Vector4 transformedHeadPosition;
            UnityEngine.Vector4 transformedHandPosition;
            Quaternion handQuaternionRotation;

            if (headPosition.X != 0 && headPosition.Y != 0 && headPosition.Z != 0)
            {
                transformedHeadPosition = this.transformMatrix * new UnityEngine.Vector4(headPosition.X, headPosition.Y, headPosition.Z, 1);
                transformedHandPosition = this.transformMatrix * new UnityEngine.Vector4(handPosition.X, handPosition.Y, handPosition.Z, 1);
                handQuaternionRotation = new Quaternion(handRotation.X, handRotation.Y, handRotation.Z, handRotation.W) * this.rotation;
            }
            else
            {
                transformedHeadPosition = UnityEngine.Vector4.zero;
                transformedHandPosition = UnityEngine.Vector4.zero;
                handQuaternionRotation = new Quaternion();
            }

            this.NetworkClient.Send("" + transformedHeadPosition.x + ';' + transformedHeadPosition.y + ';' + transformedHeadPosition.z + ';' + headRotation.X + ';' + headRotation.Y + ';' + headRotation.Z + ';' + headRotation.W + ';' + transformedHandPosition.x + ';' + transformedHandPosition.y + ';' + transformedHandPosition.z + ';' + handQuaternionRotation.x + ';' + handQuaternionRotation.y + ';' + handQuaternionRotation.z + ';' + handQuaternionRotation.w);
            //this.NetworkClient.Send("" + -transformedHeadPosition.x + ';' + transformedHeadPosition.y + ';' + transformedHeadPosition.z + ';' + headRotation.X + ';' + headRotation.Y + ';' + headRotation.Z + ';' + headRotation.W + ';' + -transformedHandPosition.x + ';' + transformedHandPosition.y + ';' + transformedHandPosition.z);

        }       
    }

    void OnDestroy()
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
