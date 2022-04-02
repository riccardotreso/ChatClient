using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ChatClient.Model;

namespace ChatClient
{
    class Program
    {

        static void Main(string[] args)
        {
            ChatRoom chatRoom = new ChatRoom();
            chatRoom.Connect();
            Console.WriteLine("Type your nickname");
            string nickname = Console.ReadLine();
            ChatResponse response;
            while ((response = chatRoom.EnterInRoom(nickname)).IsError)
            {
                Console.WriteLine("Nickname already used. Choose another one, please...");
                Console.WriteLine("Type your nickname");
                nickname = Console.ReadLine();
            }
            Console.WriteLine($"Welcome {nickname}, your id is {response.Data}");

            chatRoom.OnMessageArrived += ChatRoom_OnMessageArrived;

            Console.ReadLine();

        }

        private static void ChatRoom_OnMessageArrived(object sender, string e)
        {
            Console.WriteLine(e);
        }
    }
}
