using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PaddleMovement : MonoBehaviour
{
    private Queue<Vector3> oldPositions = new Queue<Vector3>();

    public void Move(Vector3 position)
    {
        position.x = -position.x;
        oldPositions.Enqueue(position);
        if (oldPositions.Count == 4) {
            Vector3 sum = new Vector3(0,0,0);
            foreach(Vector3 v in oldPositions)
            {
                sum += v;
            }

            this.transform.position = sum / oldPositions.Count;
            Debug.Log(this.transform.position);
            oldPositions.Dequeue();
        }

    }
}
