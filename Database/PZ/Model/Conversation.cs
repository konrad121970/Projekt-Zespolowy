using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ.Model
{
    class Conversation
    {
        public int ConversationID { get; set; }

        public string Name { get; set; }

        public List<Message> MessageList;

        public List<User> UserList;

    }
}
