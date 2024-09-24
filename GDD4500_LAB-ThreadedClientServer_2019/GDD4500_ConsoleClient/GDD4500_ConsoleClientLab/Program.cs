using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace GDD4500_ConsoleClientLab
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IPAddress ipaddress = IPAddress.Parse("192.168.0.31");
                Int32 port = 5555;

                Console.WriteLine("Connecting to server " + ipaddress.ToString() + ":" + port);
                
                // Create a socket to listen for initial connections
                Socket sender = new Socket(AddressFamily.InterNetwork,
                                           SocketType.Stream, 
                                           ProtocolType.Tcp);

                // Create an address and port combo for this server
                IPEndPoint remoteEndPoint = new IPEndPoint(ipaddress, port);

                // Bind a socket to listen for new connections
                sender.Connect(remoteEndPoint);

                Console.WriteLine("Connected to {0}",
                    sender.RemoteEndPoint.ToString());

                // Encode the message into a byte array
                byte[] msg = Encoding.ASCII.GetBytes("Hello Online World<EOF>");

                // Send the data through the socket
                int bytesSent = sender.Send(msg);

                // Receive the response from the remote device
                byte[] buffer = new byte[1024];
                int bytesRec = sender.Receive(buffer);

                Console.WriteLine("Received message: {0}", 
                    Encoding.ASCII.GetString(buffer, 0, bytesRec));

                // Release the socket
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.ReadKey();
        }
    }
}
