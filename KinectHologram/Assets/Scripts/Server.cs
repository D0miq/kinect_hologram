﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{
    public Camera Camera;
    public int Port;
    public int MaxRequests;

    private Socket listener;
    private List<Socket> sockets;
    private bool running = false;
    private Dictionary<Socket, Camera> socketCamera;
    
    private void Awake()
    {
        this.sockets = new List<Socket>();
        this.socketCamera = new Dictionary<Socket, Camera>();

        try
        {
            IPAddress ipAddress = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, this.Port);

            Debug.Log("Creating a new server socket.");
            // Create a Socket that will use Tcp protocol      
            this.listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.sockets.Add(this.listener);

            // A Socket must be associated with an endpoint using the Bind method
            Debug.Log("Binding the server socket.");
            this.listener.Bind(localEndPoint);

            // Specify how many requests a Socket can listen before it gives Server busy response  
            Debug.Log("Starting listening on the server socket.");
            this.listener.Listen(this.MaxRequests);

            this.running = true;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void OnDestroy()
    {
        // Close all sockets even a server socket and delete all client sockets
        if (this.sockets != null)
        {
            this.sockets.ForEach(socket => socket.Close());
            this.sockets.Clear();
            this.sockets = null;
        }

        // Server socket is already closed, just delete it
        if (this.listener != null)
        {
            this.listener = null;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene");
        }

        if (!running)
        {
            return;
        }

        try
        {
            List<Socket> tempSockets = new List<Socket>(sockets);
            Socket.Select(tempSockets, null, null, 10);
            if (tempSockets.Contains(this.listener))
            {
                tempSockets.Remove(this.listener);
                Socket clientSocket = this.listener.Accept();
                this.sockets.Add(clientSocket);
                Debug.Log("Connecting a new client to the server.");
            }

            foreach (Socket socket in tempSockets)
            {
                Debug.Log("TempSockets: " + tempSockets.ToString());
                Debug.Log("Connected: " + socket.Connected);
                Debug.Log("" + socket.AddressFamily.ToString());

                // Data buffer 
                byte[] bytes = new Byte[1024];
                int numByte = socket.Receive(bytes);

                if (numByte != 0)
                {
                    // TODO : zpracování vstupu
                }
                else
                {
                    socket.Close();
                    this.sockets.Remove(socket);
                    Debug.Log("Disconnecting a client from the server.");
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
