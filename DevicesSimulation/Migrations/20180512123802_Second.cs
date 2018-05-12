using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DevicesSimulation.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConect",
                table: "Lamps",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AirConditioners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Company = table.Column<string>(nullable: true),
                    IsConect = table.Column<bool>(nullable: false),
                    IsWater = table.Column<bool>(nullable: false),
                    Mode = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Power = table.Column<bool>(nullable: false),
                    Temperature = table.Column<int>(nullable: false),
                    TemperatureSensorReadings = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirConditioners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoorLocks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Company = table.Column<string>(nullable: true),
                    IsConect = table.Column<bool>(nullable: false),
                    IsOpen = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Power = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorLocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Heaters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Company = table.Column<string>(nullable: true),
                    IsConect = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Power = table.Column<bool>(nullable: false),
                    Temperature = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Heaters", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirConditioners");

            migrationBuilder.DropTable(
                name: "DoorLocks");

            migrationBuilder.DropTable(
                name: "Heaters");

            migrationBuilder.DropColumn(
                name: "IsConect",
                table: "Lamps");
        }
    }
}
