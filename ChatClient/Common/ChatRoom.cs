using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatClient.Model;

namespace ChatClient
{
    public class ChatRoom
    {
        Socket senderClient;
        public ChatRoom()
        {
        }

        public void Connect()
        {


            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.  
                senderClient = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    senderClient.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        senderClient.RemoteEndPoint.ToString());

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

        public bool EnterInRoom(string NickName)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Encode the data string into a byte array.  
            byte[] msg = Encoding.ASCII.GetBytes(ChatCommandFactory.Login(NickName).ToString());

            // Send the data through the socket.  
            int bytesSent = senderClient.Send(msg);

            // Receive the response from the remote device.  
            int bytesRec = senderClient.Receive(bytes);
            string responseMessage = Encoding.ASCII.GetString(bytes, 0, bytesRec);

            var response = ChatResponse.Parse(responseMessage);
            return !response.IsError;
            
        }

        ~ChatRoom() 
        {
            // Release the socket.  
            senderClient.Shutdown(SocketShutdown.Both);
            senderClient.Close();

        }
    }
}
