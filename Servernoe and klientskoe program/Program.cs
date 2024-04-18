using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(866);
            Console.WriteLine("Однопоточный сервер запущен");

            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 8888);

            Socket sock = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sock.Bind(ipEndPoint);
                sock.Listen(10);

                while (true)
                {
                    Console.WriteLine("Слушаем, порт {0}", ipEndPoint);

                    Socket s = sock.Accept();
                    string data = null;
                    byte[] bytes = new byte[1024];
                    int bytesRec = s.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    Console.WriteLine("Полученный текст: " + data);

                    byte[] msg = Encoding.ASCII.GetBytes("Сервер получил ваше сообщение.");
                    s.Send(msg);

                    s.Shutdown(SocketShutdown.Both);
                    s.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
