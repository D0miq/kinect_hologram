using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalClient : IClient
{
    public MoveCamera Camera;
    public PaddleMovement paddle;

    private bool rotate = false;
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
    }

    public override void Send(string message)
    {
        string[] values = message.Split(';');
        Vector3 headPosition;
        Quaternion headRotation;
        Vector3 handPosition;

        try
        {
            headPosition = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
            headRotation = new Quaternion(float.Parse(values[3]), float.Parse(values[4]), float.Parse(values[5]), float.Parse(values[6]));
            handPosition = new Vector3(float.Parse(values[7]), float.Parse(values[8]), float.Parse(values[9]));
            //Debug.Log("Head position: " + headPosition);
            //Debug.Log("Head rotation: " + headRotation);
            Debug.Log("Hand position: " + handPosition);
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

        this.paddle.Move(handPosition);
    }
}
