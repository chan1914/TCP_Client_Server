using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCP_Server
{
    public class Server
    {
       
        public bool Running { get; set; }
        public void ServerExecute()
        {

            IPHostEntry IPHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress IpAdd = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(IpAdd, 11111);

            Socket listener = new Socket(IpAdd.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

            try
            {


                listener.Bind(localEndPoint);
                listener.Listen(10);
                
                Running = true;
                while(Running)
                {

                    Console.WriteLine("Waiting connection ... ");
                    Socket clientSocket = listener.Accept();

                    byte[] bytes = new byte[1024];
                    string data = null;

                    while (Running)
                    {

                        int numByte = clientSocket.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes,
                                                   0, numByte);

                        if (data.IndexOf("<EOF>") > -1)
                            break;
                    }

                    Console.WriteLine("Text received -> {0} ", data);
                    byte[] message = Encoding.ASCII.GetBytes("Hej med dig");

                    clientSocket.Send(message);

                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

}
