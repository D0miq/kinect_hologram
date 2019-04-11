using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client : IClient
{
    private Socket sender;

    public Client(string ipString, int port)
    {
        try
        {
            // Establish the remote endpoint for the socket.
            IPAddress ipAddress = IPAddress.Parse(ipString);
            IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, port);

            // Creation TCP/IP Socket using Socket Class Costructor 
            Debug.Log("Create socket for TCP/IP to the server.");
            this.sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);        
            this.sender.Connect(remoteEndPoint);
        } catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    ~Client()
    {
        if(this.sender != null)
        {
            this.sender.Close();
            this.sender = null;
        }
    }

    public void Send(string message)
    {
        byte[] messageSent = Encoding.ASCII.GetBytes(message);

        try
        {
            if (this.sender != null)
            {
                int byteSent = this.sender.Send(messageSent);
                Debug.Log("Send: " + message);
            }
        } catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
