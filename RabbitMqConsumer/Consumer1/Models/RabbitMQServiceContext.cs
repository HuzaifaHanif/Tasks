﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RabbitMQConsumer1.Models
{
    public partial class RabbitMQServiceContext : DbContext
    {
        public RabbitMQServiceContext()
        {
        }

        public RabbitMQServiceContext(DbContextOptions<RabbitMQServiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RabbitMq> RabbitMqs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=VID-DT-1051;Initial Catalog=SoftechWorldWide;Integrated Security=True;Trust Server Certificate=True;Encrypt=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RabbitMq>(entity =>
            {
                entity.ToTable("RabbitMQ");

                entity.Property(e => e.ConsumerName).IsRequired();

                entity.Property(e => e.Exchange).IsRequired();

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Queue).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}