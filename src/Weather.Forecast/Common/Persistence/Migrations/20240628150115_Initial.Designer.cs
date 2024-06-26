﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Weather.Forecast.Common.Persistence;

#nullable disable

namespace Weather.Forecast.Common.Persistence.Migrations
{
    [DbContext(typeof(ForecastDbContext))]
    [Migration("20240628150115_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("forecast")
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Weather.Forecast.Feature.Forecast.Domain.WeatherForecast", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<Guid?>("MeteorologistId")
                        .HasColumnType("uuid");

                    b.Property<string>("Summary")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<decimal>("Temperature")
                        .HasPrecision(5, 2)
                        .HasColumnType("numeric(5,2)")
                        .HasColumnName("Celsius");

                    b.HasKey("Id");

                    b.HasIndex("MeteorologistId");

                    b.ToTable("WeatherForecast", "forecast");
                });

            modelBuilder.Entity("Weather.Forecast.Feature.Meteorologist.Domain.Meteorologist", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<int>("_forecastCount")
                        .HasColumnType("integer")
                        .HasColumnName("ForecastCount");

                    b.ComplexProperty<Dictionary<string, object>>("BirthDay", "Weather.Forecast.Feature.Meteorologist.Domain.Meteorologist.BirthDay#BirthDate", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<DateOnly>("Date")
                                .HasColumnType("date");

                            b1.Property<TimeOnly?>("Hour")
                                .HasColumnType("time without time zone");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Name", "Weather.Forecast.Feature.Meteorologist.Domain.Meteorologist.Name#Name", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Firstname")
                                .IsRequired()
                                .HasMaxLength(200)
                                .HasColumnType("character varying(200)")
                                .HasColumnName("Firstname");

                            b1.Property<string>("Lastname")
                                .IsRequired()
                                .HasMaxLength(200)
                                .HasColumnType("character varying(200)")
                                .HasColumnName("Lastname");
                        });

                    b.HasKey("Id");

                    b.ToTable("Meteorologist", "forecast");
                });

            modelBuilder.Entity("Weather.SharedKernel.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CompleteTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<DateTimeOffset>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<List<string>>("UncaughtExceptions")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.HasIndex("CompleteTime");

                    b.HasIndex("CreationTime")
                        .IsDescending();

                    b.ToTable("OutboxMessage", "forecast");
                });

            modelBuilder.Entity("Weather.Forecast.Feature.Forecast.Domain.WeatherForecast", b =>
                {
                    b.HasOne("Weather.Forecast.Feature.Meteorologist.Domain.Meteorologist", null)
                        .WithMany()
                        .HasForeignKey("MeteorologistId");
                });
#pragma warning restore 612, 618
        }
    }
}
