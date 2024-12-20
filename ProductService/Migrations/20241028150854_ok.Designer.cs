﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductService.Models;

namespace ProductService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241028150854_ok")]
    partial class ok
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("CommentUser", b =>
                {
                    b.Property<string>("LikesId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("LikesId1")
                        .HasColumnType("int");

                    b.HasKey("LikesId", "LikesId1");

                    b.HasIndex("LikesId1");

                    b.ToTable("CommentUser");
                });

            modelBuilder.Entity("ProductService.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ProductService.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Star")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ProductService.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("DiscountPercent")
                        .HasColumnType("int");

                    b.Property<bool>("Enabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Expiry")
                        .HasColumnType("int");

                    b.Property<string>("FullDescription")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Origin")
                        .HasColumnType("longtext");

                    b.Property<float>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("longtext");

                    b.Property<int>("SoldQuantity")
                        .HasColumnType("int");

                    b.Property<string>("Thumbnail")
                        .HasColumnType("longtext");

                    b.Property<int?>("UnitId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UnitId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ProductService.Models.ProductDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductDetails");
                });

            modelBuilder.Entity("ProductService.Models.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("ProductService.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Avatar")
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CommentUser", b =>
                {
                    b.HasOne("ProductService.Models.User", null)
                        .WithMany()
                        .HasForeignKey("LikesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProductService.Models.Comment", null)
                        .WithMany()
                        .HasForeignKey("LikesId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProductService.Models.Comment", b =>
                {
                    b.HasOne("ProductService.Models.Product", "Product")
                        .WithMany("Comments")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProductService.Models.User", "User")
                        .WithMany("Commnents")
                        .HasForeignKey("UserId");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProductService.Models.Product", b =>
                {
                    b.HasOne("ProductService.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProductService.Models.Unit", "Unit_")
                        .WithMany("Products")
                        .HasForeignKey("UnitId");

                    b.Navigation("Category");

                    b.Navigation("Unit_");
                });

            modelBuilder.Entity("ProductService.Models.ProductDetail", b =>
                {
                    b.HasOne("ProductService.Models.Product", "Product")
                        .WithMany("ProductDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ProductService.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("ProductService.Models.Product", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("ProductDetails");
                });

            modelBuilder.Entity("ProductService.Models.Unit", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("ProductService.Models.User", b =>
                {
                    b.Navigation("Commnents");
                });
#pragma warning restore 612, 618
        }
    }
}
