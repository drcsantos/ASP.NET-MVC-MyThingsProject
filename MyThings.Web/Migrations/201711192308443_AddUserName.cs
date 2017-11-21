namespace MyThings.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserName : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Thing", "PersonID", "dbo.Person");
            DropIndex("dbo.Thing", new[] { "PersonID" });
            AddColumn("dbo.Category", "UserName", c => c.String(maxLength: 255));
            AddColumn("dbo.Person", "UserName", c => c.String(maxLength: 255));
            AlterColumn("dbo.Thing", "UserName", c => c.String(maxLength: 255));
            AlterColumn("dbo.Thing", "PersonID", c => c.Int());
            CreateIndex("dbo.Thing", "PersonID");
            AddForeignKey("dbo.Thing", "PersonID", "dbo.Person", "PersonID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Thing", "PersonID", "dbo.Person");
            DropIndex("dbo.Thing", new[] { "PersonID" });
            AlterColumn("dbo.Thing", "PersonID", c => c.Int(nullable: false));
            AlterColumn("dbo.Thing", "UserName", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.Person", "UserName");
            DropColumn("dbo.Category", "UserName");
            CreateIndex("dbo.Thing", "PersonID");
            AddForeignKey("dbo.Thing", "PersonID", "dbo.Person", "PersonID", cascadeDelete: true);
        }
    }
}
