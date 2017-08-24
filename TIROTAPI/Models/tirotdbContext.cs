using System;
using Microsoft.EntityFrameworkCore;
//using Npgsql.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;

namespace TIROTAPI.Models
{
    public partial class tirotdbContext : DbContext
    {
        public tirotdbContext(DbContextOptions<tirotdbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbIoTactivityLog>(entity =>
            {
                entity.HasKey(e => new { e.MachineId, e.ClientDateTime, e.SensorId })
                    .HasName("PK_TbIoTActivityLog");

                entity.ToTable("TbIoTActivityLog");

                entity.Property(e => e.MachineId).HasColumnName("MachineID");

                entity.Property(e => e.SensorId).HasColumnName("SensorID");

                entity.Property(e => e.ServerDateTime).HasColumnName("ServerDateTIme");
            });

            modelBuilder.Entity<TbIoTdevice>(entity =>
            {
                entity.HasKey(e => e.IoTdeviceId)
                    .HasName("PK_TbIoTDevice");

                entity.ToTable("TbIoTDevice");

                entity.HasIndex(e => e.IoTdeviceMacAddress)
                    .HasName("IX_TbIoTDevice_MacAddress");

                entity.Property(e => e.IoTdeviceId).HasColumnName("IoTDeviceID");

                entity.Property(e => e.IoTdeviceDesc).HasColumnName("IoTDeviceDesc");

                entity.Property(e => e.IoTdeviceMacAddress)
                    .IsRequired()
                    .HasColumnName("IoTDeviceMacAddress")
                    .HasColumnType("bpchar")
                    .HasMaxLength(12);

                entity.Property(e => e.IoTmodelId).HasColumnName("IoTModelID");

                entity.Property(e => e.MachineId).HasColumnName("MachineID");

                entity.HasOne(d => d.IoTmodel)
                    .WithMany(p => p.TbIoTdevice)
                    .HasForeignKey(d => d.IoTmodelId)
                    .HasConstraintName("FP_IoTDevice_IoTModel");
            });

            modelBuilder.Entity<TbIoTdevicePort>(entity =>
            {
                entity.HasKey(e => new { e.IoTdeviceMacAddress, e.IoTdevicePort })
                    .HasName("PK_TbIoTDevicePort");

                entity.ToTable("TbIoTDevicePort");

                entity.Property(e => e.IoTdeviceMacAddress)
                    .HasColumnName("IoTDeviceMacAddress")
                    .HasColumnType("bpchar")
                    .HasMaxLength(12);

                entity.Property(e => e.IoTdevicePort).HasColumnName("IoTDevicePort");

                entity.Property(e => e.MachineId).HasColumnName("MachineID");

                entity.Property(e => e.MeasurementId).HasColumnName("MeasurementID");

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.TbIoTdevicePort)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("FK_TbIoTDevicePort_TbMechine");
            });

            modelBuilder.Entity<TbIoTmodel>(entity =>
            {
                entity.HasKey(e => e.IoTmodelId)
                    .HasName("PK_TbIoTModel");

                entity.ToTable("TbIoTModel");

                entity.Property(e => e.IoTmodelId).HasColumnName("IoTModelID");

                entity.Property(e => e.InputChannel).HasDefaultValueSql("0");

                entity.Property(e => e.IoTmodelName)
                    .HasColumnName("IoTModelName")
                    .HasColumnType("varchar")
                    .HasMaxLength(256);

                entity.Property(e => e.OutputChannel).HasDefaultValueSql("0");
            });

            modelBuilder.Entity<TbMachine>(entity =>
            {
                entity.HasKey(e => e.MachineId)
                    .HasName("PK_TbMachine");

                entity.Property(e => e.MachineId).HasColumnName("MachineID");

                entity.Property(e => e.MachineName)
                    .HasColumnType("varchar")
                    .HasMaxLength(256);

                entity.Property(e => e.MachineWorkSchduleType)
                    .IsRequired()
                    .HasColumnType("bpchar")
                    .HasMaxLength(1)
                    .HasDefaultValueSql("0");

                entity.Property(e => e.MachineWorkStatus)
                    .IsRequired()
                    .HasColumnType("bpchar")
                    .HasMaxLength(1)
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<TbUdlog>(entity =>
            {
                entity.ToTable("TbUDLog");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Indata).HasColumnName("INData");
            });
        }

        public virtual DbSet<TbIoTactivityLog> TbIoTactivityLog { get; set; }
        public virtual DbSet<TbIoTdevice> TbIoTdevice { get; set; }
        public virtual DbSet<TbIoTdevicePort> TbIoTdevicePort { get; set; }
        public virtual DbSet<TbIoTmodel> TbIoTmodel { get; set; }
        public virtual DbSet<TbMachine> TbMachine { get; set; }
        public virtual DbSet<TbUdlog> TbUdlog { get; set; }
    }
}