﻿// <auto-generated />
using DVB_Bot.Data.EfModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DVB_Bot.Data.EfModel.Migrations
{
    [DbContext(typeof(DvbBotContext))]
    [Migration("20200926170203_InitDvbBotDB")]
    partial class InitDvbBotDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DVB_Bot.Shared.Model.FavoriteStops", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<int>("StopId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StopId");

                    b.ToTable("FavoriteStops");
                });

            modelBuilder.Entity("DVB_Bot.Shared.Model.Stop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlDeparture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlStop")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Stops");
                });

            modelBuilder.Entity("DVB_Bot.Shared.Model.FavoriteStops", b =>
                {
                    b.HasOne("DVB_Bot.Shared.Model.Stop", "Stop")
                        .WithMany("FavoriteStops")
                        .HasForeignKey("StopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
