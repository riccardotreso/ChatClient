using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

namespace ChatClient.Model
{
    public abstract class ChatBase
    {
        protected static string EOF = "<EOF>";

        protected static string ClearFromEOF(string message)
            => message?.Replace(EOF, string.Empty);
    }
    public enum Command
    {
        LOGIN,
        LOGOUT,
        TEXT,
        COUNT,
        ALL_MESSAGE
    }


    public static class ChatCommandFactory {
        public static ChatCommand Login(string NickName)
            => new ChatCommand(Command.LOGIN) { Identity = new User(NickName) };

        public static ChatCommand Text(User Identity, string Text)
            => new ChatCommand(Command.TEXT) { Identity = Identity, Data = Text };

        public static ChatCommand GetCount()
            => new ChatCommand(Command.COUNT);

        public static ChatCommand GetAllMessage()
            => new ChatCommand(Command.ALL_MESSAGE);

        public static ChatCommand Louout()
            => new ChatCommand(Command.LOGOUT);
    }

    public class ChatResponse : ChatBase {
        public static List<ChatResponse> Parse(string response)
        {
            try
            {
                var rMessages = response.Split(ChatBase.EOF);

                return rMessages
                    .Where(x=> !string.IsNullOrEmpty(x))
                    .Select(x => JsonSerializer.Deserialize<ChatResponse>(x))
                    .ToList();

            }
            catch (Exception)
            {
                throw new ArgumentException("Unable to parse response: " + response);
            }
        }


        public string Code { get; set; }
        public bool IsError { get; set; }
        public string Data { get; set; }
    }

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

        public override string ToString()
        {
            return JsonSerializer.Serialize(this) + EOF;
        }
    }

}
