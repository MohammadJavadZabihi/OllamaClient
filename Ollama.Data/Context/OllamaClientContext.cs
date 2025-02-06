using Microsoft.EntityFrameworkCore;
using Ollama.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ollama.Data.Entites
{
    public class OllamaClientContext : DbContext
    {
        public OllamaClientContext(DbContextOptions<OllamaClientContext> options) : base(options)
        {
            
        }
        public OllamaClientContext()
        {
        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=OllamaDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

    }
}
