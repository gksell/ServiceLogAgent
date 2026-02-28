using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceLogAgent.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CorrelationId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    TraceId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DurationMs = table.Column<long>(type: "INTEGER", nullable: false),
                    HttpMethod = table.Column<string>(type: "TEXT", maxLength: 16, nullable: true),
                    Path = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    QueryString = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    RequestHeadersJson = table.Column<string>(type: "TEXT", nullable: true),
                    RequestBody = table.Column<string>(type: "TEXT", nullable: true),
                    ResponseStatusCode = table.Column<int>(type: "INTEGER", nullable: false),
                    ResponseHeadersJson = table.Column<string>(type: "TEXT", nullable: true),
                    ResponseBody = table.Column<string>(type: "TEXT", nullable: true),
                    RemoteIp = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ApplicationKey = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    IsSuccess = table.Column<bool>(type: "INTEGER", nullable: false),
                    ErrorMessage = table.Column<string>(type: "TEXT", maxLength: 4096, nullable: true),
                    ExceptionStack = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLogs_CreatedAtUtc",
                table: "ServiceLogs",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLogs_ResponseStatusCode",
                table: "ServiceLogs",
                column: "ResponseStatusCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceLogs");
        }
    }
}
