using System;

namespace FinancialChat.ViewModels
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string UserName { get; set; }
    }
}
