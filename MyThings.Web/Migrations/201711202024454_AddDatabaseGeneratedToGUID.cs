namespace MyThings.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDatabaseGeneratedToGUID : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Thing");
            AlterColumn("dbo.Thing", "ID", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Thing", "ID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Thing");
            AlterColumn("dbo.Thing", "ID", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Thing", "ID");
        }
    }
}
