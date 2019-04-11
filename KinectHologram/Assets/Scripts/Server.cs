using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server
{
    private Socket listener;
    private List<Socket> sockets;
    private bool running = false;

    public Server(string ipString, int port, int maxRequests)
    {
        this.sockets = new List<Socket>();

        try
        {
            IPAddress ipAddress = IPAddress.Parse(ipString);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            Debug.Log("Creating a new server socket.");
            // Create a Socket that will use Tcp protocol      
            this.listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.sockets.Add(this.listener);
        
            // A Socket must be associated with an endpoint using the Bind method
            Debug.Log("Binding the server socket.");
            this.listener.Bind(localEndPoint);

            // Specify how many requests a Socket can listen before it gives Server busy response  
            Debug.Log("Starting listening on the server socket.");
            this.listener.Listen(maxRequests);

            this.running = true;
        } catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    ~Server()
    {
        // Close all sockets even a server socket and delete all client sockets
        if(this.sockets != null)
        {
            this.sockets.ForEach(socket => socket.Close());
            this.sockets.Clear();
            this.sockets = null;
        }

        // Server socket is already closed, just delete it
        if(this.listener != null)
        {
            this.listener = null;
        }
    }

    public void ProcessInput()
    {
        if(!running)
        {
            return;
        }

        try
        {
            List<Socket> tempSockets = new List<Socket>(sockets);
            Socket.Select(tempSockets, null, null, -1);
            if (tempSockets.Contains(this.listener))
            {
                Socket clientSocket = this.listener.Accept();
                this.sockets.Add(clientSocket);
                Debug.Log("Connecting a new client to the server.");
            }

            foreach (Socket socket in tempSockets)
            {
                // Data buffer 
                byte[] bytes = new Byte[1024];
                int numByte = socket.Receive(bytes);

                if (numByte != 0)
                {
                    // TODO : zpracování vstupu
                } else
                {
                    socket.Close();
                    this.sockets.Remove(socket);
                    Debug.Log("Disconnecting a client from the server.");
                }
            }
        } catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
