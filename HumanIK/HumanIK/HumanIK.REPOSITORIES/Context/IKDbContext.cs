using HumanIK.ENTITIES.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.REPOSITORIES.Context
{
    public class IKDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public IKDbContext(DbContextOptions<IKDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Advance> Advances { get; set; }
        public DbSet<Expense> Expenses { get; set; }
    }
}
