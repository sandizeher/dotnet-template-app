using System;
using DotnetTemplateApp.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetTemplateApp.Persistence.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250919132609_InitialCreate")]
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "account");

            migrationBuilder.CreateTable(
                name: "user_accounts",
                schema: "account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_registered = table.Column<bool>(type: "boolean", nullable: false),
                    last_login = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    phone_number = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    phone_number_prefix = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    account_status = table.Column<string>(type: "text", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    firstname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    lastname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    middlename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    UserDisplayName = table.Column<string>(type: "text", nullable: true),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_user_account_id",
                        column: x => x.user_account_id,
                        principalSchema: "account",
                        principalTable: "user_accounts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "uniq_user_accounts_email",
                schema: "account",
                table: "user_accounts",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uniq_user_accounts_id",
                schema: "account",
                table: "user_accounts",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uniq_user_accounts_username",
                schema: "account",
                table: "user_accounts",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_user_account_id",
                schema: "account",
                table: "users",
                column: "user_account_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users",
                schema: "account");

            migrationBuilder.DropTable(
                name: "user_accounts",
                schema: "account");
        }
    }
}
