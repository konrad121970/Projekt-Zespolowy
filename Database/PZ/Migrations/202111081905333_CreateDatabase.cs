namespace PZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConversationMembers",
                c => new
                    {
                        ConversationMemberID = c.Int(nullable: false, identity: true),
                        ConversationID_ConversationID = c.Int(),
                        UserID_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.ConversationMemberID)
                .ForeignKey("dbo.Conversations", t => t.ConversationID_ConversationID)
                .ForeignKey("dbo.Users", t => t.UserID_UserID)
                .Index(t => t.ConversationID_ConversationID)
                .Index(t => t.UserID_UserID);
            
            CreateTable(
                "dbo.Conversations",
                c => new
                    {
                        ConversationID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ConversationID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MessageID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConversationMembers", "UserID_UserID", "dbo.Users");
            DropForeignKey("dbo.ConversationMembers", "ConversationID_ConversationID", "dbo.Conversations");
            DropIndex("dbo.ConversationMembers", new[] { "UserID_UserID" });
            DropIndex("dbo.ConversationMembers", new[] { "ConversationID_ConversationID" });
            DropTable("dbo.Messages");
            DropTable("dbo.Users");
            DropTable("dbo.Conversations");
            DropTable("dbo.ConversationMembers");
        }
    }
}
