using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class addDs18b20Sensor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ds18B20Measurements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SensorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Temperature = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ds18B20Measurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ds18B20Measurements_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ds18B20Measurements_SensorId",
                table: "Ds18B20Measurements",
                column: "SensorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ds18B20Measurements");
        }
    }
}
