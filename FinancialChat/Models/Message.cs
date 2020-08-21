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
    }
}
