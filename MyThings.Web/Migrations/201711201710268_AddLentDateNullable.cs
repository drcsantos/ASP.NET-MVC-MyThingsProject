namespace MyThings.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLentDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Thing", "LentDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Thing", "LentDate", c => c.DateTime(nullable: false));
        }
    }
}
