using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Skud.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init_after_fix_bug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "access_levels",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    level_name = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_levels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "doors",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    location = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AccessLevelDoor",
                columns: table => new
                {
                    AccessLevelsId = table.Column<int>(type: "integer", nullable: false),
                    DoorsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLevelDoor", x => new { x.AccessLevelsId, x.DoorsId });
                    table.ForeignKey(
                        name: "FK_AccessLevelDoor_access_levels_AccessLevelsId",
                        column: x => x.AccessLevelsId,
                        principalTable: "access_levels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessLevelDoor_doors_DoorsId",
                        column: x => x.DoorsId,
                        principalTable: "doors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRole",
                columns: table => new
                {
                    PermissionsId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionsId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_PermissionRole_permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionRole_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    password_salt = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    access_level_id = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_access_levels_access_level_id",
                        column: x => x.access_level_id,
                        principalTable: "access_levels",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_users_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "access_records",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    door_id = table.Column<int>(type: "integer", nullable: false),
                    access_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_entry = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_records", x => x.id);
                    table.ForeignKey(
                        name: "FK_access_records_doors_door_id",
                        column: x => x.door_id,
                        principalTable: "doors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_access_records_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cards",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: true),
                    updated_by = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cards", x => x.id);
                    table.ForeignKey(
                        name: "FK_cards_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "access_levels",
                columns: new[] { "id", "created_by", "created_date", "level_name", "updated_by", "updated_date" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Employee", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Guest", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "doors",
                columns: new[] { "id", "created_by", "created_date", "location", "updated_by", "updated_date" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1 - floor", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2 - floor", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3 - floor", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "created_by", "created_date", "name", "updated_by", "updated_date" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1020), "CreateOrUpdateSp", 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1029) },
                    { 2, 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1031), "DeleteSp", 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1031) },
                    { 3, 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1049), "GetUsers", 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1050) },
                    { 4, 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1050), "BlockOrUnlockUser", 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1051) },
                    { 5, 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1052), "DeleteUser", 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1052) },
                    { 6, 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1053), "CreateOrUpdateUser", 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1053) },
                    { 7, 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1053), "CreateOrUpdateRole", 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1054) },
                    { 8, 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1054), "DeleteRole", 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1055) },
                    { 9, 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1055), "GetRole", 0, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1055) }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "created_by", "created_date", "name", "updated_by", "updated_date" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Moderator", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "access_level_id", "created_by", "created_date", "email", "full_name", "password_hash", "password_salt", "phone_number", "role_id", "status", "updated_by", "updated_date" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1108), "example1@gmail.com", "John Doe", "04e17720763c4a5203a6b3aea4b2df3551aedd0f55438dad8d6987a2dcb1ed78ad5b86afa3abe481ecab4b3d1e45aa1a86d3d0754208c05fac0c54aee27898f1", "3a5f6ebb381a0b11937f3e96263eb087e09b7c2789e1f9e90d41d74cec6573f8", "+998901234567", 1, 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, null, new DateTime(2024, 9, 24, 9, 40, 38, 761, DateTimeKind.Local).AddTicks(1109), "example2@gmail.com", "Mike Tyson", "04e17720763c4a5203a6b3aea4b2df3551aedd0f55438dad8d6987a2dcb1ed78ad5b86afa3abe481ecab4b3d1e45aa1a86d3d0754208c05fac0c54aee27898f1", "3a5f6ebb381a0b11937f3e96263eb087e09b7c2789e1f9e90d41d74cec6573f8", "+998901234569", 2, 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "cards",
                columns: new[] { "id", "created_by", "created_date", "is_active", "updated_by", "updated_date", "user_id" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessLevelDoor_DoorsId",
                table: "AccessLevelDoor",
                column: "DoorsId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_RoleId",
                table: "PermissionRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_access_records_door_id",
                table: "access_records",
                column: "door_id");

            migrationBuilder.CreateIndex(
                name: "IX_access_records_user_id",
                table: "access_records",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_cards_user_id",
                table: "cards",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_access_level_id",
                table: "users",
                column: "access_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessLevelDoor");

            migrationBuilder.DropTable(
                name: "PermissionRole");

            migrationBuilder.DropTable(
                name: "access_records");

            migrationBuilder.DropTable(
                name: "cards");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "doors");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "access_levels");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
