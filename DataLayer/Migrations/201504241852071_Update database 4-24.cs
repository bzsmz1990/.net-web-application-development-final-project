namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updatedatabase424 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "UploadTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Albums", "UploadTime");
        }
    }
}
