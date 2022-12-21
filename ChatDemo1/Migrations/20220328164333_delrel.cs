using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatDemo1.Migrations
{
    public partial class delrel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_chats_chatId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "ApplicationUserUserChatModel");

            migrationBuilder.DropTable(
                name: "ChatUserChatModel");

            migrationBuilder.DropTable(
                name: "chats");

            migrationBuilder.DropTable(
                name: "UserChatModel");

            migrationBuilder.DropIndex(
                name: "IX_Messages_chatId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "chatId",
                table: "Messages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "chatId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create_Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserChatModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    chatsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChatModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserUserChatModel",
                columns: table => new
                {
                    UserChatModelsId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserUserChatModel", x => new { x.UserChatModelsId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserUserChatModel_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserUserChatModel_UserChatModel_UserChatModelsId",
                        column: x => x.UserChatModelsId,
                        principalTable: "UserChatModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatUserChatModel",
                columns: table => new
                {
                    UserChatModelsId = table.Column<int>(type: "int", nullable: false),
                    chatsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUserChatModel", x => new { x.UserChatModelsId, x.chatsId });
                    table.ForeignKey(
                        name: "FK_ChatUserChatModel_chats_chatsId",
                        column: x => x.chatsId,
                        principalTable: "chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatUserChatModel_UserChatModel_UserChatModelsId",
                        column: x => x.UserChatModelsId,
                        principalTable: "UserChatModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_chatId",
                table: "Messages",
                column: "chatId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserUserChatModel_UserId",
                table: "ApplicationUserUserChatModel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatUserChatModel_chatsId",
                table: "ChatUserChatModel",
                column: "chatsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_chats_chatId",
                table: "Messages",
                column: "chatId",
                principalTable: "chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
