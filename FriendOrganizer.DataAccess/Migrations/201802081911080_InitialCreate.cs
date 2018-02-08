namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Friend", "FavoriteLanguageId", "dbo.ProgrammingLanguage");
            DropForeignKey("dbo.MeetingFriend", "Meeting_Id", "dbo.Meeting");
            DropForeignKey("dbo.MeetingFriend", "Friend_Id", "dbo.Friend");
            DropForeignKey("dbo.FriendPhoneNumber", "FriendId", "dbo.Friend");
            DropIndex("dbo.FriendPhoneNumber", new[] { "FriendId" });
            DropIndex("dbo.Friend", new[] { "FavoriteLanguageId" });
            DropIndex("dbo.MeetingFriend", new[] { "Meeting_Id" });
            DropIndex("dbo.MeetingFriend", new[] { "Friend_Id" });
            DropColumn("dbo.Friend", "FavoriteLanguageId");
            DropColumn("dbo.Friend", "RowVersion");
            DropTable("dbo.FriendPhoneNumber");
            DropTable("dbo.ProgrammingLanguage");
            DropTable("dbo.Meeting");
            DropTable("dbo.MeetingFriend");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MeetingFriend",
                c => new
                    {
                        Meeting_Id = c.Int(nullable: false),
                        Friend_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Meeting_Id, t.Friend_Id });
            
            CreateTable(
                "dbo.Meeting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProgrammingLanguage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FriendPhoneNumber",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false),
                        FriendId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Friend", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Friend", "FavoriteLanguageId", c => c.Int());
            CreateIndex("dbo.MeetingFriend", "Friend_Id");
            CreateIndex("dbo.MeetingFriend", "Meeting_Id");
            CreateIndex("dbo.Friend", "FavoriteLanguageId");
            CreateIndex("dbo.FriendPhoneNumber", "FriendId");
            AddForeignKey("dbo.FriendPhoneNumber", "FriendId", "dbo.Friend", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MeetingFriend", "Friend_Id", "dbo.Friend", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MeetingFriend", "Meeting_Id", "dbo.Meeting", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Friend", "FavoriteLanguageId", "dbo.ProgrammingLanguage", "Id");
        }
    }
}
