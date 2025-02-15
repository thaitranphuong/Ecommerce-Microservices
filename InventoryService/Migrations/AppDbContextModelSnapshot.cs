﻿// <auto-generated />
using System;
using InventoryService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InventoryService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("InventoryService.Models.Export", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Reason")
                        .HasColumnType("longtext");

                    b.Property<string>("ReceiverName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("WarehouseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Exports");
                });

            modelBuilder.Entity("InventoryService.Models.ExportDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ExportId")
                        .HasColumnType("int");

                    b.Property<float>("Price")
                        .HasColumnType("float");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExportId");

                    b.ToTable("ExportDetails");
                });

            modelBuilder.Entity("InventoryService.Models.Import", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("WarehouseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SupplierId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Imports");
                });

            modelBuilder.Entity("InventoryService.Models.ImportDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ExportedQuantity")
                        .HasColumnType("int");

                    b.Property<int>("ImportId")
                        .HasColumnType("int");

                    b.Property<string>("Position")
                        .HasColumnType("longtext");

                    b.Property<float>("Price")
                        .HasColumnType("float");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ImportId");

                    b.ToTable("ImportDetails");
                });

            modelBuilder.Entity("InventoryService.Models.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("InventoryService.Models.Warehouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("InventoryService.Models.Export", b =>
                {
                    b.HasOne("InventoryService.Models.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("InventoryService.Models.ExportDetail", b =>
                {
                    b.HasOne("InventoryService.Models.Export", "Export")
                        .WithMany("ExportDetails")
                        .HasForeignKey("ExportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Export");
                });

            modelBuilder.Entity("InventoryService.Models.Import", b =>
                {
                    b.HasOne("InventoryService.Models.Supplier", "Supplier")
                        .WithMany("Imports")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventoryService.Models.Warehouse", "Warehouse")
                        .WithMany("Imports")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Supplier");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("InventoryService.Models.ImportDetail", b =>
                {
                    b.HasOne("InventoryService.Models.Import", "Import")
                        .WithMany("ImportDetails")
                        .HasForeignKey("ImportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Import");
                });

            modelBuilder.Entity("InventoryService.Models.Export", b =>
                {
                    b.Navigation("ExportDetails");
                });

            modelBuilder.Entity("InventoryService.Models.Import", b =>
                {
                    b.Navigation("ImportDetails");
                });

            modelBuilder.Entity("InventoryService.Models.Supplier", b =>
                {
                    b.Navigation("Imports");
                });

            modelBuilder.Entity("InventoryService.Models.Warehouse", b =>
                {
                    b.Navigation("Imports");
                });
#pragma warning restore 612, 618
        }
    }
}
