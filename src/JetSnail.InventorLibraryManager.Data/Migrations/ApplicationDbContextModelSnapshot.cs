﻿// <auto-generated />
using System;
using JetSnail.InventorLibraryManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JetSnail.InventorLibraryManager.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("JetSnail.InventorLibraryManager.Core.Entities.DerivativeEntity", b =>
                {
                    b.Property<string>("FamilyId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LibraryId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PrototypeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SynchronizedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("FamilyId", "LibraryId");

                    b.HasIndex("PrototypeId");

                    b.ToTable("Derivative");
                });

            modelBuilder.Entity("JetSnail.InventorLibraryManager.Core.Entities.GroupEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ShortName")
                        .IsUnique()
                        .HasFilter("[ShortName] IS NOT NULL");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("JetSnail.InventorLibraryManager.Core.Entities.PartEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("FamilyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("PartId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("PrototypeFamilyEntityId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasAlternateKey("PartId", "FamilyId");

                    b.HasIndex("PrototypeFamilyEntityId");

                    b.ToTable("Part");
                });

            modelBuilder.Entity("JetSnail.InventorLibraryManager.Core.Entities.PrototypeFamilyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("FamilyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("GroupModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LibraryId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasAlternateKey("FamilyId");

                    b.HasIndex("GroupId");

                    b.ToTable("Prototype");
                });

            modelBuilder.Entity("JetSnail.InventorLibraryManager.Core.Entities.DerivativeEntity", b =>
                {
                    b.HasOne("JetSnail.InventorLibraryManager.Core.Entities.PrototypeFamilyEntity", "Prototype")
                        .WithMany("Derivatives")
                        .HasForeignKey("PrototypeId");

                    b.Navigation("Prototype");
                });

            modelBuilder.Entity("JetSnail.InventorLibraryManager.Core.Entities.PartEntity", b =>
                {
                    b.HasOne("JetSnail.InventorLibraryManager.Core.Entities.PrototypeFamilyEntity", null)
                        .WithMany("Parts")
                        .HasForeignKey("PrototypeFamilyEntityId");
                });

            modelBuilder.Entity("JetSnail.InventorLibraryManager.Core.Entities.PrototypeFamilyEntity", b =>
                {
                    b.HasOne("JetSnail.InventorLibraryManager.Core.Entities.GroupEntity", "Group")
                        .WithMany("Prototypes")
                        .HasForeignKey("GroupId");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("JetSnail.InventorLibraryManager.Core.Entities.GroupEntity", b =>
                {
                    b.Navigation("Prototypes");
                });

            modelBuilder.Entity("JetSnail.InventorLibraryManager.Core.Entities.PrototypeFamilyEntity", b =>
                {
                    b.Navigation("Derivatives");

                    b.Navigation("Parts");
                });
#pragma warning restore 612, 618
        }
    }
}
