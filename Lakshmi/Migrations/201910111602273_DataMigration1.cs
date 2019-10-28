namespace Lakshmi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "ApplicationUserId", c => c.Int());
            AddColumn("dbo.Photos", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Photos", "ApplicationUser_Id");
            AddForeignKey("dbo.Photos", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Photos", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Photos", "UserId", c => c.Int());
            DropForeignKey("dbo.Photos", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Photos", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Photos", "ApplicationUser_Id");
            DropColumn("dbo.Photos", "ApplicationUserId");
        }
    }
}
