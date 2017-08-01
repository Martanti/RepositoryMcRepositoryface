namespace EFDataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alteredConnectionStringsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConnectionStrings", "InternalConnString", c => c.String());
            DropColumn("dbo.ConnectionStrings", "DataSource");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ConnectionStrings", "DataSource", c => c.String());
            DropColumn("dbo.ConnectionStrings", "InternalConnString");
        }
    }
}
