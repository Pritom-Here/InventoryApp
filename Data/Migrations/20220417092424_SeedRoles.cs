using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryApp.Data.Migrations
{
    public partial class SeedRoles : Migration
    {
        private string AdminRoleId = Guid.NewGuid().ToString();
        private string CashierRoleId = Guid.NewGuid().ToString();

        private string UserOneId = Guid.NewGuid().ToString();
        private string UserTwoId = Guid.NewGuid().ToString();

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            SeedUsersSql(migrationBuilder);
            SeedRolesSql(migrationBuilder);
            SeedUserRolesSql(migrationBuilder);
        }

        private void SeedUsersSql(MigrationBuilder migrationBuilder) 
        {
            migrationBuilder.Sql(
                        $@"INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ProfilePicture]) 
                        VALUES ('{UserOneId}', N'admin@inventory.com', N'ADMIN@INVENTORY.COM', N'admin@inventory.com', N'ADMIN@INVENTORY.COM', 1, N'AQAAAAEAACcQAAAAEHwI/wmiAqPA4lp1uSxzM7cT5k794xoi9UY/9I9wD3OOBN2ZOrSXVRi5ycpUBXLgDA==', N'QOBUR5RWK24WOM4TJW4DNZD3X3NEEVRR', N'6d48793a-59d9-4f1b-8f9a-66e1c7d36c40', NULL, 0, 0, NULL, 1, 0, N'Pritom', N'Chakraborty', NULL)"
                    );
            migrationBuilder.Sql(
                        $@"INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ProfilePicture]) 
                        VALUES ('{UserTwoId}', N'cashier@inventory.com', N'CASHIER@INVENTORY.COM', N'cashier@inventory.com', N'CASHIER@INVENTORY.COM', 1, N'AQAAAAEAACcQAAAAEHwI/wmiAqPA4lp1uSxzM7cT5k794xoi9UY/9I9wD3OOBN2ZOrSXVRi5ycpUBXLgDA==', N'QOBUR5RWK24WOM4TJW4DNZD3X3NEEVRR', N'6d48793a-59d9-4f1b-8f9a-66e1c7d36c40', NULL, 0, 0, NULL, 1, 0, N'Jason', N'Todd', NULL)"
                    );
        }

        private void SeedRolesSql(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                        $@"INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
                        VALUES ('{AdminRoleId}', N'Administrator', N'ADMINISTRATOR', NULL)"
                    );
            migrationBuilder.Sql(
                        $@"INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
                        VALUES ('{CashierRoleId}', N'Cashier', N'CASHIER', NULL)"
                    );
        }

        private void SeedUserRolesSql(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES ('{UserOneId}', '{AdminRoleId}')");
            migrationBuilder.Sql($@"INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES ('{UserTwoId}', '{CashierRoleId}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
