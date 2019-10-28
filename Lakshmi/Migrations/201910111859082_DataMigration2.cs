namespace Lakshmi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Photos", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Photos", new[] { "ApplicationUser_Id" });
            DropTable("dbo.Photos");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Image = c.Binary(),
                        ApplicationUserId = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Photos", "ApplicationUser_Id");
            AddForeignKey("dbo.Photos", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
