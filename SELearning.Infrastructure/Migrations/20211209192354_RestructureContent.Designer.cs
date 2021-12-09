﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SELearning.Infrastructure;

#nullable disable

namespace SELearning.Infrastructure.Migrations
{
    [DbContext(typeof(SELearningContext))]
    [Migration("20211209192354_RestructureContent")]
    partial class RestructureContent
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SELearning.Core.Comment.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AuthorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("ContentId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ContentId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("SELearning.Core.Content.Content", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AuthorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int?>("SectionId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VideoLink")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("SectionId");

                    b.ToTable("Content");
                });

            modelBuilder.Entity("SELearning.Core.Content.Section", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Section");
                });

            modelBuilder.Entity("SELearning.Core.User.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SELearning.Core.Comment.Comment", b =>
                {
                    b.HasOne("SELearning.Core.User.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("SELearning.Core.Content.Content", "Content")
                        .WithMany()
                        .HasForeignKey("ContentId");

                    b.Navigation("Author");

                    b.Navigation("Content");
                });

            modelBuilder.Entity("SELearning.Core.Content.Content", b =>
                {
                    b.HasOne("SELearning.Core.User.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("SELearning.Core.Content.Section", "Section")
                        .WithMany("Content")
                        .HasForeignKey("SectionId");

                    b.Navigation("Author");

                    b.Navigation("Section");
                });

            modelBuilder.Entity("SELearning.Core.Content.Section", b =>
                {
                    b.Navigation("Content");
                });
#pragma warning restore 612, 618
        }
    }
}