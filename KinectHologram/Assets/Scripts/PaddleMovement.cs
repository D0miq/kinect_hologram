using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PaddleMovement : MonoBehaviour
{
    [SerializeField]
    private int averageCount;

    private Queue<Vector3> oldPositions = new Queue<Vector3>();
    private Queue<Quaternion> oldRotations = new Queue<Quaternion>();

    public void Move(Vector3 position)
    {
        this.oldPositions.Enqueue(position);
        if (this.oldPositions.Count == this.averageCount) {
            Vector3 sum = new Vector3(0,0,0);
            foreach(Vector3 v in this.oldPositions)
            {
                sum += v;
            }

            this.transform.position = sum / this.oldPositions.Count;
            Debug.Log(this.transform.position);
            this.oldPositions.Dequeue();
        }
    }

    public void Rotate(Quaternion quaternion) {
        /*this.oldRotations.Enqueue(quaternion);
        if (this.oldRotations.Count == this.averageCount) {
            Quaternion sum = new Quaternion();
            foreach(Quaternion q in this.oldRotations)
            {
                sum *= q;
            }

            this.transform.position = sum / this.oldRotations.Count;
            Debug.Log(this.transform.position);
            this.oldRotations.Dequeue();
        }*/
    }
}
