using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseInputClient : MonoBehaviour
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

        if(Input.GetButton("Fire1")) {
            //Debug.Log(paddle.transform.position + new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
            paddle.Position = paddle.transform.position + new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
    }
}
