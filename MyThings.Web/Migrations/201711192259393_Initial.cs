namespace MyThings.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Icon = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Thing",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 255),
                        ImageLink = c.String(maxLength: 255),
                        UserName = c.String(nullable: false, maxLength: 255),
                        CategoryID = c.Int(nullable: false),
                        PersonID = c.Int(nullable: false),
                        LentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.CategoryID)
                .Index(t => t.PersonID);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        PersonID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        NickName = c.String(maxLength: 50),
                        ImageLink = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.PersonID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Thing", "PersonID", "dbo.Person");
            DropForeignKey("dbo.Thing", "CategoryID", "dbo.Category");
            DropIndex("dbo.Thing", new[] { "PersonID" });
            DropIndex("dbo.Thing", new[] { "CategoryID" });
            DropTable("dbo.Person");
            DropTable("dbo.Thing");
            DropTable("dbo.Category");
        }
    }
}
