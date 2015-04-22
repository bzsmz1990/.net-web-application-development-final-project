namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColumnNamechanges : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Albums", name: "Cost", newName: "AlbumPrice");
            AddColumn("dbo.Albums", "AlbumName", c => c.String(maxLength: 50));
            AddColumn("dbo.Pictures", "NumberOfLikes", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pictures", "NumberOfLikes");
            DropColumn("dbo.Albums", "AlbumName");
            RenameColumn(table: "dbo.Albums", name: "AlbumPrice", newName: "Cost");
        }
    }
}
