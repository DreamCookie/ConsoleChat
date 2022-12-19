using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Server
    {
        // List to store connected clients
        private static List<TcpClient> clients = new List<TcpClient>();

        static void Main(string[] args)
        {
            // Start the server and listen for incoming connections
            TcpListener server = new TcpListener(System.Net.IPAddress.Any, 5000);
            server.Start();
            Console.WriteLine("Server started. Waiting for connections...");

            while (true)
            {
                // Accept an incoming connection
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client connected.");

                // Add the client to the list of connected clients
                clients.Add(client);

                // Start a new thread to listen for messages from this client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.Start(client);
            }
        }

        // Method to handle communication with a client
        private static void HandleClient(object obj)
        {
            // Get the client object and its network stream
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();

            // Use a stream reader and writer to read and write data to the stream
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);

            while (true)
            {
                // Read a message from the client
                string message = reader.ReadLine();

                // Send the message to all connected clients
                SendMessageToAll(message);
            }
        }

        // Method to send a message to all connected clients
        private static void SendMessageToAll(string message)
        {
            // Loop through all connected clients
            foreach (TcpClient c in clients)
            {
                // Get the client's network stream
                NetworkStream stream = c.GetStream();

                // Use a stream writer to write the message to the stream
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(message);
                writer.Flush();
            }
        }
    }
}
