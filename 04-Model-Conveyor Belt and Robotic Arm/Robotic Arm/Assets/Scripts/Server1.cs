using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

/*
 * Server1.cs
 * TCP Server implementation for Unity-based robotic arm control
 * 
 * This server:
 * - Runs on a separate thread to avoid blocking Unity's main thread
 * - Listens for TCP connections on localhost:55001
 * - Handles client messages and sends responses
 * - Supports basic message exchange protocol
 */

public class Server1 : MonoBehaviour {
    // TCP networking components
    TcpListener server = null; // Listens for incoming connections
    TcpClient client = null;  // Handles active client connection
    NetworkStream stream = null; // Manages data transfer
    Thread thread;          // Server thread to avoid blocking main Unity thread

    private void Start() {
        // Unity Start method - Initializes and starts server thread
        thread = new Thread(new ThreadStart(SetupServer));
        thread.Start();
    }

    // Unity Update method - Handles input for testing server
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SendMessageToClient("Hello");
        }
    }

    // Main server logic running on separate thread
    // Handles client connections and message processing
    private void SetupServer() {
        // Define the server address and port
        string server_address = "127.0.0.1";
        int server_port = 55001;

        try {
           // Initialize and start TCP listener
            IPAddress localAddr = IPAddress.Parse(server_address);
            server = new TcpListener(localAddr, server_port);
            server.Start();

            byte[] buffer = new byte[1024];
            string data = null;

            // Main server loop - waits for client connections and processes messages
            while (true) {
                // Wait for client connection
                Debug.Log("Waiting for connection...");
                client = server.AcceptTcpClient();
                Debug.Log("Someone has connected!");

                data = null;
                stream = client.GetStream();

                // Receive and process messages from client
                int i;
                while ((i = stream.Read(buffer, 0, buffer.Length)) != 0) {
                    // Server processes received data
                    data = Encoding.UTF8.GetString(buffer, 0, i);
                    Debug.Log("Server receives: " + data);

                    // Server sends a response to client
                    string response = data.ToString();
                    SendMessageToClient(message: response);
                }
                client.Close();
            }
        }
        catch (SocketException e) {
            Debug.Log("SocketException: " + e);
        }
        finally {
            server.Stop();
        }
    }

    private void OnApplicationQuit() {
        stream.Close();
        client.Close();
        server.Stop();
        thread.Abort();
    }

    // Sends message to connected client
    public void SendMessageToClient(string message) {
        byte[] msg = Encoding.UTF8.GetBytes(message);
        stream.Write(msg, 0, msg.Length);
        Debug.Log("Server send: " + message);
    }
}