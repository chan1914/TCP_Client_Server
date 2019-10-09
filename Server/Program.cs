using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace TCP_Server
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            Thread thread = new Thread(ServerExecute);
            //Server server = new Server();
            //server.ServerExecute();
            thread.Start();

            System.Threading.Thread.Sleep(1000);
            Thread thread1 = new Thread(ClientExecute);
            thread1.Start();
            //Client client = new Client();
            //client.ClientExecute();


        }
        
        
        
        public static void ServerExecute()
        {
            bool running = true;
            
            IPHostEntry IPHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress IpAdd = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(IpAdd, 11111);

            Socket listener = new Socket(IpAdd.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

            try
            {
                

                listener.Bind(localEndPoint);
                listener.Listen(10);

                
                while (running)
                {

                    Console.WriteLine("Waiting connection ... ");
                    Socket clientSocket = listener.Accept();

                    byte[] bytes = new byte[1024];
                    string data = null;

                    while (running)
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

        public static void ClientExecute()
        {

            try
            {

                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = IPAddress.Parse("127.0.0.1"); //ipHost.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);


                Socket sender = new Socket(ipAddr.AddressFamily,
                           SocketType.Stream, ProtocolType.Tcp);

                try
                {

                    sender.Connect(localEndPoint);

                    Console.WriteLine("Socket connected to -> {0} ",
                                  sender.RemoteEndPoint.ToString());



                    byte[] messageSent = Encoding.ASCII.GetBytes("Test Client<EOF>");
                    int byteSent = sender.Send(messageSent);

                    byte[] messageReceived = new byte[1024];

                    int byteRecv = sender.Receive(messageReceived);
                    Console.WriteLine("Message from Server -> {0}",
                          Encoding.ASCII.GetString(messageReceived,
                                                     0, byteRecv));

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }


                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}

