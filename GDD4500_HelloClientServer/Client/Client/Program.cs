using System;
using System.Net.Sockets;

namespace Client
{
    public class Client
    {
        static public void Main(string[] Args)
        {
            string ipAddress = "127.0.0.1";
            int portNbr = 5555;

            TcpClient socketForServer;
            try
            {
                socketForServer = new TcpClient(ipAddress, portNbr);
            }
            catch
            {
                Console.WriteLine(
                "Failed to connect to server at {0}:{1}", ipAddress, portNbr);
                return;
            }
            NetworkStream networkStream = socketForServer.GetStream();
            System.IO.StreamReader streamReader =
            new System.IO.StreamReader(networkStream);
            System.IO.StreamWriter streamWriter =
            new System.IO.StreamWriter(networkStream);
            try
            {
                string outputString;
                // read the data from the host and display it
                {
                    outputString = streamReader.ReadLine();
                    Console.WriteLine("From Server" + outputString);
                    streamWriter.WriteLine("To Server: Client Message");
                    Console.WriteLine("Client Message");
                    streamWriter.Flush();
                }
            }
            catch
            {
                Console.WriteLine("Exception reading from Server");
            }
            // tidy up
            networkStream.Close();
            Console.ReadKey();
        }
    }
}