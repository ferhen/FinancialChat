using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialChat.Models
{
    public class Message
    {
        public int Id { get; private set; }
        public string Content { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }

        public int ChatroomId { get; private set; }
        public virtual Chatroom Chatroom { get; private set; }

        public string UserId { get; private set; }
        public virtual ApplicationUser User { get; private set; }

        private Message() { }
        public Message(string content, int chatroomId, string userId)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Message content can't be empty");
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException("UserId is necessary");
            Content = content;
            CreatedOn = DateTimeOffset.Now;
            ChatroomId = chatroomId;
            UserId = userId;
        }

        public bool IsCommand()
        {
            return Content.StartsWith("/stock=");
        }

        public string GetCommandParameter()
        {
            if (!Content.Contains('='))
                return null;
            return Content.Split('=')[1];
        }
    }
}
