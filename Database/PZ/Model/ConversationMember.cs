using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ.Model
{
    class ConversationMember
    {
        public int ConversationMemberID { get; set; }

        public Conversation ConversationID { get; set; }
        public User UserID { get; set; }

    }
}
