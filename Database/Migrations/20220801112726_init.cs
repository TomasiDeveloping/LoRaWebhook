using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SensorTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DevAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SensorTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensors_SensorTypes_SensorTypeId",
                        column: x => x.SensorTypeId,
                        principalTable: "SensorTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Bme280Measurements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SensorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Temperature = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    Humidity = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Pressure = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bme280Measurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bme280Measurements_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bme280Measurements_SensorId",
                table: "Bme280Measurements",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_DevAddress",
                table: "Sensors",
                column: "DevAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_SensorTypeId",
                table: "Sensors",
                column: "SensorTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bme280Measurements");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "SensorTypes");
        }
    }
}
