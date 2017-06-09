using System;
using System.IO;
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
                client.Client.ReceiveTimeout = 20;

                client.ConnectAsync("192.168.1.28",18000).Wait();

                using(NetworkStream stream = client.GetStream())
                {
                    bool exit = false;
                    while (!exit)
                    {
                        try
                        {
                            Console.WriteLine("Enter command >>");
                            string decision = Console.ReadLine();
                    
                            exit = decision == "exit";

                            if (decision == "auto")
                            {
                                Console.WriteLine(SendCmd(stream,"HELLO"));
                                Console.WriteLine(SendCmd(stream,"INFO STREAMS"));
                                Console.WriteLine(SendCmd(stream,"STATION  R5807 AM"));
                                Console.WriteLine(SendCmd(stream,"SELECT 00SHZ.D"));
                                Console.WriteLine(SendCmd(stream, "TIME " + DateTime.Now.AddDays(-1).ToString("yyyy,MM,dd,HH,mm,ss"))); //"TIME 2017,06,08,17,55,59"));
                                Console.WriteLine(SendCmd(stream,"END"));
                            }else if (!exit)
                            {
                                Console.WriteLine(SendCmd(stream,decision));
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                }
            }
        }


        private static string SendCmd(NetworkStream stream,string command)
        {
            var data = System.Text.Encoding.ASCII.GetBytes(command + Environment.NewLine);
           
            // Set a 250 millisecond timeout for reading (instead of Infinite the default)
            stream.ReadTimeout = 1000;

            Console.Write(command +  ":::::");

            stream.Write(data, 0, data.Length);
            byte[] resp = new byte[2048];
            var memStream = new MemoryStream();
            int bytesread = stream.Read(resp, 0, resp.Length);

            while (bytesread > 0)
            {
                memStream.Write(resp, 0, bytesread);
                try
                {
                    bytesread = stream.Read(resp, 0, resp.Length);
                }
                catch
                {
                    bytesread = 0;
                }
            }
            return System.Text.Encoding.ASCII.GetString(memStream.ToArray());
        }

    }
}
