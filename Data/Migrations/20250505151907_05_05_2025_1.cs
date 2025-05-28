using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerTracker.Migrations
{
    /// <inheritdoc />
    public partial class _05_05_2025_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentID",
                table: "Employees");

            migrationBuilder.AlterColumn<string>(
                name: "OSVersion",
                table: "ComputerSystemDatas",
                type: "varchar(150)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AddColumn<int>(
                name: "CpuClockMHz",
                table: "ComputerSystemDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CpuCores",
                table: "ComputerSystemDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CpuThreads",
                table: "ComputerSystemDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OSCaption",
                table: "ComputerSystemDatas",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OSManufacturer",
                table: "ComputerSystemDatas",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WindowsDirectory",
                table: "ComputerSystemDatas",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Host",
                table: "Computers",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "Computers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppUsageEntries",
                columns: table => new
                {
                    AppUsageEntryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComputerId = table.Column<int>(type: "int", nullable: false),
                    WindowTitle = table.Column<string>(type: "varchar(200)", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsageEntries", x => x.AppUsageEntryID);
                    table.ForeignKey(
                        name: "FK_AppUsageEntries_Computers_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computers",
                        principalColumn: "ComputerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gpus",
                columns: table => new
                {
                    ComputerGpuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComputerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    DriverVersion = table.Column<string>(type: "varchar(50)", nullable: false),
                    AdapterRAM = table.Column<long>(type: "bigint", nullable: false),
                    ComputerSystemDataSystemDataID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gpus", x => x.ComputerGpuID);
                    table.ForeignKey(
                        name: "FK_Gpus_ComputerSystemDatas_ComputerSystemDataSystemDataID",
                        column: x => x.ComputerSystemDataSystemDataID,
                        principalTable: "ComputerSystemDatas",
                        principalColumn: "SystemDataID");
                    table.ForeignKey(
                        name: "FK_Gpus_Computers_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computers",
                        principalColumn: "ComputerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Keyboards",
                columns: table => new
                {
                    KeyboardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComputerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", nullable: false),
                    DeviceID = table.Column<string>(type: "varchar(200)", nullable: false),
                    Manufacturer = table.Column<string>(type: "varchar(100)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    ComputerSystemDataSystemDataID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keyboards", x => x.KeyboardID);
                    table.ForeignKey(
                        name: "FK_Keyboards_ComputerSystemDatas_ComputerSystemDataSystemDataID",
                        column: x => x.ComputerSystemDataSystemDataID,
                        principalTable: "ComputerSystemDatas",
                        principalColumn: "SystemDataID");
                    table.ForeignKey(
                        name: "FK_Keyboards_Computers_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computers",
                        principalColumn: "ComputerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KeyLogEntries",
                columns: table => new
                {
                    KeyLogEntryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComputerId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "varchar(10)", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ComputerSystemDataSystemDataID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyLogEntries", x => x.KeyLogEntryID);
                    table.ForeignKey(
                        name: "FK_KeyLogEntries_ComputerSystemDatas_ComputerSystemDataSystemDataID",
                        column: x => x.ComputerSystemDataSystemDataID,
                        principalTable: "ComputerSystemDatas",
                        principalColumn: "SystemDataID");
                    table.ForeignKey(
                        name: "FK_KeyLogEntries_Computers_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computers",
                        principalColumn: "ComputerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mice",
                columns: table => new
                {
                    MouseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComputerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", nullable: false),
                    DeviceID = table.Column<string>(type: "varchar(200)", nullable: false),
                    Manufacturer = table.Column<string>(type: "varchar(100)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    ComputerSystemDataSystemDataID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mice", x => x.MouseID);
                    table.ForeignKey(
                        name: "FK_Mice_ComputerSystemDatas_ComputerSystemDataSystemDataID",
                        column: x => x.ComputerSystemDataSystemDataID,
                        principalTable: "ComputerSystemDatas",
                        principalColumn: "SystemDataID");
                    table.ForeignKey(
                        name: "FK_Mice_Computers_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computers",
                        principalColumn: "ComputerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Monitors",
                columns: table => new
                {
                    MonitorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComputerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Manufacturer = table.Column<string>(type: "varchar(100)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", nullable: false),
                    ComputerSystemDataSystemDataID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monitors", x => x.MonitorID);
                    table.ForeignKey(
                        name: "FK_Monitors_ComputerSystemDatas_ComputerSystemDataSystemDataID",
                        column: x => x.ComputerSystemDataSystemDataID,
                        principalTable: "ComputerSystemDatas",
                        principalColumn: "SystemDataID");
                    table.ForeignKey(
                        name: "FK_Monitors_Computers_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computers",
                        principalColumn: "ComputerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Printers",
                columns: table => new
                {
                    PrinterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComputerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Status = table.Column<string>(type: "varchar(100)", nullable: false),
                    ComputerSystemDataSystemDataID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Printers", x => x.PrinterID);
                    table.ForeignKey(
                        name: "FK_Printers_ComputerSystemDatas_ComputerSystemDataSystemDataID",
                        column: x => x.ComputerSystemDataSystemDataID,
                        principalTable: "ComputerSystemDatas",
                        principalColumn: "SystemDataID");
                    table.ForeignKey(
                        name: "FK_Printers_Computers_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computers",
                        principalColumn: "ComputerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scanners",
                columns: table => new
                {
                    ScannerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComputerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", nullable: false),
                    Status = table.Column<string>(type: "varchar(100)", nullable: false),
                    ComputerSystemDataSystemDataID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scanners", x => x.ScannerID);
                    table.ForeignKey(
                        name: "FK_Scanners_ComputerSystemDatas_ComputerSystemDataSystemDataID",
                        column: x => x.ComputerSystemDataSystemDataID,
                        principalTable: "ComputerSystemDatas",
                        principalColumn: "SystemDataID");
                    table.ForeignKey(
                        name: "FK_Scanners_Computers_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computers",
                        principalColumn: "ComputerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Computers_Host_Port",
                table: "Computers",
                columns: new[] { "Host", "Port" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsageEntries_ComputerId",
                table: "AppUsageEntries",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_Gpus_ComputerId",
                table: "Gpus",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_Gpus_ComputerSystemDataSystemDataID",
                table: "Gpus",
                column: "ComputerSystemDataSystemDataID");

            migrationBuilder.CreateIndex(
                name: "IX_Keyboards_ComputerId",
                table: "Keyboards",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_Keyboards_ComputerSystemDataSystemDataID",
                table: "Keyboards",
                column: "ComputerSystemDataSystemDataID");

            migrationBuilder.CreateIndex(
                name: "IX_KeyLogEntries_ComputerId",
                table: "KeyLogEntries",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyLogEntries_ComputerSystemDataSystemDataID",
                table: "KeyLogEntries",
                column: "ComputerSystemDataSystemDataID");

            migrationBuilder.CreateIndex(
                name: "IX_Mice_ComputerId",
                table: "Mice",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_Mice_ComputerSystemDataSystemDataID",
                table: "Mice",
                column: "ComputerSystemDataSystemDataID");

            migrationBuilder.CreateIndex(
                name: "IX_Monitors_ComputerId",
                table: "Monitors",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_Monitors_ComputerSystemDataSystemDataID",
                table: "Monitors",
                column: "ComputerSystemDataSystemDataID");

            migrationBuilder.CreateIndex(
                name: "IX_Printers_ComputerId",
                table: "Printers",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_Printers_ComputerSystemDataSystemDataID",
                table: "Printers",
                column: "ComputerSystemDataSystemDataID");

            migrationBuilder.CreateIndex(
                name: "IX_Scanners_ComputerId",
                table: "Scanners",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_Scanners_ComputerSystemDataSystemDataID",
                table: "Scanners",
                column: "ComputerSystemDataSystemDataID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentID",
                table: "Employees",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentID",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "AppUsageEntries");

            migrationBuilder.DropTable(
                name: "Gpus");

            migrationBuilder.DropTable(
                name: "Keyboards");

            migrationBuilder.DropTable(
                name: "KeyLogEntries");

            migrationBuilder.DropTable(
                name: "Mice");

            migrationBuilder.DropTable(
                name: "Monitors");

            migrationBuilder.DropTable(
                name: "Printers");

            migrationBuilder.DropTable(
                name: "Scanners");

            migrationBuilder.DropIndex(
                name: "IX_Computers_Host_Port",
                table: "Computers");

            migrationBuilder.DropColumn(
                name: "CpuClockMHz",
                table: "ComputerSystemDatas");

            migrationBuilder.DropColumn(
                name: "CpuCores",
                table: "ComputerSystemDatas");

            migrationBuilder.DropColumn(
                name: "CpuThreads",
                table: "ComputerSystemDatas");

            migrationBuilder.DropColumn(
                name: "OSCaption",
                table: "ComputerSystemDatas");

            migrationBuilder.DropColumn(
                name: "OSManufacturer",
                table: "ComputerSystemDatas");

            migrationBuilder.DropColumn(
                name: "WindowsDirectory",
                table: "ComputerSystemDatas");

            migrationBuilder.DropColumn(
                name: "Host",
                table: "Computers");

            migrationBuilder.DropColumn(
                name: "Port",
                table: "Computers");

            migrationBuilder.AlterColumn<string>(
                name: "OSVersion",
                table: "ComputerSystemDatas",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentID",
                table: "Employees",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
