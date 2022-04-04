namespace ChatClient.Model
{
    public static class ChatCommandFactory
    {
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

}
