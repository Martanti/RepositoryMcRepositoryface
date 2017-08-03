namespace EFDataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedConnectionStringsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConnectionStrings", "InternalDatabaseName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConnectionStrings", "InternalDatabaseName");
        }
    }
}
