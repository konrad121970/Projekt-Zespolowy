using PZ.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ.DB
{
    class DrocsidDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Conversation> Conversations { get; set; }

        public DbSet<ConversationMember> ConversationMembers { get; set; }

        public DbSet<Message> Messages { get; set; }


    }
}
