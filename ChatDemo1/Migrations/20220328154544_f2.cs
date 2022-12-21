using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatDemo1.Migrations
{
    public partial class f2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Create_Time",
                table: "chats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Create_Time",
                table: "chats");
        }
    }
}
