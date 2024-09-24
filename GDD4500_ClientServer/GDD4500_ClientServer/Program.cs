using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace GDD4500_ClientServer
{
    /// <summary>
    /// Standalone TCP/IP sockets example using C#.NET 4.0
    /// </summary>
    /// <remarks>
    /// Compile as a C#.NET console mode application
    /// 
    /// Command line usage:
    ///   SocketsTest -client   => run the client
    ///   SocketsTest -server   => run the server
    /// </remarks>
    class SocketsTest
    {
        static TcpListener listener;

        // Sample high score table data
        static Dictionary<string, int> highScoreTable = new Dictionary<string, int>() { 
            { "john", 1001 }, 
            { "ann", 1350 }, 
            { "bob", 1200 }, 
            { "roxy", 1199 } 
        };

        static int Port = 4321;
        static IPAddress IP_ADDRESS = new IPAddress(new byte[] { 127, 0, 0, 1 });
        static string HOSTNAME = "127.0.0.1";
        static int MAX_CLIENTS = 5;

        public static void Main(string[] args)
        {
            if (args.Contains("-server"))
            {
                ServerMain();
            }
            else if (args.Contains("-client"))
            {
                ClientMain();
            }
            else
            {
                Console.WriteLine("Usage: SocketsTest -client   => run the client");
                Console.WriteLine("       SocketsTest -server   => run the server");
            }


        }

        /// <summary>
        /// Server receives player name requests from the client and responds with the score.
        /// </summary>
        private static void ServerMain()
        {
            listener = new TcpListener(IP_ADDRESS, Port);
            listener.Start();
            Console.WriteLine("Server running, listening to port " + Port + " at " + IP_ADDRESS);
            Console.WriteLine("Hit Ctrl-C to exit");
            var tasks = new List<Task>();
            for (int i = 0; i < MAX_CLIENTS; i++)
            {
                Task task = new Task(Service, TaskCreationOptions.LongRunning);
                task.Start();
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
            listener.Stop();
        }

        private static void Service()
        {
            while (true)
            {
                Socket socket = listener.AcceptSocket();

                Console.WriteLine("Connected: {0}", socket.RemoteEndPoint);
                try
                {
                    // Open the stream
                    Stream stream = new NetworkStream(socket);
                    StreamReader sr = new StreamReader(stream);
                    StreamWriter sw = new StreamWriter(stream);
                    sw.AutoFlush = true;

                    sw.WriteLine("{0} stats available", highScoreTable.Count);
                    while (true)
                    {
                        // Read name from client
                        string name = sr.ReadLine();
                        if (name == "" || name == null) break;

                        // Write score to client
                        if (highScoreTable.ContainsKey(name))
                            sw.WriteLine(highScoreTable[name]);
                        else
                            sw.WriteLine("Player '" + name + "' was not found.");

                    }
                    stream.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine("Disconnected: {0}", socket.RemoteEndPoint);
                socket.Close();
            }
        }

        /// <summary>
        /// Client requests a player name's score from the server.
        /// </summary>
        private static void ClientMain()
        {
            TcpClient client = new TcpClient(HOSTNAME, Port);
            try
            {
                // Open the stream
                Stream stream = client.GetStream();
                StreamReader sr = new StreamReader(stream);
                StreamWriter sw = new StreamWriter(stream);
                sw.AutoFlush = true;

                // Read and output the first line from the service, which
                // contains the number of players listed in the table.
                Console.WriteLine(sr.ReadLine());

                while (true)
                {
                    // Input player name
                    Console.Write("Enter player name: (leave blank to exit)");
                    string name = Console.ReadLine();

                    // Write name to server
                    sw.WriteLine(name);
                    if (name == "") break;

                    // Read score from server
                    Console.WriteLine(sr.ReadLine());
                }
                stream.Close();
            }
            finally
            {
                // Close the connection
                client.Close();
            }
        }
    }
}
