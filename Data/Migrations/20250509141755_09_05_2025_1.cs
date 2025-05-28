using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerTracker.Migrations
{
    /// <inheritdoc />
    public partial class _09_05_2025_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CPUUsage",
                table: "ComputerSystemDatas");

            migrationBuilder.DropColumn(
                name: "DiskUsage",
                table: "ComputerSystemDatas");

            migrationBuilder.DropColumn(
                name: "MemoryUsage",
                table: "ComputerSystemDatas");

            migrationBuilder.DropColumn(
                name: "NetworkUsage",
                table: "ComputerSystemDatas");

            migrationBuilder.AddColumn<string>(
                name: "CPUName",
                table: "ComputerSystemDatas",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ComputerSystemDataSystemDataID",
                table: "AppUsageEntries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsageEntries_ComputerSystemDataSystemDataID",
                table: "AppUsageEntries",
                column: "ComputerSystemDataSystemDataID");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsageEntries_ComputerSystemDatas_ComputerSystemDataSystemDataID",
                table: "AppUsageEntries",
                column: "ComputerSystemDataSystemDataID",
                principalTable: "ComputerSystemDatas",
                principalColumn: "SystemDataID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsageEntries_ComputerSystemDatas_ComputerSystemDataSystemDataID",
                table: "AppUsageEntries");

            migrationBuilder.DropIndex(
                name: "IX_AppUsageEntries_ComputerSystemDataSystemDataID",
                table: "AppUsageEntries");

            migrationBuilder.DropColumn(
                name: "CPUName",
                table: "ComputerSystemDatas");

            migrationBuilder.DropColumn(
                name: "ComputerSystemDataSystemDataID",
                table: "AppUsageEntries");

            migrationBuilder.AddColumn<double>(
                name: "CPUUsage",
                table: "ComputerSystemDatas",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DiskUsage",
                table: "ComputerSystemDatas",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MemoryUsage",
                table: "ComputerSystemDatas",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NetworkUsage",
                table: "ComputerSystemDatas",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
