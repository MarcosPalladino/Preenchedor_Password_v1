using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPPreenchedor.Data.Models;

namespace TPPreenchedor.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Configurando o caminho do banco de dados SQLite
        private const string ConnectionString = "Data Source=TpPreenchedor.db";

        public ApplicationDbContext() : base()
        {
        }

        // Mapeamento das entidades
        public DbSet<Usuario> Usuarios { get; set; }

        // Configurando o SQLite
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }
    }
}
