﻿// <auto-generated />
using System;
using LucidHR.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LucidHR.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LucidHR.Models.Career", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("FileData")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("city")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ctc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("doj")
                        .HasColumnType("datetime2");

                    b.Property<string>("expctc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("noticeperiod")
                        .HasColumnType("int");

                    b.Property<string>("reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("yrofexp")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Careers");
                });

            modelBuilder.Entity("LucidHR.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CustomColor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("LucidHR.Models.JoiningForm", b =>
                {
                    b.Property<int>("Jdid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Jdid"));

                    b.Property<string>("AadharAttachment")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Aadharno")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("Accountnumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Bankname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Bloodgroup")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime>("Dateofjoining")
                        .HasColumnType("datetime2");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<string>("Emergencycontactname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Emergencycontactnumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Fathersname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Ifsccode")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("Maritalstatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Pan")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PanAttachment")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Placeofbirth")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Relationwithperson")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Totalyearsofexperience")
                        .HasColumnType("int");

                    b.Property<string>("Uemail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("pphoto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Jdid");

                    b.ToTable("JoiningForm");
                });

            modelBuilder.Entity("LucidHR.Models.LeaveRequest", b =>
                {
                    b.Property<int>("sr")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("sr"));

                    b.Property<int>("absentleavedays")
                        .HasColumnType("int");

                    b.Property<int>("applyleavedays")
                        .HasColumnType("int");

                    b.Property<int>("balleavedays")
                        .HasColumnType("int");

                    b.Property<DateTime>("leavefromdate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("leavetodate")
                        .HasColumnType("datetime2");

                    b.Property<string>("lstatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("uemail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("uid")
                        .HasColumnType("int");

                    b.Property<string>("uname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("sr");

                    b.ToTable("LeaveRequest");
                });

            modelBuilder.Entity("LucidHR.Models.PerformanceReview", b =>
                {
                    b.Property<int>("ReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReviewId"));

                    b.Property<string>("AnswerText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int>("uid")
                        .HasColumnType("int");

                    b.HasKey("ReviewId");

                    b.HasIndex("QuestionId");

                    b.ToTable("PerformanceReviews");
                });

            modelBuilder.Entity("LucidHR.Models.Question", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestionId"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("uid")
                        .HasColumnType("int");

                    b.HasKey("QuestionId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("LucidHR.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FromEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromRole")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("GetDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("TicketDetail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToRole")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("solution")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("LucidHR.Models.f16", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("attachment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("f16s");
                });

            modelBuilder.Entity("LucidHR.Models.userLogin", b =>
                {
                    b.Property<int>("uid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("uid"));

                    b.Property<string>("uemail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("uname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("upass")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("urole")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("uid");

                    b.ToTable("userLogin");
                });

            modelBuilder.Entity("LucidHR.Models.PerformanceReview", b =>
                {
                    b.HasOne("LucidHR.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });
#pragma warning restore 612, 618
        }
    }
}