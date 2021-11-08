namespace PZ.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIDs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "ConversationID_ConversationID", c => c.Int());
            AddColumn("dbo.Messages", "UserID_UserID", c => c.Int());
            CreateIndex("dbo.Messages", "ConversationID_ConversationID");
            CreateIndex("dbo.Messages", "UserID_UserID");
            AddForeignKey("dbo.Messages", "ConversationID_ConversationID", "dbo.Conversations", "ConversationID");
            AddForeignKey("dbo.Messages", "UserID_UserID", "dbo.Users", "UserID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "UserID_UserID", "dbo.Users");
            DropForeignKey("dbo.Messages", "ConversationID_ConversationID", "dbo.Conversations");
            DropIndex("dbo.Messages", new[] { "UserID_UserID" });
            DropIndex("dbo.Messages", new[] { "ConversationID_ConversationID" });
            DropColumn("dbo.Messages", "UserID_UserID");
            DropColumn("dbo.Messages", "ConversationID_ConversationID");
        }
    }
}
