﻿// <auto-generated />
using CallAppTask.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CallAppTask.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231203185021_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CallAppTask.DTO.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EmailAdress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsActive = false,
                            Password = "Luke123",
                            UserName = "Luke",
                            UserProfileId = 0
                        });
                });

            modelBuilder.Entity("CallAppTask.DTO.User", b =>
                {
                    b.OwnsOne("CallAppTask.DTO.UserProfile", "UserProfile", b1 =>
                        {
                            b1.Property<int>("Id")
                                .HasColumnType("int");

                            b1.Property<string>("FirstName")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("LastName")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PersonalNumber")
                                .IsRequired()
                                .HasMaxLength(11)
                                .HasColumnType("nvarchar(11)");

                            b1.HasKey("Id");

                            b1.ToTable("UserProfile");

                            b1.WithOwner()
                                .HasForeignKey("Id");

                            b1.HasData(
                                new
                                {
                                    Id = 1,
                                    PersonalNumber = "01010039867"
                                });
                        });

                    b.Navigation("UserProfile");
                });
#pragma warning restore 612, 618
        }
    }
}
