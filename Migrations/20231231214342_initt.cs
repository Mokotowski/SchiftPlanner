using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchiftPlanner.Migrations
{
    /// <inheritdoc />
    public partial class initt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Type_Subscriptions",
                columns: table => new
                {
                    Id_Sub = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    DateLenght = table.Column<int>(type: "int", nullable: false),
                    MaxPlann = table.Column<int>(type: "int", nullable: false),
                    MaxPersonforPlann = table.Column<int>(type: "int", nullable: false),
                    TypeCompany = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type_Subscriptions", x => x.Id_Sub);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id_Company = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_User = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id_Sub = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AutoRenew = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id_Company);
                    table.ForeignKey(
                        name: "FK_Subscriptions_AspNetUsers_Id_User",
                        column: x => x.Id_User,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Type_Subscriptions_Id_Sub",
                        column: x => x.Id_Sub,
                        principalTable: "Type_Subscriptions",
                        principalColumn: "Id_Sub",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyInfo",
                columns: table => new
                {
                    Id_Company = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInfo", x => x.Id_Company);
                    table.ForeignKey(
                        name: "FK_CompanyInfo_Subscriptions_Id_Company",
                        column: x => x.Id_Company,
                        principalTable: "Subscriptions",
                        principalColumn: "Id_Company",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Company_Type1",
                columns: table => new
                {
                    Id_Company = table.Column<int>(type: "int", nullable: false),
                    Id_user = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Work_Group = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Type1", x => x.Id_Company);
                    table.ForeignKey(
                        name: "FK_Company_Type1_CompanyInfo_Id_Company",
                        column: x => x.Id_Company,
                        principalTable: "CompanyInfo",
                        principalColumn: "Id_Company",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Company_Type2",
                columns: table => new
                {
                    Id_Company = table.Column<int>(type: "int", nullable: false),
                    Id_user = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company_Type2", x => x.Id_Company);
                    table.ForeignKey(
                        name: "FK_Company_Type2_CompanyInfo_Id_Company",
                        column: x => x.Id_Company,
                        principalTable: "CompanyInfo",
                        principalColumn: "Id_Company",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Opinions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Company = table.Column<int>(type: "int", nullable: false),
                    Anonymously = table.Column<bool>(type: "bit", nullable: false),
                    Id_user = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAdd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opinions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Opinions_CompanyInfo_Id_Company",
                        column: x => x.Id_Company,
                        principalTable: "CompanyInfo",
                        principalColumn: "Id_Company",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveysProperties",
                columns: table => new
                {
                    Id_Survey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Anonymously = table.Column<bool>(type: "bit", nullable: false),
                    Everyone = table.Column<bool>(type: "bit", nullable: false),
                    Id_Company = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveysProperties", x => x.Id_Survey);
                    table.ForeignKey(
                        name: "FK_SurveysProperties_CompanyInfo_Id_Company",
                        column: x => x.Id_Company,
                        principalTable: "CompanyInfo",
                        principalColumn: "Id_Company",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Worker_Timetable",
                columns: table => new
                {
                    Id_Timetable = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Company = table.Column<int>(type: "int", nullable: false),
                    Simultant = table.Column<int>(type: "int", nullable: false),
                    Column = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worker_Timetable", x => x.Id_Timetable);
                    table.ForeignKey(
                        name: "FK_Worker_Timetable_Company_Type1_Id_Company",
                        column: x => x.Id_Company,
                        principalTable: "Company_Type1",
                        principalColumn: "Id_Company",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Work_Group_User = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id_user = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Work_Group = table.Column<int>(type: "int", nullable: false),
                    Company_Type1Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Work_Group_User);
                    table.ForeignKey(
                        name: "FK_Workers_Company_Type1_Company_Type1Id",
                        column: x => x.Company_Type1Id,
                        principalTable: "Company_Type1",
                        principalColumn: "Id_Company",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer_Timetable",
                columns: table => new
                {
                    Id_Timetable = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Company = table.Column<int>(type: "int", nullable: false),
                    Break_after_Client = table.Column<int>(type: "int", nullable: false),
                    Column = table.Column<int>(type: "int", nullable: false),
                    Simultant = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer_Timetable", x => x.Id_Timetable);
                    table.ForeignKey(
                        name: "FK_Customer_Timetable_Company_Type2_Id_Company",
                        column: x => x.Id_Company,
                        principalTable: "Company_Type2",
                        principalColumn: "Id_Company",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Question_Location = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id_Survey = table.Column<int>(type: "int", nullable: false),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false),
                    Question_Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Question_Location);
                    table.ForeignKey(
                        name: "FK_Question_SurveysProperties_Id_Survey",
                        column: x => x.Id_Survey,
                        principalTable: "SurveysProperties",
                        principalColumn: "Id_Survey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Day_Worker_Timetable",
                columns: table => new
                {
                    Timetable_Day = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id_Timetable = table.Column<int>(type: "int", nullable: false),
                    IsWork = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Day_Worker_Timetable", x => x.Timetable_Day);
                    table.ForeignKey(
                        name: "FK_Day_Worker_Timetable_Worker_Timetable_Id_Timetable",
                        column: x => x.Id_Timetable,
                        principalTable: "Worker_Timetable",
                        principalColumn: "Id_Timetable",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Day_Customer_Timetable",
                columns: table => new
                {
                    Timetable_Day = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id_Timetable = table.Column<int>(type: "int", nullable: false),
                    IsWork = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeStart = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeEnd = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Day_Customer_Timetable", x => x.Timetable_Day);
                    table.ForeignKey(
                        name: "FK_Day_Customer_Timetable_Customer_Timetable_Id_Timetable",
                        column: x => x.Id_Timetable,
                        principalTable: "Customer_Timetable",
                        principalColumn: "Id_Timetable",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Option_Answer",
                columns: table => new
                {
                    Question_Location = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Question_Location_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text_Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option_Answer", x => x.Question_Location);
                    table.ForeignKey(
                        name: "FK_Option_Answer_Question_Question_Location",
                        column: x => x.Question_Location,
                        principalTable: "Question",
                        principalColumn: "Question_Location",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Day_Worker_Claimed",
                columns: table => new
                {
                    Timetable_Day_User = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Timetable_Day = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TimeStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id_User = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Day_Worker_Claimed", x => x.Timetable_Day_User);
                    table.ForeignKey(
                        name: "FK_Day_Worker_Claimed_Day_Worker_Timetable_Timetable_Day",
                        column: x => x.Timetable_Day,
                        principalTable: "Day_Worker_Timetable",
                        principalColumn: "Timetable_Day",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Day_Customer_Claimed",
                columns: table => new
                {
                    Timetable_Day_User = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id_Timetable = table.Column<int>(type: "int", nullable: false),
                    Timetable_Day = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeStart = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeEnd = table.Column<TimeSpan>(type: "time", nullable: false),
                    Id_User = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Day_Customer_Claimed", x => x.Timetable_Day_User);
                    table.ForeignKey(
                        name: "FK_Day_Customer_Claimed_Day_Customer_Timetable_Timetable_Day",
                        column: x => x.Timetable_Day,
                        principalTable: "Day_Customer_Timetable",
                        principalColumn: "Timetable_Day",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Timetable_Id_Company",
                table: "Customer_Timetable",
                column: "Id_Company");

            migrationBuilder.CreateIndex(
                name: "IX_Day_Customer_Claimed_Timetable_Day",
                table: "Day_Customer_Claimed",
                column: "Timetable_Day");

            migrationBuilder.CreateIndex(
                name: "IX_Day_Customer_Timetable_Id_Timetable",
                table: "Day_Customer_Timetable",
                column: "Id_Timetable");

            migrationBuilder.CreateIndex(
                name: "IX_Day_Worker_Claimed_Timetable_Day",
                table: "Day_Worker_Claimed",
                column: "Timetable_Day");

            migrationBuilder.CreateIndex(
                name: "IX_Day_Worker_Timetable_Id_Timetable",
                table: "Day_Worker_Timetable",
                column: "Id_Timetable");

            migrationBuilder.CreateIndex(
                name: "IX_Opinions_Id_Company",
                table: "Opinions",
                column: "Id_Company");

            migrationBuilder.CreateIndex(
                name: "IX_Question_Id_Survey",
                table: "Question",
                column: "Id_Survey");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Id_Sub",
                table: "Subscriptions",
                column: "Id_Sub");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Id_User",
                table: "Subscriptions",
                column: "Id_User");

            migrationBuilder.CreateIndex(
                name: "IX_SurveysProperties_Id_Company",
                table: "SurveysProperties",
                column: "Id_Company");

            migrationBuilder.CreateIndex(
                name: "IX_Worker_Timetable_Id_Company",
                table: "Worker_Timetable",
                column: "Id_Company");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_Company_Type1Id",
                table: "Workers",
                column: "Company_Type1Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Day_Customer_Claimed");

            migrationBuilder.DropTable(
                name: "Day_Worker_Claimed");

            migrationBuilder.DropTable(
                name: "Opinions");

            migrationBuilder.DropTable(
                name: "Option_Answer");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Day_Customer_Timetable");

            migrationBuilder.DropTable(
                name: "Day_Worker_Timetable");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Customer_Timetable");

            migrationBuilder.DropTable(
                name: "Worker_Timetable");

            migrationBuilder.DropTable(
                name: "SurveysProperties");

            migrationBuilder.DropTable(
                name: "Company_Type2");

            migrationBuilder.DropTable(
                name: "Company_Type1");

            migrationBuilder.DropTable(
                name: "CompanyInfo");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Type_Subscriptions");
        }
    }
}
