using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatDemo1.Migrations
{
    /// <inheritdoc />
    public partial class changeOnMessageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlgorithmType",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlgorithmType",
                table: "Messages");
        }
    }
}
