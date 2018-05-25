using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DevicesSimulation.Migrations
{
    public partial class new_devices1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Temperature",
                table: "Heaters",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "TemperatureSensorReadings",
                table: "AirConditioners",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "Temperature",
                table: "AirConditioners",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "MoveSensors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Company = table.Column<string>(nullable: true),
                    IsConect = table.Column<bool>(nullable: false),
                    IsMove = table.Column<bool>(nullable: false),
                    IsSecureMod = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Power = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveSensors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PowerSockets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Company = table.Column<string>(nullable: true),
                    IsConect = table.Column<bool>(nullable: false),
                    IsOn = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Power = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerSockets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemperatureSensors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Company = table.Column<string>(nullable: true),
                    IsConect = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Power = table.Column<bool>(nullable: false),
                    TemperatureSensorReadings = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemperatureSensors", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoveSensors");

            migrationBuilder.DropTable(
                name: "PowerSockets");

            migrationBuilder.DropTable(
                name: "TemperatureSensors");

            migrationBuilder.AlterColumn<int>(
                name: "Temperature",
                table: "Heaters",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "TemperatureSensorReadings",
                table: "AirConditioners",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Temperature",
                table: "AirConditioners",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
