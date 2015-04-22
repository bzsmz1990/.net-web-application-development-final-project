namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Makealbumnullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pictures", "AlbumId", "dbo.Albums");
            DropIndex("dbo.Pictures", new[] { "AlbumId" });
            AlterColumn("dbo.Pictures", "AlbumId", c => c.Int());
            CreateIndex("dbo.Pictures", "AlbumId");
            AddForeignKey("dbo.Pictures", "AlbumId", "dbo.Albums", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pictures", "AlbumId", "dbo.Albums");
            DropIndex("dbo.Pictures", new[] { "AlbumId" });
            AlterColumn("dbo.Pictures", "AlbumId", c => c.Int(nullable: false));
            CreateIndex("dbo.Pictures", "AlbumId");
            AddForeignKey("dbo.Pictures", "AlbumId", "dbo.Albums", "Id", cascadeDelete: true);
        }
    }
}
