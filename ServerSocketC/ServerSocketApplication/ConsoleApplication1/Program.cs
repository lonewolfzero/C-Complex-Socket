using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        private static TcpListener tcpListener;
        private static List<TcpClient> tcpClientsList = new List<TcpClient>();

        static void Main(string[] args)
        {
            IPAddress serverAddress = IPAddress.Parse("172.18.95.71");
            int serverPort = 8040;

            //tcpListener = new TcpListener(IPAddress.Any, 8040);
            tcpListener = new TcpListener(serverAddress, 8040);
            tcpListener.Start();

            Console.WriteLine("Server started");

            while (true)
            {
                try
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    tcpClientsList.Add(tcpClient);

                    Thread thread = new Thread(ClientListener);
                    thread.Start(tcpClient);
                }
                catch(Exception e)
                {
                    
                }
            }
        }

        public static void ClientListener(object obj)
        {
            TcpClient tcpClient = (TcpClient)obj;
            StreamReader reader = new StreamReader(tcpClient.GetStream());

            Console.WriteLine("Client connected");

            while (true)
            {
                try
                {
                    if(tcpClient.Connected)
                    {
                        System.Threading.Thread.Sleep(8000);
                        //string message = reader.ReadLine();
                        //string message = reader.ReadLine();
                        string message = "hello Client This Message From Server";
                        
                   
                        BroadCast(message, tcpClient);
                        Console.WriteLine("Broadcast message Sent:"+message);
                    }
                }
                catch (Exception e)
                {

                }

            }
        }

        public static void BroadCast(string msg, TcpClient excludeClient)
        {
            foreach (TcpClient client in tcpClientsList)
            {
                if (client != excludeClient)
                {
                    StreamWriter sWriter = new StreamWriter(client.GetStream());
                    sWriter.WriteLine(msg);
                    sWriter.Flush();
                }
            }
        }

    }
}