using System;
using System.Collections.Generic;
using System.Text;
using BikeService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BikeService.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Request> Requests{ get; set; }
        public DbSet<RequestPos> RequestPos { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<RequestSymbol> RequestSymbol { get; set; }
        public DbSet<RequestImages> RequestImages { get; set; }
        public DbSet<Bike> Bike { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
