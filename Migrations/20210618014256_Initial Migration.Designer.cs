﻿// <auto-generated />
using System;
using CovidApiNigeria.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CovidApiNigeria.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210618014256_Initial Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CovidApiNigeria.Models.DataModel", b =>
                {
                    b.Property<string>("StateName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("NumberOfActiveCases")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfConfirmedCases")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfDeaths")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfDischarged")
                        .HasColumnType("int");

                    b.Property<int>("SamplesTested")
                        .HasColumnType("int");

                    b.Property<int>("TotalActiveCases")
                        .HasColumnType("int");

                    b.Property<int>("TotalConfirmedCases")
                        .HasColumnType("int");

                    b.Property<int>("TotalDeaths")
                        .HasColumnType("int");

                    b.Property<int>("TotalDischarged")
                        .HasColumnType("int");

                    b.HasKey("StateName", "Date");

                    b.ToTable("CovidNigeriaData");
                });
#pragma warning restore 612, 618
        }
    }
}