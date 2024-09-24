using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace ClientForm
{
    class Client
    {
        private string m_ipAddress;
        private int m_portNbr;
        private TcpClient m_serverSocket;

        public Client(string ipAddress, int portNbr)
        {
            m_ipAddress = ipAddress;
            m_portNbr = portNbr;

            try
            {
                m_serverSocket = new TcpClient(ipAddress, portNbr);
            }
            catch
            {
                Console.WriteLine(
                "Failed to connect to server at {0}:{1}", ipAddress, portNbr);
                return;
            }
        }
        public string ReadMessage()
        {
            string outputString = "ERROR";

            NetworkStream networkStream = m_serverSocket.GetStream();
            System.IO.StreamReader streamReader =
            new System.IO.StreamReader(networkStream);
            System.IO.StreamWriter streamWriter =
            new System.IO.StreamWriter(networkStream);
            try
            {
                // read the data from the host and display it
                {
                    outputString = streamReader.ReadLine();
                    //Console.WriteLine(outputString);
                    streamWriter.WriteLine("Hello, I'm the client!");
                    //Console.WriteLine("Client Message");
                    streamWriter.Flush();
                }
            }
            catch
            {
                Console.WriteLine("Exception reading from Server");
            }
            // tidy up
            networkStream.Close();

            return outputString;
        }

    }
}
