using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class TCPServer : MonoBehaviour
{
    Thread tcpListenerThread;
    List<TcpClient> clientList;
    TcpListener server;
    bool isServerRunning = true;

    private void Start()
    {
        Debug.Log("ServerStart");
        clientList = new List<TcpClient>();

        tcpListenerThread = new Thread(new ThreadStart(ServerStart));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
    }

    private void ServerStart()
    {
        server = null;
        try
        {
            // Set the TcpListener on port 13000.
            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);
            // Start listening for client requests.
            server.Start();

            Debug.Log("Waiting for a connection... ");

            while (isServerRunning)
            {
                // Accept the TcpClient connection.
                TcpClient client = server.AcceptTcpClient();
                Debug.Log("Connect");
                // Start a new thread to handle communication with the client.
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientCommunication));
                clientThread.Start(client);

                //Client ¿˙¿Â
                clientList.Add(client);
            }
        }
        catch (SocketException e)
        {
            Debug.Log("SocketException: " + e);
        }
        finally
        {
            server.Stop();
        }
    }

    private void HandleClientCommunication(object clientObj)
    {
        TcpClient client = (TcpClient)clientObj;
        NetworkStream stream = client.GetStream();

        Byte[] bytes = new Byte[256];
        String data = null;

        int i;
        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
        {
            // Translate data bytes to a ASCII string.
            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            Debug.Log("Received: " + data);

            // Process the data sent by the client.
            data = data.ToUpper();

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

            // Send back a response.
            foreach (TcpClient allClient in clientList)
            {
                if (allClient == client)
                {
                    Debug.Log("same1");
                    continue;
                }
                NetworkStream allStream = allClient.GetStream();
                allStream.Write(msg, 0, msg.Length);
            }
            //stream.Write(msg,0, msg.Length);
            Debug.Log("Sent: " + data);
        }

        // Shutdown and close the client connection.
        client.Close();
        clientList.Remove(client);
    }

    public void ServerEnd()
    {
        isServerRunning = false;
        server.Stop();
    }
}
