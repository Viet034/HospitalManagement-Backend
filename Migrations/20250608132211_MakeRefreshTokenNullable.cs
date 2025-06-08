using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391_SE1914_ManageHospital.Migrations
{
    /// <inheritdoc />
    public partial class MakeRefreshTokenNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "MedicineCategories",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "MedicineCategories",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "MedicineCategories",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdateBy",
                table: "MedicineCategories",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "MedicineCategories",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "MedicineCategories");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "MedicineCategories");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "MedicineCategories");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "MedicineCategories");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "MedicineCategories");
        }
    }
}
