using Microsoft.EntityFrameworkCore;

namespace TransportSystem.Api.Models.TransportChooseAlgorithm.QLearning.Storage.Sql
{
    public class QFuncContext : DbContext
    {
        public DbSet<QFuncInfo> QFuncInfos { get; set; }

        public QFuncContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=helloworld4db;Trusted_Connection=True;");
        }
    }
}