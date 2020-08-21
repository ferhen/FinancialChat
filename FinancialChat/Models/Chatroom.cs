using IdentityServer4.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialChat.Models
{
    public class Chatroom
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public virtual IEnumerable<Message> Messages { get; private set; }

        private Chatroom() { }
        public Chatroom(string name)
        {
            ValidateName(name);
            Name = name;
        }

        public void Update(string name)
        {
            ValidateName(name);
            Name = name;
        }

        private void ValidateName(string name)
        {
            if (name.IsNullOrEmpty())
                throw new ArgumentException("Chatroom name must be valid");
        }
    }
}
