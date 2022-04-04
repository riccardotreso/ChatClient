namespace ChatClient.Model
{
    public abstract class ChatBase
    {
        protected static string EOF = "<EOF>";

        protected static string ClearFromEOF(string message)
            => message?.Replace(EOF, string.Empty);
    }

}
