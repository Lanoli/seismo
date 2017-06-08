using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Seismo
{
    class Program
    {
        //http://www.seiscomp3.org/wiki/doc/applications/seedlink
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient();

            client.ConnectAsync("192.168.1.28",18000).Wait();

            NetworkStream stream = client.GetStream();
           SendCommand(stream,"HELLO");
           var hello = ReadMore(stream);

           SendCommand(stream,"INFO ALL");
           var info = ReadMore(stream);

           SendCommand(stream,"INFO STREAMS");
           var streaminfo = ReadMore(stream);


            SendCommand(stream,"STATION  R5807 AM");
            var station = ReadMore(stream);
            // Read the first batch of the TcpServer response bytes.
            
            // SendCommand(stream,"SELECT 00SHZ.D.");
            // var data1 = ReadMore(stream);

            SendCommand(stream,"END");

        //    var info1 = ReadMore(stream);
        //    var info2 = ReadMore(stream);
        //    var info3 = ReadMore(stream);
            SendCommand(stream,"BYE");
        }

        private static void SendCommand(NetworkStream stream,string command)
        {
            var message = command + Environment.NewLine;

            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);  

            stream.Write(data,0,data.Length);
        }

        private static string ReadMore(NetworkStream stream)
        {
            var data = new Byte[1086];

            Int32 bytes = stream.Read(data, 0, data.Length);

            // String to store the response ASCII representation.
            return System.Text.Encoding.ASCII.GetString(data,0,data.Length);
        }
    }
}
