namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Useridentitygenerated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserInfoUserInfoes", "UserInfo_UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.UserInfoUserInfoes", "UserInfo_UserId1", "dbo.UserInfoes");
            DropForeignKey("dbo.Pictures", "UserInfo_UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.Pictures", "UserInfo_UserId1", "dbo.UserInfoes");
            DropForeignKey("dbo.Transactions", "BuyerId", "dbo.UserInfoes");
            DropForeignKey("dbo.Transactions", "SellerId", "dbo.UserInfoes");
            DropForeignKey("dbo.Transactions", "UserInfo_UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.Transactions", "UserInfo_UserId1", "dbo.UserInfoes");
            DropForeignKey("dbo.Pictures", "OwnerId", "dbo.UserInfoes");
            DropForeignKey("dbo.Carts", "UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.Albums", "UserId", "dbo.UserInfoes");
            DropIndex("dbo.UserInfoes", new[] { "UserId" });
            DropPrimaryKey("dbo.UserInfoes");
            AlterColumn("dbo.UserInfoes", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.UserInfoes", "UserId");
            CreateIndex("dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.UserInfoUserInfoes", "UserInfo_UserId", "dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.UserInfoUserInfoes", "UserInfo_UserId1", "dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.Pictures", "UserInfo_UserId", "dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.Pictures", "UserInfo_UserId1", "dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.Transactions", "BuyerId", "dbo.UserInfoes", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Transactions", "SellerId", "dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.Transactions", "UserInfo_UserId", "dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.Transactions", "UserInfo_UserId1", "dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.Pictures", "OwnerId", "dbo.UserInfoes", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Carts", "UserId", "dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.Albums", "UserId", "dbo.UserInfoes", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Albums", "UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.Carts", "UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.Pictures", "OwnerId", "dbo.UserInfoes");
            DropForeignKey("dbo.Transactions", "UserInfo_UserId1", "dbo.UserInfoes");
            DropForeignKey("dbo.Transactions", "UserInfo_UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.Transactions", "SellerId", "dbo.UserInfoes");
            DropForeignKey("dbo.Transactions", "BuyerId", "dbo.UserInfoes");
            DropForeignKey("dbo.Pictures", "UserInfo_UserId1", "dbo.UserInfoes");
            DropForeignKey("dbo.Pictures", "UserInfo_UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.UserInfoUserInfoes", "UserInfo_UserId1", "dbo.UserInfoes");
            DropForeignKey("dbo.UserInfoUserInfoes", "UserInfo_UserId", "dbo.UserInfoes");
            DropIndex("dbo.UserInfoes", new[] { "UserId" });
            DropPrimaryKey("dbo.UserInfoes");
            AlterColumn("dbo.UserInfoes", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.UserInfoes", "UserId");
            CreateIndex("dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.Albums", "UserId", "dbo.UserInfoes", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Carts", "UserId", "dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.Pictures", "OwnerId", "dbo.UserInfoes", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Transactions", "UserInfo_UserId1", "dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.Transactions", "UserInfo_UserId", "dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.Transactions", "SellerId", "dbo.UserInfoes", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Transactions", "BuyerId", "dbo.UserInfoes", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Pictures", "UserInfo_UserId1", "dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.Pictures", "UserInfo_UserId", "dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.UserInfoUserInfoes", "UserInfo_UserId1", "dbo.UserInfoes", "UserId");
            AddForeignKey("dbo.UserInfoUserInfoes", "UserInfo_UserId", "dbo.UserInfoes", "UserId");
        }
    }
}
