using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChatClient.Model;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChatClient
{
    class Program
    {

        static void Main(string[] args)
        {
            ChatRoom chatRoom = new ChatRoom();
            /*var host = new WebHostBuilder()
            .UseKestrel()
            .UseUrls("http://*:9999")
            .UseStartup<Startup>()
            .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                chatRoom = services.GetRequiredService<ChatRoom>();

            }
            Task.Run(() =>
            {
                host.Run();
            });
            */

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
            string message = string.Empty;
            while (true)
            {
                Console.WriteLine("Type our message; Q for exit");
                message = Console.ReadLine();
                if ("Q".Equals(message, StringComparison.CurrentCultureIgnoreCase))
                    break;
                chatRoom.SendMessage(nickname, message);
            }


        }

        private static void ChatRoom_OnMessageArrived(object sender, string e)
        {
            Console.WriteLine(e);
        }



    }

    public class SayHi : ControllerBase
    {
        [Route("sayhi/{name}")]
        public IActionResult Get(string name)
        {
            return Ok($"Hello {name}");
        }
    }
}
