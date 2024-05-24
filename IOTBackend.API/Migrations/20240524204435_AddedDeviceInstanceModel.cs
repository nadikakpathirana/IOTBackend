using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IOTBackend.API.Migrations
{
    public partial class AddedDeviceInstanceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceInstances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    XCordinate = table.Column<double>(type: "double precision", nullable: false),
                    YCordinate = table.Column<double>(type: "double precision", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceInstances_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceInstances_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceInstances_DeviceId",
                table: "DeviceInstances",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceInstances_ProjectId",
                table: "DeviceInstances",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceInstances");
        }
    }
}
