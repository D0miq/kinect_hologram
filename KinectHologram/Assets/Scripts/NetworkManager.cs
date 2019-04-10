using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public int Port;
    public int MaxRequests;
    public string ServerIpAddress = "localhost";

    private Socket listener;
    private Socket sender;

    private bool isConnected = false;
    private bool activeServer = false;
    private bool atStart = true;

    // Update is called once per frame
    void Update()
    {
        if(this.atStart)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                this.SetupServer(this.ServerIpAddress);
                this.atStart = false;
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                this.SetupClient(this.ServerIpAddress);
                this.atStart = false;
                this.isConnected = true;
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                this.SetupServer("localhost");
                this.SetupClient("localhost");
                this.atStart = false;
                this.isConnected = true;
            }
        }

        if (this.activeServer)
        {
            try
            {
                // Suspend while waiting for incoming connection Using Accept() method the server  
                // will accept connection of client 
                Socket clientSocket = this.listener.Accept();

                // Data buffer 
                byte[] bytes = new Byte[1024];

                int numByte = clientSocket.Receive(bytes);

                // TODO: zpracování
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }           
        }        
    }

    public void Send(string message)
    {
        byte[] messageSent = Encoding.ASCII.GetBytes(message);

        try
        {
            if(this.isConnected)
            {
                int byteSent = this.sender.Send(messageSent);
                Debug.Log("Send: " + message);
            }
        } catch(Exception e)
        {
            Debug.Log(e.ToString());
        }       
    }

    private void SetupServer(string hostNameOrAddress)
    {
        // Get Host IP Address that is used to establish a connection
        // If a host has multiple addresses, you will get a list of addresses  
        IPHostEntry host = Dns.GetHostEntry(hostNameOrAddress);
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, this.Port);

        // Create a Socket that will use Tcp protocol      
        this.listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            // A Socket must be associated with an endpoint using the Bind method  
            this.listener.Bind(localEndPoint);
            // Specify how many requests a Socket can listen before it gives Server busy response.  
            // We will listen 10 requests at a time  
            this.listener.Listen(MaxRequests);
            // A server was started
            this.activeServer = true;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void SetupClient(string serverNameorAddress)
    {
        // Establish the remote endpoint for the socket.
        IPHostEntry ipHost = Dns.GetHostEntry(serverNameorAddress);
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint remoteEndPoint = new IPEndPoint(ipAddr, this.Port);

        // Creation TCP/IP Socket using  
        // Socket Class Costructor 
        this.sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            sender.Connect(remoteEndPoint);
        } catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void OnApplicationQuit()
    {
        if(this.listener != null)
        {
            this.listener.Close();
        }

        if(this.sender != null)
        {
            this.sender.Close();
        }
    }
}
