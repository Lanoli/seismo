using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Seismo
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient();

            client.ConnectAsync("192.168.1.28",18000).Wait();


            NetworkStream stream = client.GetStream();

           var data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);

        }
    }
}
