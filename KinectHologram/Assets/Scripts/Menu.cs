using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene("ServerScene");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("ClientScene");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene("LocalClientScene");
        }
    }
}
