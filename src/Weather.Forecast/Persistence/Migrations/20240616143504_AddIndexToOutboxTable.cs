using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Forecast.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToOutboxTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_CompleteTime",
                schema: "outbox",
                table: "OutboxMessage",
                column: "CompleteTime");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_CreationTime",
                schema: "outbox",
                table: "OutboxMessage",
                column: "CreationTime",
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OutboxMessage_CompleteTime",
                schema: "outbox",
                table: "OutboxMessage");

            migrationBuilder.DropIndex(
                name: "IX_OutboxMessage_CreationTime",
                schema: "outbox",
                table: "OutboxMessage");
        }
    }
}
