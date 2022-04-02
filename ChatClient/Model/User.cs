namespace ChatClient.Model
{
    public class User
    {
        public string Id { get; set; }
        public string NickName { get; set; }

        public User()
        {
            
        }
        public User(string nickname)
        {
            NickName = nickname;
        }
    }

}
