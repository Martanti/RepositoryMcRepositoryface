namespace EFDataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RegisteredUsers", "eMail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RegisteredUsers", "eMail");
        }
    }
}
