using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Forecast.Common.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "forecast");

            migrationBuilder.CreateTable(
                name: "Meteorologist",
                schema: "forecast",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ForecastCount = table.Column<int>(type: "integer", nullable: false),
                    BirthDay_Date = table.Column<DateOnly>(type: "date", nullable: false),
                    BirthDay_Hour = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    Firstname = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Lastname = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meteorologist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                schema: "forecast",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Content = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CompleteTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UncaughtExceptions = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherForecast",
                schema: "forecast",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Celsius = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    Summary = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    MeteorologistId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecast", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherForecast_Meteorologist_MeteorologistId",
                        column: x => x.MeteorologistId,
                        principalSchema: "forecast",
                        principalTable: "Meteorologist",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_CompleteTime",
                schema: "forecast",
                table: "OutboxMessage",
                column: "CompleteTime");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_CreationTime",
                schema: "forecast",
                table: "OutboxMessage",
                column: "CreationTime",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_WeatherForecast_MeteorologistId",
                schema: "forecast",
                table: "WeatherForecast",
                column: "MeteorologistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboxMessage",
                schema: "forecast");

            migrationBuilder.DropTable(
                name: "WeatherForecast",
                schema: "forecast");

            migrationBuilder.DropTable(
                name: "Meteorologist",
                schema: "forecast");
        }
    }
}
