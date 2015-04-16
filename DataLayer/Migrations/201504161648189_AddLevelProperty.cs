namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLevelProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInfoes", "Level", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInfoes", "Level");
        }
    }
}
