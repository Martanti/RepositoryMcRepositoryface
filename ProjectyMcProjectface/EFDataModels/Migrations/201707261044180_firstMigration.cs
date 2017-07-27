namespace EFDataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConnectionStrings",
                c => new
                    {
                        ConnectionId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        String = c.String(),
                    })
                .PrimaryKey(t => t.ConnectionId)
                .ForeignKey("dbo.RegisteredUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.RegisteredUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 20, unicode: false),
                        PassWord = c.String(nullable: false, maxLength: 64, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConnectionStrings", "UserId", "dbo.RegisteredUsers");
            DropIndex("dbo.ConnectionStrings", new[] { "UserId" });
            DropTable("dbo.RegisteredUsers");
            DropTable("dbo.ConnectionStrings");
        }
    }
}
