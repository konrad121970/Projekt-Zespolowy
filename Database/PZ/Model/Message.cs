using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ.Model
{
    class Message
    {
        public int MessageID { get; set; }

        public string Content { get; set; }
        public DateTime Time { get; set; }
     
        public User UserID { get; set; }

        public Conversation ConversationID { get; set; }
    }
}
