using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Forecast.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMeteorologist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MeteorologistId",
                schema: "forecast",
                table: "WeatherForecast",
                type: "uuid",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_WeatherForecast_MeteorologistId",
                schema: "forecast",
                table: "WeatherForecast",
                column: "MeteorologistId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeatherForecast_Meteorologist_MeteorologistId",
                schema: "forecast",
                table: "WeatherForecast",
                column: "MeteorologistId",
                principalSchema: "forecast",
                principalTable: "Meteorologist",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeatherForecast_Meteorologist_MeteorologistId",
                schema: "forecast",
                table: "WeatherForecast");

            migrationBuilder.DropTable(
                name: "Meteorologist",
                schema: "forecast");

            migrationBuilder.DropIndex(
                name: "IX_WeatherForecast_MeteorologistId",
                schema: "forecast",
                table: "WeatherForecast");

            migrationBuilder.DropColumn(
                name: "MeteorologistId",
                schema: "forecast",
                table: "WeatherForecast");
        }
    }
}
