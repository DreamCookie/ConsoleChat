using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    class Client
    {
        static void Main(string[] args)
        {
            // Connect to the server
            TcpClient client = new TcpClient("localhost", 5000);
            Console.WriteLine("Connected to server.");

            // Start a new thread to listen for incoming messages
            Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
            receiveThread.Start();

            // Use a stream reader and writer to read and write data to the stream
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);

            // Send messages to the server
            while (true)
            {
                Console.Write("Enter message: ");
                string message = Console.ReadLine();

                // Write the message to the stream
                writer.WriteLine(message);
                writer.Flush();
            }
        }

        // Method to receive incoming messages
        private static void ReceiveMessage()
        {
            // Connect to the server
            TcpClient client = new TcpClient("localhost", 5000);
            NetworkStream stream = client.GetStream();

            // Use a stream reader to read data from the stream
            StreamReader reader = new StreamReader(stream);

            while (true)
            {
                // Read a message from the stream
                string message = reader.ReadLine();

                // Print the message to the console
                Console.WriteLine(message);
            }
        }
    }
}
