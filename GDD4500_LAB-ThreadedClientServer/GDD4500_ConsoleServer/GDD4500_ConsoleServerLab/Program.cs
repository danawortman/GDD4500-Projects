using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace GDD4500_ConsoleServerLab
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // IPAddress ipaddress = IPAddress.Loopback;
                IPAddress ipaddress = IPAddress.Parse("192.168.0.31");
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

                //string message = null;
                // While there are connections to open
                //while (true)
                //{
                    byte[] buffer = new byte[1024];

                    //message = "";

                    Console.WriteLine("Waiting for Connection...");

                    // Create a new socket that will accept messages
                    Socket socket = listener.Accept();

                    //while (true)
                    //{
                        // Allocate space for the message
                        buffer = new byte[1024];

                        // Receive a chunk of message over the socket
                        int bytesRec = socket.Receive(buffer);

                        // Convert the chunk to a string
                        string message = Encoding.ASCII.GetString(buffer, 0, bytesRec);

                        //// If the message has <EOF>, then we have reached its end
                        //if (message.IndexOf("<EOF>") > -1)
                        //{
                        //    break;
                        //}
                    //}

                    Console.WriteLine("Message received: {0}", message);

                    // Send the message back to the client (for confirmation)
                    byte[] msg = Encoding.ASCII.GetBytes(message);

                    socket.Send(msg);

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                //}

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.ReadKey();
        }
    }
}
