﻿// <auto-generated />
using System;
using LocacaoDesafioBackEnd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LocacaoDesafioBackEnd.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241021131620_InitialMigrationCreate")]
    partial class InitialMigrationCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LocacaoDesafioBackEnd.Models.Entregador", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "cnpj");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "data_nascimento");

                    b.Property<string>("Identificador")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "identificador");

                    b.Property<string>("ImagemCNH")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "imagem_cnh");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "nome");

                    b.Property<string>("NumeroCNH")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "numero_cnh");

                    b.Property<string>("TipoCNH")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "tipo_cnh");

                    b.HasKey("Id");

                    b.ToTable("entregadores", (string)null);
                });

            modelBuilder.Entity("LocacaoDesafioBackEnd.Models.Locacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataDevolucao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataLocacao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EntregadorId")
                        .HasColumnType("integer");

                    b.Property<int>("MotoId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EntregadorId");

                    b.HasIndex("MotoId");

                    b.ToTable("locacoes", (string)null);
                });

            modelBuilder.Entity("LocacaoDesafioBackEnd.Models.Moto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Disponivel")
                        .HasColumnType("boolean");

                    b.Property<string>("Marca")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.ToTable("motos", (string)null);
                });

            modelBuilder.Entity("LocacaoDesafioBackEnd.Models.Locacao", b =>
                {
                    b.HasOne("LocacaoDesafioBackEnd.Models.Entregador", "Entregador")
                        .WithMany()
                        .HasForeignKey("EntregadorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LocacaoDesafioBackEnd.Models.Moto", "Moto")
                        .WithMany()
                        .HasForeignKey("MotoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entregador");

                    b.Navigation("Moto");
                });
#pragma warning restore 612, 618
        }
    }
}
