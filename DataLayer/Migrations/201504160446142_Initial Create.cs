namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfoes", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.UserInfoes", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerId = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 50),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Location = c.String(maxLength: 50),
                        Description = c.String(maxLength: 500),
                        UploadTime = c.DateTime(nullable: false),
                        HasBeenReported = c.Boolean(nullable: false),
                        OriginalImg = c.String(nullable: false),
                        CompressedImg = c.String(nullable: false),
                        AlbumId = c.Int(nullable: false),
                        UserInfo_UserId = c.String(maxLength: 128),
                        UserInfo_UserId1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Id, t.OwnerId })
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfo_UserId)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfo_UserId1)
                .ForeignKey("dbo.UserInfoes", t => t.OwnerId, cascadeDelete: true)
                .Index(t => t.OwnerId)
                .Index(t => t.AlbumId)
                .Index(t => t.UserInfo_UserId)
                .Index(t => t.UserInfo_UserId1);
            
            CreateTable(
                "dbo.UserInfoes",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 25),
                        MiddleInitial = c.String(maxLength: 1),
                        LastName = c.String(nullable: false, maxLength: 25),
                        AccountBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        isDeleted = c.Boolean(nullable: false),
                        Picture_Id = c.Int(),
                        Picture_OwnerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Pictures", t => new { t.Picture_Id, t.Picture_OwnerId })
                .Index(t => t.UserId)
                .Index(t => new { t.Picture_Id, t.Picture_OwnerId });
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BuyerId = c.String(nullable: false, maxLength: 128),
                        SellerId = c.String(nullable: false, maxLength: 128),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserInfo_UserId = c.String(maxLength: 128),
                        UserInfo_UserId1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfoes", t => t.BuyerId, cascadeDelete: true)
                .ForeignKey("dbo.UserInfoes", t => t.SellerId, cascadeDelete: true)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfo_UserId)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfo_UserId1)
                .Index(t => t.BuyerId)
                .Index(t => t.SellerId)
                .Index(t => t.UserInfo_UserId)
                .Index(t => t.UserInfo_UserId1);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.CartAlbums",
                c => new
                    {
                        Cart_UserId = c.String(nullable: false, maxLength: 128),
                        Album_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Cart_UserId, t.Album_Id })
                .ForeignKey("dbo.Carts", t => t.Cart_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Albums", t => t.Album_Id, cascadeDelete: true)
                .Index(t => t.Cart_UserId)
                .Index(t => t.Album_Id);
            
            CreateTable(
                "dbo.CartAlbum1",
                c => new
                    {
                        Cart_UserId = c.String(nullable: false, maxLength: 128),
                        Album_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Cart_UserId, t.Album_Id })
                .ForeignKey("dbo.Carts", t => t.Cart_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Albums", t => t.Album_Id, cascadeDelete: true)
                .Index(t => t.Cart_UserId)
                .Index(t => t.Album_Id);
            
            CreateTable(
                "dbo.UserInfoUserInfoes",
                c => new
                    {
                        UserInfo_UserId = c.String(nullable: false, maxLength: 128),
                        UserInfo_UserId1 = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserInfo_UserId, t.UserInfo_UserId1 })
                .ForeignKey("dbo.UserInfoes", t => t.UserInfo_UserId)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfo_UserId1)
                .Index(t => t.UserInfo_UserId)
                .Index(t => t.UserInfo_UserId1);
            
            CreateTable(
                "dbo.TransactionAlbums",
                c => new
                    {
                        Transaction_Id = c.Int(nullable: false),
                        Album_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Transaction_Id, t.Album_Id })
                .ForeignKey("dbo.Transactions", t => t.Transaction_Id, cascadeDelete: true)
                .ForeignKey("dbo.Albums", t => t.Album_Id, cascadeDelete: true)
                .Index(t => t.Transaction_Id)
                .Index(t => t.Album_Id);
            
            CreateTable(
                "dbo.TransactionPictures",
                c => new
                    {
                        Transaction_Id = c.Int(nullable: false),
                        Picture_Id = c.Int(nullable: false),
                        Picture_OwnerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Transaction_Id, t.Picture_Id, t.Picture_OwnerId })
                .ForeignKey("dbo.Transactions", t => t.Transaction_Id, cascadeDelete: true)
                .ForeignKey("dbo.Pictures", t => new { t.Picture_Id, t.Picture_OwnerId }, cascadeDelete: true)
                .Index(t => t.Transaction_Id)
                .Index(t => new { t.Picture_Id, t.Picture_OwnerId });
            
            CreateTable(
                "dbo.PictureTags",
                c => new
                    {
                        Picture_Id = c.Int(nullable: false),
                        Picture_OwnerId = c.String(nullable: false, maxLength: 128),
                        Tag_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Picture_Id, t.Picture_OwnerId, t.Tag_Id })
                .ForeignKey("dbo.Pictures", t => new { t.Picture_Id, t.Picture_OwnerId }, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .Index(t => new { t.Picture_Id, t.Picture_OwnerId })
                .Index(t => t.Tag_Id);
            
            CreateTable(
                "dbo.CartPictures",
                c => new
                    {
                        Cart_UserId = c.String(nullable: false, maxLength: 128),
                        Picture_Id = c.Int(nullable: false),
                        Picture_OwnerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Cart_UserId, t.Picture_Id, t.Picture_OwnerId })
                .ForeignKey("dbo.Carts", t => t.Cart_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Pictures", t => new { t.Picture_Id, t.Picture_OwnerId }, cascadeDelete: true)
                .Index(t => t.Cart_UserId)
                .Index(t => new { t.Picture_Id, t.Picture_OwnerId });
            
            CreateTable(
                "dbo.CartPicture1",
                c => new
                    {
                        Cart_UserId = c.String(nullable: false, maxLength: 128),
                        Picture_Id = c.Int(nullable: false),
                        Picture_OwnerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Cart_UserId, t.Picture_Id, t.Picture_OwnerId })
                .ForeignKey("dbo.Carts", t => t.Cart_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Pictures", t => new { t.Picture_Id, t.Picture_OwnerId }, cascadeDelete: true)
                .Index(t => t.Cart_UserId)
                .Index(t => new { t.Picture_Id, t.Picture_OwnerId });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Albums", "UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.Carts", "UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.CartPicture1", new[] { "Picture_Id", "Picture_OwnerId" }, "dbo.Pictures");
            DropForeignKey("dbo.CartPicture1", "Cart_UserId", "dbo.Carts");
            DropForeignKey("dbo.CartPictures", new[] { "Picture_Id", "Picture_OwnerId" }, "dbo.Pictures");
            DropForeignKey("dbo.CartPictures", "Cart_UserId", "dbo.Carts");
            DropForeignKey("dbo.PictureTags", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.PictureTags", new[] { "Picture_Id", "Picture_OwnerId" }, "dbo.Pictures");
            DropForeignKey("dbo.Pictures", "OwnerId", "dbo.UserInfoes");
            DropForeignKey("dbo.UserInfoes", new[] { "Picture_Id", "Picture_OwnerId" }, "dbo.Pictures");
            DropForeignKey("dbo.UserInfoes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transactions", "UserInfo_UserId1", "dbo.UserInfoes");
            DropForeignKey("dbo.Transactions", "UserInfo_UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.Transactions", "SellerId", "dbo.UserInfoes");
            DropForeignKey("dbo.TransactionPictures", new[] { "Picture_Id", "Picture_OwnerId" }, "dbo.Pictures");
            DropForeignKey("dbo.TransactionPictures", "Transaction_Id", "dbo.Transactions");
            DropForeignKey("dbo.Transactions", "BuyerId", "dbo.UserInfoes");
            DropForeignKey("dbo.TransactionAlbums", "Album_Id", "dbo.Albums");
            DropForeignKey("dbo.TransactionAlbums", "Transaction_Id", "dbo.Transactions");
            DropForeignKey("dbo.Pictures", "UserInfo_UserId1", "dbo.UserInfoes");
            DropForeignKey("dbo.Pictures", "UserInfo_UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.UserInfoUserInfoes", "UserInfo_UserId1", "dbo.UserInfoes");
            DropForeignKey("dbo.UserInfoUserInfoes", "UserInfo_UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.Pictures", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.CartAlbum1", "Album_Id", "dbo.Albums");
            DropForeignKey("dbo.CartAlbum1", "Cart_UserId", "dbo.Carts");
            DropForeignKey("dbo.CartAlbums", "Album_Id", "dbo.Albums");
            DropForeignKey("dbo.CartAlbums", "Cart_UserId", "dbo.Carts");
            DropIndex("dbo.CartPicture1", new[] { "Picture_Id", "Picture_OwnerId" });
            DropIndex("dbo.CartPicture1", new[] { "Cart_UserId" });
            DropIndex("dbo.CartPictures", new[] { "Picture_Id", "Picture_OwnerId" });
            DropIndex("dbo.CartPictures", new[] { "Cart_UserId" });
            DropIndex("dbo.PictureTags", new[] { "Tag_Id" });
            DropIndex("dbo.PictureTags", new[] { "Picture_Id", "Picture_OwnerId" });
            DropIndex("dbo.TransactionPictures", new[] { "Picture_Id", "Picture_OwnerId" });
            DropIndex("dbo.TransactionPictures", new[] { "Transaction_Id" });
            DropIndex("dbo.TransactionAlbums", new[] { "Album_Id" });
            DropIndex("dbo.TransactionAlbums", new[] { "Transaction_Id" });
            DropIndex("dbo.UserInfoUserInfoes", new[] { "UserInfo_UserId1" });
            DropIndex("dbo.UserInfoUserInfoes", new[] { "UserInfo_UserId" });
            DropIndex("dbo.CartAlbum1", new[] { "Album_Id" });
            DropIndex("dbo.CartAlbum1", new[] { "Cart_UserId" });
            DropIndex("dbo.CartAlbums", new[] { "Album_Id" });
            DropIndex("dbo.CartAlbums", new[] { "Cart_UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Transactions", new[] { "UserInfo_UserId1" });
            DropIndex("dbo.Transactions", new[] { "UserInfo_UserId" });
            DropIndex("dbo.Transactions", new[] { "SellerId" });
            DropIndex("dbo.Transactions", new[] { "BuyerId" });
            DropIndex("dbo.UserInfoes", new[] { "Picture_Id", "Picture_OwnerId" });
            DropIndex("dbo.UserInfoes", new[] { "UserId" });
            DropIndex("dbo.Pictures", new[] { "UserInfo_UserId1" });
            DropIndex("dbo.Pictures", new[] { "UserInfo_UserId" });
            DropIndex("dbo.Pictures", new[] { "AlbumId" });
            DropIndex("dbo.Pictures", new[] { "OwnerId" });
            DropIndex("dbo.Carts", new[] { "UserId" });
            DropIndex("dbo.Albums", new[] { "UserId" });
            DropTable("dbo.CartPicture1");
            DropTable("dbo.CartPictures");
            DropTable("dbo.PictureTags");
            DropTable("dbo.TransactionPictures");
            DropTable("dbo.TransactionAlbums");
            DropTable("dbo.UserInfoUserInfoes");
            DropTable("dbo.CartAlbum1");
            DropTable("dbo.CartAlbums");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Tags");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Transactions");
            DropTable("dbo.UserInfoes");
            DropTable("dbo.Pictures");
            DropTable("dbo.Carts");
            DropTable("dbo.Albums");
        }
    }
}
