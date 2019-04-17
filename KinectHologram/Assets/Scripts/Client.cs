using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : IClient
{
    public string IpAddress;
    public int Port;

    private Socket sender;

    private void Awake()
    {
        try
        {
            // Establish the remote endpoint for the socket.
            IPAddress ipAddress = IPAddress.Parse(this.IpAddress);
            IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, this.Port);

            // Creation TCP/IP Socket using Socket Class Costructor 
            Debug.Log("Create socket for TCP/IP to the server.");
            this.sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.sender.Connect(remoteEndPoint);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            this.sender = null;
        }
    }

    private void OnDestroy()
    {
        if(this.sender != null)
        {
            this.sender.Close();
            this.sender = null;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene");
        }
    }

    public override void Send(string message)
    {
        byte[] messageSent = Encoding.ASCII.GetBytes(message);

        try
        {
            if (this.sender != null)
            {
                Debug.Log("Send: " + message);
                int byteSent = this.sender.Send(messageSent);
                Debug.Log("Sent bytes: " + byteSent);
            }
        } catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
