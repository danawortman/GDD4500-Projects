using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace GDD4500_ConsoleServerLab
{
    class Program
    {
        public static void ClientHandler(object clientSocket)
        {
            string message;
            byte[] buffer;

            Socket socket = (Socket)clientSocket;

            // While there are connections to open
            while (true)
            {
                message = null;

                while (true)
                {
                    // Allocate space for the message
                    buffer = new byte[1024];

                    // Receive a chunk of message over the socket
                    int bytesRec = socket.Receive(buffer);

                    // Convert the chunk to a string
                    message += Encoding.ASCII.GetString(buffer, 0, bytesRec);

                    // If the message has <EOF>, then we have reached its end
                    if (message.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }

                Console.WriteLine("Message received: {0}", message);

                // Send the message back to the client (for confirmation)
                byte[] msg = Encoding.ASCII.GetBytes(message);
                socket.Send(msg);
            }

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        public static void ListenMethod(object listenerSocket)
        {
            Socket listener = (Socket)listenerSocket;
            listener.Listen(10);

            while (true)
            {
                Console.WriteLine("Waiting for Connection...");
                
                // Create a new socket that will accept messages
                Thread thread = new Thread(ClientHandler);
                thread.Start(listener.Accept());
            }
        }
        static void Main(string[] args)
        {
            try
            {
                // IPAddress ipaddress = IPAddress.Loopback;
                IPAddress ipaddress = IPAddress.Parse("192.168.56.1");
                Int32 port = 5555;

                Console.WriteLine("Setting up server " + ipaddress.ToString() + ":" + port);

                // Create a socket to listen for initial connections
                Socket listener = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Create an address and port combo for this server
                IPEndPoint localEndPoint = new IPEndPoint(ipaddress, port);

                // Bind a socket to listen for new connections
                listener.Bind(localEndPoint);
                listener.Listen(10);

                Thread thread = new Thread(ListenMethod);
                thread.Start(listener);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.ReadKey();
        }
    }
}
