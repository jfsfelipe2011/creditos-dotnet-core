using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ControleCreditos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ControleCreditos.Models.Client> Client { get; set; }
        public DbSet<ControleCreditos.Models.Credit> Credit { get; set; }
        public DbSet<ControleCreditos.Models.Extract> Extract { get; set; }
    }
}
