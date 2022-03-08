using Microsoft.EntityFrameworkCore.Migrations;

namespace Wing.SqlServer.Migrations
{
    public partial class GateWayLogUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GateWayServerIp",
                table: "GateWay_Log",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceAddress",
                table: "GateWay_Log",
                type: "varchar(200)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GateWayServerIp",
                table: "GateWay_Log");

            migrationBuilder.DropColumn(
                name: "ServiceAddress",
                table: "GateWay_Log");
        }
    }
}
