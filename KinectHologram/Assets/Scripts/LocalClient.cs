using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalClient : IClient
{
    [SerializeField]
    private MoveCamera Camera;

    [SerializeField]
    private PaddleMovement paddle;

    [SerializeField]
    private bool rotate = false;
    
    [SerializeField]
    private bool move = true;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            this.rotate = !this.rotate;
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            this.move = !this.move;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene");
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("LocalClientScene");
        }
    }

    public override void Send(string message)
    {
        string[] values = message.Split(';');
        Vector3 headPosition;
        Quaternion headRotation;
        Vector3 handPosition;
        Quaternion handRotation;

        try
        {
            headPosition = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
            headRotation = new Quaternion(float.Parse(values[3]), float.Parse(values[4]), float.Parse(values[5]), float.Parse(values[6]));
            handPosition = new Vector3(float.Parse(values[7]), float.Parse(values[8]), float.Parse(values[9]));
            handRotation = new Quaternion(float.Parse(values[10]), float.Parse(values[11]), float.Parse(values[12]), float.Parse(values[13]));

            Debug.Log("Head position: " + headPosition);
            //Debug.Log("Head rotation: " + headRotation);
            Debug.Log("Hand position: " + handPosition);
            //Debug.Log("Hand rotation:" + handRotation);
        } catch(Exception e)
        {
            Debug.Log(e.ToString());
            return;
        }

        if(move && headPosition != Vector3.zero)
        {
            this.Camera.Move(headPosition);
        }

        if(rotate)
        {
            this.Camera.Rotate(headRotation);
        }

        if(handPosition != Vector3.zero) {
            this.paddle.Move(handPosition);
        }
        
        this.paddle.Rotate(handRotation);
    }
}
