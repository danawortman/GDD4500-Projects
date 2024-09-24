using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class AsynchIOServer
    {
        public static void Main()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5555);
            tcpListener.Start();

            // put beginning of while loop here!
            TcpClient client = tcpListener.AcceptTcpClient();
            if (client.Connected)
            {
                Console.WriteLine("Client connected");
                NetworkStream networkStream = client.GetStream();
                System.IO.StreamWriter streamWriter =
                            new System.IO.StreamWriter(networkStream);
                System.IO.StreamReader streamReader =
                         new System.IO.StreamReader(networkStream);
                
                string theString = "Hello, I am the server";
                streamWriter.WriteLine(theString);
                Console.WriteLine(theString);
                streamWriter.Flush();
                theString = streamReader.ReadLine();
                Console.WriteLine(theString);
                streamReader.Close();
                networkStream.Close();
                streamWriter.Close();
            }
            client.Close();
            Console.WriteLine("Exiting...");
            Console.ReadKey();
        }
    }

}