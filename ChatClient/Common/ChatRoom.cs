using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChatClient.Model;
using System.Linq;

namespace ChatClient
{


    public class ChatRoom
    {
        private string NickName;
        Socket senderClient;
        public event EventHandler<string> OnMessageArrived;
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

                while (!senderClient.Connected)
                {
                    // Connect the socket to the remote endpoint. Catch any errors.  
                    try
                    {
                        senderClient.Connect(remoteEP);

                        Console.WriteLine("Chat connected to {0}",
                            senderClient.RemoteEndPoint.ToString());

                    }
                    catch (ArgumentNullException ane)
                    {
                        Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                    }
                    catch (SocketException se)
                    {
                        Console.WriteLine("SocketException : {0}", se.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Unexpected exception : {0}", e.ToString());
                    }

                    if (!senderClient.Connected)
                    {
                        Console.WriteLine("Wait 30 sec and try to reconnect...");
                        Task.Delay(30000).Wait();
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public ChatResponse EnterInRoom(string NickName)
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

            if (!response.First().IsError)
            {
                this.NickName = NickName;
                ListenMessages();
            }
                

            return response.First();
        }

        public void SendMessage(string Message)
        {
            byte[] msg = Encoding.ASCII.GetBytes(ChatCommandFactory.Text(new User { NickName = NickName }, Message).ToString());

            int bytesSent = senderClient.Send(msg);
        }

        private void ListenMessages()
        {
            Task.Run(() =>
            {
                // Data buffer for incoming data.  
                byte[] bytes = new byte[1024];
                // An incoming connection needs to be processed.  
                while (true)
                {
                    // Receive the response from the remote device.  
                    int bytesRec = senderClient.Receive(bytes);
                    string message = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    var responses = ChatResponse.Parse(message);
                    foreach (var response in responses)
                    {
                        if (response.Data != "ACK")
                        {
                            if (OnMessageArrived != null)
                                OnMessageArrived.Invoke(this, response.Data);
                        }
                    }
                }
            });

        }




        ~ChatRoom()
        {
            // Release the socket.  
            senderClient.Shutdown(SocketShutdown.Both);
            senderClient.Close();

        }
    }
}
