using System;
using Microsoft.AspNetCore.Mvc;

namespace ChatClient.Controllers
{
    public class Message {
        public string NickName { get; set; }
        public string Text { get; set; }
    }

    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        ChatRoom room;
        public MessageController(ChatRoom chatRoom)
        {
            room = chatRoom;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Message message)
        {
            room.SendMessage(message.NickName, message.Text);
            return Accepted(message);
        }
    }
}
