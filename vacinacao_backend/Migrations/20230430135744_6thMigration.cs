using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vacinacao_backend.Migrations
{
    /// <inheritdoc />
    public partial class _6thMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VacinaId",
                table: "Alergias",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alergias_VacinaId",
                table: "Alergias",
                column: "VacinaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Alergias_Vacinas_VacinaId",
                table: "Alergias",
                column: "VacinaId",
                principalTable: "Vacinas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alergias_Vacinas_VacinaId",
                table: "Alergias");

            migrationBuilder.DropIndex(
                name: "IX_Alergias_VacinaId",
                table: "Alergias");

            migrationBuilder.DropColumn(
                name: "VacinaId",
                table: "Alergias");
        }
    }
}
