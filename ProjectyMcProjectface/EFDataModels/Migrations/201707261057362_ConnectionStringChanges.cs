namespace EFDataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectionStringChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConnectionStrings", "DataSource", c => c.String());
            AddColumn("dbo.ConnectionStrings", "DatabaseName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConnectionStrings", "DatabaseName");
            DropColumn("dbo.ConnectionStrings", "DataSource");
        }
    }
}
