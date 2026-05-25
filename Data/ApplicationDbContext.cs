using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using TPPreenchedor.Data.Models;

namespace TPPreenchedor.Data
{
    public class ApplicationDbContext : DbContext
    {
        private const string DatabaseFileName = "TpPreenchedor.db";

        public static string DatabasePath
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "TPPreenchedor",
                    DatabaseFileName);
            }
        }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(DatabasePath));

            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = DatabasePath
            }.ToString();

            optionsBuilder.UseSqlite(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(usuario => usuario.Id);
                entity.HasIndex(usuario => usuario.Login).IsUnique();
                entity.Property(usuario => usuario.Login).IsRequired();
                entity.Property(usuario => usuario.Senha).IsRequired();
            });
        }
    }
}
