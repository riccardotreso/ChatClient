using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

namespace ChatClient.Model
{
    public class ChatResponse : ChatBase
    {
        public static List<ChatResponse> Parse(string response)
        {
            try
            {
                var rMessages = response.Split(ChatBase.EOF);

                return rMessages
                    .Where(x => !string.IsNullOrEmpty(x))
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

}
