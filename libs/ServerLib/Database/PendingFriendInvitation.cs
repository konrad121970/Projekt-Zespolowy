//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Network.Server.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class PendingFriendInvitation
    {
        public int ID { get; set; }
        public int Sender_ID { get; set; }
        public int Receiver_ID { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Account Account1 { get; set; }
    }
}