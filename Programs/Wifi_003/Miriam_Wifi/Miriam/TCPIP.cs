using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Miriam
{
    class TCPIP
    {
        public static String ip = "10.9.23.175";
        //public static String ip = "192.168.1.7";

        public String command;
        public String par;
        public String res = "";
       

        public void sendANDreceive()
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 80;
                var host = Dns.GetHostEntry("JUHO-PC");
                TcpClient client = new TcpClient(ip, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes("$command$" + this.command + "$" + this.par);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                //Console.WriteLine("Sent: {0}", "$command$" + command + "$" + par);

                Boolean messageRdy = false;
                String message = "";

                // Receive the TcpServer.response.
                do
                {
                    // Buffer to store the response bytes.
                    data = new Byte[256];

                    // String to store the response ASCII representation.
                    String responseData = String.Empty;

                    // Read the first batch of the TcpServer response bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    //Console.WriteLine("Received: {0}", responseData);

                    message = message + responseData;

                    if (responseData.Contains("$"))
                    {
                        messageRdy = true;
                    }
                } while (messageRdy == false);

                message = message.Replace("$", "");

                // Close everything.
                stream.Close();
                client.Close();

                res = message;

                //return message;
            }
            catch (Exception exc)
            {
                res = "ERROR";
            }
            
        }

    }
}
