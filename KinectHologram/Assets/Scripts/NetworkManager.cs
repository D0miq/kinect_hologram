using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public int Port;
    public int MaxRequests;
    public string ServerIpAddress = "localhost";

    private Server server;
    private IClient client; 
    
    private bool atStart = true;
    
    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.atStart)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                this.server = new Server(this.ServerIpAddress, this.Port, this.MaxRequests);
                this.atStart = false;
                Time.timeScale = 1.0f;
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                this.client = new Client(this.ServerIpAddress, this.Port);
                this.atStart = false;
                Time.timeScale = 1.0f;
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                this.client = new LocalClient();
                this.atStart = false;
                Time.timeScale = 1.0f;
            }
        }
        
        if(this.server != null)
        {
            this.server.ProcessInput();
        }
    }

    public void Send(string message)
    {
        if(this.client != null)
        {
            this.client.Send(message);
        }
    }
}
