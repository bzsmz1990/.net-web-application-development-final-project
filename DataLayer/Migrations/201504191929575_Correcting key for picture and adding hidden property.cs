namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Correctingkeyforpictureandaddinghiddenproperty : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CartPictures", new[] { "Picture_Id", "Picture_OwnerId" }, "dbo.Pictures");
            DropForeignKey("dbo.CartPicture1", new[] { "Picture_Id", "Picture_OwnerId" }, "dbo.Pictures");
            DropForeignKey("dbo.UserInfoes", new[] { "Picture_Id", "Picture_OwnerId" }, "dbo.Pictures");
            DropForeignKey("dbo.TransactionPictures", new[] { "Picture_Id", "Picture_OwnerId" }, "dbo.Pictures");
            DropForeignKey("dbo.PictureTags", new[] { "Picture_Id", "Picture_OwnerId" }, "dbo.Pictures");
            DropIndex("dbo.UserInfoes", new[] { "Picture_Id", "Picture_OwnerId" });
            DropIndex("dbo.TransactionPictures", new[] { "Picture_Id", "Picture_OwnerId" });
            DropIndex("dbo.PictureTags", new[] { "Picture_Id", "Picture_OwnerId" });
            DropIndex("dbo.CartPictures", new[] { "Picture_Id", "Picture_OwnerId" });
            DropIndex("dbo.CartPicture1", new[] { "Picture_Id", "Picture_OwnerId" });
            DropPrimaryKey("dbo.Pictures");
            DropPrimaryKey("dbo.CartPictures");
            DropPrimaryKey("dbo.CartPicture1");
            DropPrimaryKey("dbo.TransactionPictures");
            DropPrimaryKey("dbo.PictureTags");
            AddColumn("dbo.Albums", "HasBeenReported", c => c.Boolean(nullable: false));
            AddColumn("dbo.Pictures", "Hidden", c => c.Boolean(nullable: false));
            AddPrimaryKey("dbo.Pictures", "Id");
            AddPrimaryKey("dbo.CartPictures", new[] { "Cart_UserId", "Picture_Id" });
            AddPrimaryKey("dbo.CartPicture1", new[] { "Cart_UserId", "Picture_Id" });
            AddPrimaryKey("dbo.TransactionPictures", new[] { "Transaction_Id", "Picture_Id" });
            AddPrimaryKey("dbo.PictureTags", new[] { "Picture_Id", "Tag_Id" });
            CreateIndex("dbo.UserInfoes", "Picture_Id");
            CreateIndex("dbo.TransactionPictures", "Picture_Id");
            CreateIndex("dbo.PictureTags", "Picture_Id");
            CreateIndex("dbo.CartPictures", "Picture_Id");
            CreateIndex("dbo.CartPicture1", "Picture_Id");
            AddForeignKey("dbo.CartPictures", "Picture_Id", "dbo.Pictures", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CartPicture1", "Picture_Id", "dbo.Pictures", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserInfoes", "Picture_Id", "dbo.Pictures", "Id");
            AddForeignKey("dbo.TransactionPictures", "Picture_Id", "dbo.Pictures", "Id", cascadeDelete: false);
            AddForeignKey("dbo.PictureTags", "Picture_Id", "dbo.Pictures", "Id", cascadeDelete: true);
            DropColumn("dbo.UserInfoes", "Picture_OwnerId");
            DropColumn("dbo.CartPictures", "Picture_OwnerId");
            DropColumn("dbo.CartPicture1", "Picture_OwnerId");
            DropColumn("dbo.TransactionPictures", "Picture_OwnerId");
            DropColumn("dbo.PictureTags", "Picture_OwnerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PictureTags", "Picture_OwnerId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.TransactionPictures", "Picture_OwnerId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.CartPicture1", "Picture_OwnerId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.CartPictures", "Picture_OwnerId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.UserInfoes", "Picture_OwnerId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.PictureTags", "Picture_Id", "dbo.Pictures");
            DropForeignKey("dbo.TransactionPictures", "Picture_Id", "dbo.Pictures");
            DropForeignKey("dbo.UserInfoes", "Picture_Id", "dbo.Pictures");
            DropForeignKey("dbo.CartPicture1", "Picture_Id", "dbo.Pictures");
            DropForeignKey("dbo.CartPictures", "Picture_Id", "dbo.Pictures");
            DropIndex("dbo.CartPicture1", new[] { "Picture_Id" });
            DropIndex("dbo.CartPictures", new[] { "Picture_Id" });
            DropIndex("dbo.PictureTags", new[] { "Picture_Id" });
            DropIndex("dbo.TransactionPictures", new[] { "Picture_Id" });
            DropIndex("dbo.UserInfoes", new[] { "Picture_Id" });
            DropPrimaryKey("dbo.PictureTags");
            DropPrimaryKey("dbo.TransactionPictures");
            DropPrimaryKey("dbo.CartPicture1");
            DropPrimaryKey("dbo.CartPictures");
            DropPrimaryKey("dbo.Pictures");
            DropColumn("dbo.Pictures", "Hidden");
            DropColumn("dbo.Albums", "HasBeenReported");
            AddPrimaryKey("dbo.PictureTags", new[] { "Picture_Id", "Picture_OwnerId", "Tag_Id" });
            AddPrimaryKey("dbo.TransactionPictures", new[] { "Transaction_Id", "Picture_Id", "Picture_OwnerId" });
            AddPrimaryKey("dbo.CartPicture1", new[] { "Cart_UserId", "Picture_Id", "Picture_OwnerId" });
            AddPrimaryKey("dbo.CartPictures", new[] { "Cart_UserId", "Picture_Id", "Picture_OwnerId" });
            AddPrimaryKey("dbo.Pictures", new[] { "Id", "OwnerId" });
            CreateIndex("dbo.CartPicture1", new[] { "Picture_Id", "Picture_OwnerId" });
            CreateIndex("dbo.CartPictures", new[] { "Picture_Id", "Picture_OwnerId" });
            CreateIndex("dbo.PictureTags", new[] { "Picture_Id", "Picture_OwnerId" });
            CreateIndex("dbo.TransactionPictures", new[] { "Picture_Id", "Picture_OwnerId" });
            CreateIndex("dbo.UserInfoes", new[] { "Picture_Id", "Picture_OwnerId" });
            AddForeignKey("dbo.PictureTags", new[] { "Picture_Id", "Picture_OwnerId" }, "dbo.Pictures", new[] { "Id", "OwnerId" }, cascadeDelete: true);
            AddForeignKey("dbo.TransactionPictures", new[] { "Picture_Id", "Picture_OwnerId" }, "dbo.Pictures", new[] { "Id", "OwnerId" }, cascadeDelete: true);
            AddForeignKey("dbo.UserInfoes", new[] { "Picture_Id", "Picture_OwnerId" }, "dbo.Pictures", new[] { "Id", "OwnerId" });
            AddForeignKey("dbo.CartPicture1", new[] { "Picture_Id", "Picture_OwnerId" }, "dbo.Pictures", new[] { "Id", "OwnerId" }, cascadeDelete: true);
            AddForeignKey("dbo.CartPictures", new[] { "Picture_Id", "Picture_OwnerId" }, "dbo.Pictures", new[] { "Id", "OwnerId" }, cascadeDelete: true);
        }
    }
}
