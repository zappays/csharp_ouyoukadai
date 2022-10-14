using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OuyouKadai.Models;
using System;

namespace OuyouKadai.Data
{
    public class OuyouKadaiContext : DbContext
    {
        public OuyouKadaiContext (DbContextOptions<OuyouKadaiContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> TaskItem { get; set; }
        public DbSet<User>? User { get; set; }
        public DbSet<Auth>? Auth { get; set; }
        public DbSet<Status>? Status { get; set; }
        public DbSet<Priority>? Priority { get; set; }

    }
}
