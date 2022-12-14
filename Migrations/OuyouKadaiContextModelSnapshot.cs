// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OuyouKadai.Data;

#nullable disable

namespace OuyouKadai.Migrations
{
    [DbContext(typeof(OuyouKadaiContext))]
    partial class OuyouKadaiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("OuyouKadai.Models.Auth", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Auth_name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(10)");

                    b.HasKey("Id");

                    b.ToTable("Auth");
                });

            modelBuilder.Entity("OuyouKadai.Models.Priority", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Priority_name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(10)");

                    b.HasKey("Id");

                    b.ToTable("Priority");
                });

            modelBuilder.Entity("OuyouKadai.Models.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Status_name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(10)");

                    b.HasKey("Id");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("OuyouKadai.Models.TaskItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("PicID")
                        .HasColumnType("int");

                    b.Property<int>("PriorityID")
                        .HasColumnType("int");

                    b.Property<int>("RegID")
                        .HasColumnType("int");

                    b.Property<int>("StatusID")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime?>("Updated_at")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("Updated_byID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PicID");

                    b.HasIndex("PriorityID");

                    b.HasIndex("RegID");

                    b.HasIndex("StatusID");

                    b.ToTable("TaskItem");
                });

            modelBuilder.Entity("OuyouKadai.Models.User", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Pass")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.Property<int>("RegID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Updated_at")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("Updated_byID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("OuyouKadai.Models.TaskItem", b =>
                {
                    b.HasOne("OuyouKadai.Models.User", "Pic")
                        .WithMany()
                        .HasForeignKey("PicID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OuyouKadai.Models.Priority", "Priority")
                        .WithMany("TaskItems")
                        .HasForeignKey("PriorityID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OuyouKadai.Models.User", "Reg")
                        .WithMany()
                        .HasForeignKey("RegID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OuyouKadai.Models.Status", "Status")
                        .WithMany("TaskItems")
                        .HasForeignKey("StatusID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pic");

                    b.Navigation("Priority");

                    b.Navigation("Reg");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("OuyouKadai.Models.User", b =>
                {
                    b.HasOne("OuyouKadai.Models.Auth", "Auth")
                        .WithMany("Users")
                        .HasForeignKey("AuthID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Auth");
                });

            modelBuilder.Entity("OuyouKadai.Models.Auth", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("OuyouKadai.Models.Priority", b =>
                {
                    b.Navigation("TaskItems");
                });

            modelBuilder.Entity("OuyouKadai.Models.Status", b =>
                {
                    b.Navigation("TaskItems");
                });
#pragma warning restore 612, 618
        }
    }
}
