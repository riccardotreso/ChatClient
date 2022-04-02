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
            if (chatRoom.EnterInRoom(nickname))
                Console.WriteLine($"Welcome {nickname}");

            Console.ReadLine();

        }
    }
}
