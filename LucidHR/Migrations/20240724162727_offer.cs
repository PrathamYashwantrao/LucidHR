using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LucidHR.Migrations
{
    /// <inheritdoc />
    public partial class offer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Careers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    yrofexp = table.Column<int>(type: "int", nullable: true),
                    noticeperiod = table.Column<int>(type: "int", nullable: true),
                    doj = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ctc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expctc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Careers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomColor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "f16s",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    attachment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_f16s", x => x.id);
                });

            //migrationBuilder.CreateTable(
            //    name: "JoiningForm",
            //    columns: table => new
            //    {
            //        Jdid = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Fullname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        Designation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Dateofjoining = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Totalyearsofexperience = table.Column<int>(type: "int", nullable: false),
            //        Contact = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
            //        Emergencycontactname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Emergencycontactnumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
            //        Relationwithperson = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Uemail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
            //        Fathersname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Maritalstatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //        Bloodgroup = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
            //        Pan = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
            //        Aadharno = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
            //        Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Placeofbirth = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Bankname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Ifsccode = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
            //        Accountnumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
            //        AadharAttachment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        PanAttachment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        pphoto = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_JoiningForm", x => x.Jdid);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "LeaveRequest",
            //    columns: table => new
            //    {
            //        sr = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        uemail = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        uid = table.Column<int>(type: "int", nullable: false),
            //        uname = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        leavefromdate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        leavetodate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        applyleavedays = table.Column<int>(type: "int", nullable: false),
            //        balleavedays = table.Column<int>(type: "int", nullable: false),
            //        absentleavedays = table.Column<int>(type: "int", nullable: false),
            //        lstatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_LeaveRequest", x => x.sr);
            //    });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uid = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TicketDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    solution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GetDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "userLogin",
            //    columns: table => new
            //    {
            //        uid = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        uname = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        uemail = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        urole = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        upass = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_userLogin", x => x.uid);
            //    });

            migrationBuilder.CreateTable(
                name: "PerformanceReviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uid = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceReviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_QuestionId",
                table: "PerformanceReviews",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Careers");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "f16s");

            migrationBuilder.DropTable(
                name: "JoiningForm");

            migrationBuilder.DropTable(
                name: "LeaveRequest");

            migrationBuilder.DropTable(
                name: "PerformanceReviews");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "userLogin");

            migrationBuilder.DropTable(
                name: "Questions");
        }
    }
}
