using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wing.SqlServer.Migrations
{
    public partial class GateWayLogUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestType",
                table: "GateWay_Log",
                newName: "RequestMethod");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResponseTime",
                table: "GateWay_Log",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequestUrl",
                table: "GateWay_Log",
                type: "varchar(8000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestTime",
                table: "GateWay_Log",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DownstreamUrl",
                table: "GateWay_Log",
                type: "varchar(8000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthKey",
                table: "GateWay_Log",
                type: "varchar(8000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "GateWay_Log",
                type: "varchar(8000)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthKey",
                table: "GateWay_Log");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "GateWay_Log");

            migrationBuilder.RenameColumn(
                name: "RequestMethod",
                table: "GateWay_Log",
                newName: "RequestType");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ResponseTime",
                table: "GateWay_Log",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "RequestUrl",
                table: "GateWay_Log",
                type: "varchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(8000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestTime",
                table: "GateWay_Log",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "DownstreamUrl",
                table: "GateWay_Log",
                type: "varchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(8000)",
                oldNullable: true);
        }
    }
}
