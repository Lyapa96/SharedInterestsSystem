using Microsoft.EntityFrameworkCore;

namespace TransportSystem.Api.Models.TransportChooseAlgorithms.QLearning.Storage.Sql
{
    public class QFuncContext : DbContext
    {
        public QFuncContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<QFuncInfo> QFuncInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=helloworld4db;Trusted_Connection=True;");
        }
    }
}