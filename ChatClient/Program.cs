using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChatClient.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            BuildWebServer(chatRoom, response.Data);

            chatRoom.OnMessageArrived += ChatRoom_OnMessageArrived;
            string message = string.Empty;
            while (true)
            {
                Console.WriteLine("Type our message; Q for exit");
                message = Console.ReadLine();
                if ("Q".Equals(message, StringComparison.CurrentCultureIgnoreCase))
                    break;
                chatRoom.SendMessage(message);
            }


        }

        private static void BuildWebServer(ChatRoom chatRoom, string UserId)
        {
            var port = 5003 + int.Parse(UserId);
            Console.WriteLine($"Rest API listen at http://*:{port}");

            var host = new WebHostBuilder()
                        .UseKestrel()
                        .UseSetting(WebHostDefaults.SuppressStatusMessagesKey, "True")
                        .UseUrls($"http://*:{port}")
                        .ConfigureLogging(config =>
                        {
                            config.ClearProviders();
                        })
                        .ConfigureServices(services =>
                        {
                            services.AddControllers();
                            services
                                .AddSingleton<ChatRoom>(chatRoom);
                        })
                        .Configure(app =>
                        {

                            app.UseRouting();
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapControllers();
                                endpoints.MapGet("/", async context =>
                                {
                                    System.Reflection.AssemblyName current = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                                    string appVersion = $"{current.Name} v.{current.Version}";
                                    await context.Response.WriteAsync(appVersion);
                                });
                            });
                        })
                        .Build();

            host.RunAsync();
            
        }

        private static void ChatRoom_OnMessageArrived(object sender, string e)
        {
            Console.WriteLine(e);
        }
    }
}
