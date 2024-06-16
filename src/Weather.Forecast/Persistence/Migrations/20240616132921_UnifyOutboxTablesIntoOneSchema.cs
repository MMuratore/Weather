using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Forecast.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UnifyOutboxTablesIntoOneSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxIntegrationEvent",
                schema: "forecast",
                table: "OutboxIntegrationEvent");

            migrationBuilder.EnsureSchema(
                name: "outbox");

            migrationBuilder.RenameTable(
                name: "OutboxIntegrationEvent",
                schema: "forecast",
                newName: "OutboxMessage",
                newSchema: "outbox");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxMessage",
                schema: "outbox",
                table: "OutboxMessage",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxMessage",
                schema: "outbox",
                table: "OutboxMessage");

            migrationBuilder.RenameTable(
                name: "OutboxMessage",
                schema: "outbox",
                newName: "OutboxIntegrationEvent",
                newSchema: "forecast");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxIntegrationEvent",
                schema: "forecast",
                table: "OutboxIntegrationEvent",
                column: "Id");
        }
    }
}
