namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePictureTypetoenumfromstring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pictures", "PictureType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pictures", "PictureType", c => c.String(nullable: false));
        }
    }
}
