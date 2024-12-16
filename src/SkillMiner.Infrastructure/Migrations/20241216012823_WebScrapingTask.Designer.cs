﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillMiner.Infrastructure.Persistence;

#nullable disable

namespace SkillMiner.Infrastructure.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20241216012823_WebScrapingTask")]
    partial class WebScrapingTask
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SkillMiner.Application.Abstractions.CommandQueue.CommandQueueMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CommandQueueMessages", (string)null);
                });

            modelBuilder.Entity("SkillMiner.Domain.Entities.JobListingEntity.JobListing", b =>
                {
                    b.Property<int>("DatabaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DatabaseId"));

                    b.Property<DateTime?>("ClosingOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmploymentType")
                        .HasColumnType("int");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Industry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PostedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("SalaryMax")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("SalaryMin")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Tags")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("WebScrapingTaskId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DatabaseId");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("IX_JobListing_Id");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Id"), false);

                    b.ToTable("JobListing", (string)null);
                });

            modelBuilder.Entity("SkillMiner.Domain.Entities.WebScrapingTaskEntity.WebScrapingTask", b =>
                {
                    b.Property<int>("DatabaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DatabaseId"));

                    b.Property<DateTime?>("CompletedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("StartedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedOnUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("DatabaseId");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("IX_WebScrapingTask_Id");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Id"), false);

                    b.ToTable("WebScrapingTask", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
