using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CargoTrack.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig_cargo_package_size_and_transfer_center : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PackageSize",
                table: "Cargos",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<Guid>(
                name: "TransferCenterId",
                table: "CargoMovements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CargoMovements_TransferCenterId",
                table: "CargoMovements",
                column: "TransferCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_CargoMovements_TransferCenters_TransferCenterId",
                table: "CargoMovements",
                column: "TransferCenterId",
                principalTable: "TransferCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CargoMovements_TransferCenters_TransferCenterId",
                table: "CargoMovements");

            migrationBuilder.DropIndex(
                name: "IX_CargoMovements_TransferCenterId",
                table: "CargoMovements");

            migrationBuilder.DropColumn(
                name: "PackageSize",
                table: "Cargos");

            migrationBuilder.DropColumn(
                name: "TransferCenterId",
                table: "CargoMovements");
        }
    }
}
