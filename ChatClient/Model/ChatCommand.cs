using System;
using System.Text.Json;

namespace ChatClient.Model
{

    public static class ChatCommandFactory {
        public static ChatCommand Login(string NickName)
            => new ChatCommand("LOGIN") { Identity = new User(NickName) };
    }

    public class ChatCommand
    {
        private string EOF = "<EOF>";
        public string Command { get; private set; }
        public string Data { get; set; }
        public User Identity { get; set; }

        public ChatCommand()
        {

        }

        public ChatCommand(string command)
        {
            Command = command;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this) + EOF;
        }
    }

}
