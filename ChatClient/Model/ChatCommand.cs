using System;
using System.Text.Json;

namespace ChatClient.Model
{

    public class ChatCommand : ChatBase
    {
        public Command Command { get; private set; }
        public string Data { get; set; }
        public User Identity { get; set; }

        public ChatCommand()
        {

        }

        public ChatCommand(Command command)
        {
            Command = command;
        }

        public static ChatCommand Parse(string command)
        {
            try
            {
                return JsonSerializer.Deserialize<ChatCommand>(ClearFromEOF(command));
            }
            catch (Exception)
            {
                throw new ArgumentException("Unable to parse command: " + command);
            }
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this) + EOF;
        }
    }

}
