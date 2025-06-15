using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_SE1914_ManageHospital.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToMedicineAndCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
  
            migrationBuilder.CreateIndex(
                name: "IX_Medicines_UnitId",
                table: "Medicines",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_Units_UnitId",
                table: "Medicines",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_Units_UnitId",
                table: "Medicines");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Medicines_UnitId",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Medicines");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Medicines",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
