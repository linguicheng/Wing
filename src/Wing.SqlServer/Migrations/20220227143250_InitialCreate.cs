using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wing.SqlServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GateWay_Log",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(32)", nullable: false),
                    ServiceName = table.Column<string>(type: "varchar(800)", nullable: true),
                    DownstreamUrl = table.Column<string>(type: "varchar(max)", nullable: true),
                    RequestUrl = table.Column<string>(type: "varchar(max)", nullable: true),
                    ClientIp = table.Column<string>(type: "varchar(20)", nullable: true),
                    RequestTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    RequestType = table.Column<string>(type: "varchar(20)", nullable: true),
                    RequestValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ResponseValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    Policy = table.Column<string>(type: "varchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GateWay_Log", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GateWay_Log");
        }
    }
}
