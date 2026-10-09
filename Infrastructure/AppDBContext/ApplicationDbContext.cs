using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ShipmentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AppDBContext
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        public DbSet<Shipment> Shipments { get; set; }

    }
}
