using Hubbetech.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hubbetech.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Demanda> Demandas { get; set; }
        public DbSet<Equipamento> Equipamentos { get; set; }
        public DbSet<LinkEmpresa> LinksEmpresa { get; set; }
    }
}
