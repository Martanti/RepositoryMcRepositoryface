namespace EFDataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValidationRemoval : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RegisteredUsers", "UserName", c => c.String(unicode: false));
            AlterColumn("dbo.RegisteredUsers", "PassWord", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RegisteredUsers", "PassWord", c => c.String(nullable: false, maxLength: 64, unicode: false));
            AlterColumn("dbo.RegisteredUsers", "UserName", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }
    }
}
