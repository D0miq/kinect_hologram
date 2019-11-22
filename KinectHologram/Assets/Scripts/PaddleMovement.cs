using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PaddleMovement : MonoBehaviour
{
    [SerializeField]
    private int averageCount;

    private Queue<Vector3> rawPositions = new Queue<Vector3>();
    private Queue<Quaternion> rawRotations = new Queue<Quaternion>();
    private Rigidbody _rigidbody;
    private Vector3 velocityBeforeCollision;
    private Vector3 position;
    private Vector3 oldPosition;
    private Vector3 force;
    private Vector3 oldVelocity;
    private float newTime;
    private float oldTime;
    private float oldOldTime;
    private Vector3 newVelocity;

    public Vector3 Position {
        get => this.position;
        set {
            this.rawPositions.Enqueue(value);
            if (this.rawPositions.Count == this.averageCount) {
                Vector3 sum = new Vector3(0,0,0);
                foreach(Vector3 v in this.rawPositions)
                {
                    sum += v;
                }

                // Filter new position
                Vector3 newPosition = sum / this.rawPositions.Count;

                // Set new time
                this.oldOldTime = this.oldTime;
                this.oldTime = this.newTime;
                this.newTime = Time.time;

                // Set new velocity
                this.oldVelocity = this.newVelocity;
                this.newVelocity = (newPosition - this.position) / (this.newTime - this.oldTime);
                
                // Set new position
                this.position = newPosition;

                Debug.Log("newPosition: " + this.position);
                this.rawPositions.Dequeue();
            }
        }
    }

    public Vector3 Force {
        get {
            Vector3 acceleration = (this.newVelocity - this.oldVelocity) / (this.newTime - this.oldOldTime);
            return this._rigidbody.mass * acceleration;
        }
    }

    public Quaternion Rotation {
        get => this.transform.rotation;
        set {
            //this.transform.rotation = quaternion;
        }
    }

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        this._rigidbody.position = this.position;
    }
}
