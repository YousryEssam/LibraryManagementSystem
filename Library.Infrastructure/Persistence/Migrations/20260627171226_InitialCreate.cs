using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Authors",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Bio = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                Nationality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Authors", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Categories",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Categories", x => x.Id);
                table.ForeignKey(
                    name: "FK_Categories_Categories_ParentCategoryId",
                    column: x => x.ParentCategoryId,
                    principalTable: "Categories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Members",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                MembershipDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                MembershipExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                Status = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Members", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Publishers",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                ContactEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                Website = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Publishers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Roles",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SystemUsers",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Role = table.Column<int>(type: "int", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SystemUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Books",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                ISBN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                Edition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                PublicationYear = table.Column<int>(type: "int", nullable: true),
                Language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Summary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                CoverImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                Status = table.Column<int>(type: "int", nullable: false),
                PublisherId = table.Column<int>(type: "int", nullable: false),
                CategoryId = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Books", x => x.Id);
                table.ForeignKey(
                    name: "FK_Books_Categories_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "Categories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Books_Publishers_PublisherId",
                    column: x => x.PublisherId,
                    principalTable: "Publishers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "RoleClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<int>(type: "int", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_RoleClaims_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ActivityLogs",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                SystemUserId = table.Column<int>(type: "int", nullable: false),
                Action = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                EntityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                EntityId = table.Column<int>(type: "int", nullable: true),
                Details = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                table.ForeignKey(
                    name: "FK_ActivityLogs_SystemUsers_SystemUserId",
                    column: x => x.SystemUserId,
                    principalTable: "SystemUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "UserClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<int>(type: "int", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserClaims_SystemUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "SystemUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserLogins",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UserId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_UserLogins_SystemUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "SystemUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserRoles",
            columns: table => new
            {
                UserId = table.Column<int>(type: "int", nullable: false),
                RoleId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_UserRoles_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserRoles_SystemUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "SystemUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserTokens",
            columns: table => new
            {
                UserId = table.Column<int>(type: "int", nullable: false),
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_UserTokens_SystemUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "SystemUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "BookAuthors",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                BookId = table.Column<int>(type: "int", nullable: false),
                AuthorId = table.Column<int>(type: "int", nullable: false),
                Role = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BookAuthors", x => x.Id);
                table.UniqueConstraint("AK_BookAuthors_BookId_AuthorId", x => new { x.BookId, x.AuthorId });
                table.ForeignKey(
                    name: "FK_BookAuthors_Authors_AuthorId",
                    column: x => x.AuthorId,
                    principalTable: "Authors",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_BookAuthors_Books_BookId",
                    column: x => x.BookId,
                    principalTable: "Books",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "BorrowingTransactions",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                BookId = table.Column<int>(type: "int", nullable: false),
                MemberId = table.Column<int>(type: "int", nullable: false),
                ProcessedByUserId = table.Column<int>(type: "int", nullable: false),
                BorrowedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                ReturnedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                Status = table.Column<int>(type: "int", nullable: false),
                Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BorrowingTransactions", x => x.Id);
                table.ForeignKey(
                    name: "FK_BorrowingTransactions_Books_BookId",
                    column: x => x.BookId,
                    principalTable: "Books",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_BorrowingTransactions_Members_MemberId",
                    column: x => x.MemberId,
                    principalTable: "Members",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_BorrowingTransactions_SystemUsers_ProcessedByUserId",
                    column: x => x.ProcessedByUserId,
                    principalTable: "SystemUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ActivityLogs_Entity",
            table: "ActivityLogs",
            columns: new[] { "EntityName", "EntityId" });

        migrationBuilder.CreateIndex(
            name: "IX_ActivityLogs_SystemUserId",
            table: "ActivityLogs",
            column: "SystemUserId");

        migrationBuilder.CreateIndex(
            name: "IX_ActivityLogs_Timestamp",
            table: "ActivityLogs",
            column: "Timestamp");

        migrationBuilder.CreateIndex(
            name: "IX_Authors_FullName",
            table: "Authors",
            column: "FullName");

        migrationBuilder.CreateIndex(
            name: "IX_BookAuthors_AuthorId",
            table: "BookAuthors",
            column: "AuthorId");

        migrationBuilder.CreateIndex(
            name: "IX_BookAuthors_BookId",
            table: "BookAuthors",
            column: "BookId");

        migrationBuilder.CreateIndex(
            name: "IX_Books_CategoryId",
            table: "Books",
            column: "CategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_Books_ISBN",
            table: "Books",
            column: "ISBN",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Books_PublisherId",
            table: "Books",
            column: "PublisherId");

        migrationBuilder.CreateIndex(
            name: "IX_Books_Status",
            table: "Books",
            column: "Status");

        migrationBuilder.CreateIndex(
            name: "IX_Books_Title",
            table: "Books",
            column: "Title");

        migrationBuilder.CreateIndex(
            name: "IX_BorrowingTransactions_BookId",
            table: "BorrowingTransactions",
            column: "BookId");

        migrationBuilder.CreateIndex(
            name: "IX_BorrowingTransactions_DueDate",
            table: "BorrowingTransactions",
            column: "DueDate");

        migrationBuilder.CreateIndex(
            name: "IX_BorrowingTransactions_MemberId",
            table: "BorrowingTransactions",
            column: "MemberId");

        migrationBuilder.CreateIndex(
            name: "IX_BorrowingTransactions_ProcessedByUserId",
            table: "BorrowingTransactions",
            column: "ProcessedByUserId");

        migrationBuilder.CreateIndex(
            name: "IX_BorrowingTransactions_Status",
            table: "BorrowingTransactions",
            column: "Status");

        migrationBuilder.CreateIndex(
            name: "IX_Categories_Name",
            table: "Categories",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_Categories_ParentCategoryId",
            table: "Categories",
            column: "ParentCategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_Members_Email",
            table: "Members",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Members_FullName",
            table: "Members",
            column: "FullName");

        migrationBuilder.CreateIndex(
            name: "IX_Members_Status",
            table: "Members",
            column: "Status");

        migrationBuilder.CreateIndex(
            name: "IX_Publishers_Name",
            table: "Publishers",
            column: "Name");

        migrationBuilder.CreateIndex(
            name: "IX_RoleClaims_RoleId",
            table: "RoleClaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            table: "Roles",
            column: "NormalizedName",
            unique: true,
            filter: "[NormalizedName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "SystemUsers",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "IX_SystemUsers_IsActive",
            table: "SystemUsers",
            column: "IsActive");

        migrationBuilder.CreateIndex(
            name: "IX_SystemUsers_Role",
            table: "SystemUsers",
            column: "Role");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            table: "SystemUsers",
            column: "NormalizedUserName",
            unique: true,
            filter: "[NormalizedUserName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_UserClaims_UserId",
            table: "UserClaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserLogins_UserId",
            table: "UserLogins",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserRoles_RoleId",
            table: "UserRoles",
            column: "RoleId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ActivityLogs");

        migrationBuilder.DropTable(
            name: "BookAuthors");

        migrationBuilder.DropTable(
            name: "BorrowingTransactions");

        migrationBuilder.DropTable(
            name: "RoleClaims");

        migrationBuilder.DropTable(
            name: "UserClaims");

        migrationBuilder.DropTable(
            name: "UserLogins");

        migrationBuilder.DropTable(
            name: "UserRoles");

        migrationBuilder.DropTable(
            name: "UserTokens");

        migrationBuilder.DropTable(
            name: "Authors");

        migrationBuilder.DropTable(
            name: "Books");

        migrationBuilder.DropTable(
            name: "Members");

        migrationBuilder.DropTable(
            name: "Roles");

        migrationBuilder.DropTable(
            name: "SystemUsers");

        migrationBuilder.DropTable(
            name: "Categories");

        migrationBuilder.DropTable(
            name: "Publishers");
    }
}
