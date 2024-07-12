using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class latlngplaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lat",
                table: "Halls");

            migrationBuilder.DropColumn(
                name: "lng",
                table: "Halls");

            migrationBuilder.AddColumn<decimal>(
                name: "lat",
                table: "Places",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "lng",
                table: "Places",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lat",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "lng",
                table: "Places");

            migrationBuilder.AddColumn<decimal>(
                name: "lat",
                table: "Halls",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "lng",
                table: "Halls",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
